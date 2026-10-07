using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Nodig voor het New Input System

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Student & Scenario Settings")]
    public string currentStudentId = "CURSIST_123";
    public int totalPatientsInScenario = 5;

    [Header("Timer Settings")]
    [Tooltip("Tijd in seconden (bijv. 300 = 5 minuten)")]
    public float scenarioDuration = 300f;

    [Header("References")]
    public EvaluationSystem evaluationSystem;

    private TriageSessionData currentSession;
    private List<Patient3D> triagedPatients = new List<Patient3D>();
    private float startTime;
    private float timeRemaining;
    private bool isScenarioActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartScenario();
    }

    private void Update()
    {
        // Herstart direct met de '\' toets via het New Input System
        if (Keyboard.current != null && Keyboard.current.backslashKey.wasPressedThisFrame)
        {
            RestartScenario();
            return;
        }

        if (!isScenarioActive) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            if (UI_Controller.Instance != null)
            {
                UI_Controller.Instance.UpdateTimerDisplay(timeRemaining);
            }
        }
        else
        {
            timeRemaining = 0;
            isScenarioActive = false;

            if (UI_Controller.Instance != null)
            {
                UI_Controller.Instance.UpdateTimerDisplay(0);
            }

            EndScenario();
        }
    }

    public void StartScenario()
    {
        currentSession = new TriageSessionData
        {
            studentId = currentStudentId,
            protocolType = "TRIAGE",
            steps = new List<ProtocolStepLog>()
        };

        triagedPatients.Clear();
        startTime = Time.time;
        timeRemaining = scenarioDuration;
        isScenarioActive = true;
    }

    public void LogProtocolStep(string stepName, string patientId, bool isCorrect, string details)
    {
        if (currentSession == null) return;

        ProtocolStepLog log = new ProtocolStepLog
        {
            stepName = stepName,
            patientId = patientId,
            isCorrect = isCorrect,
            timestamp = Time.time - startTime,
            details = details
        };

        currentSession.steps.Add(log);
    }

    public void RegisterTriagedPatient(Patient3D patient)
    {
        if (!triagedPatients.Contains(patient))
        {
            triagedPatients.Add(patient);

            bool correct = (patient.isCriticallyIll && (patient.assignedCategory == TriageCategory.RED || patient.assignedCategory == TriageCategory.ORANGE)) ||
                           (!patient.isCriticallyIll && (patient.assignedCategory == TriageCategory.YELLOW || patient.assignedCategory == TriageCategory.GREEN));

            LogProtocolStep("ASSIGN_TRIAGE", patient.patientId, correct, $"Toegewezen: {patient.assignedCategory}");
        }

        if (triagedPatients.Count >= totalPatientsInScenario)
        {
            EndScenario();
        }
    }

    public void EndScenario()
    {
        if (!isScenarioActive) return;
        isScenarioActive = false;

        currentSession.completionTime = Time.time - startTime;

        if (evaluationSystem != null)
        {
            currentSession.finalScore = evaluationSystem.CalculateFinalScore(triagedPatients, currentSession.completionTime);
        }

        if (UI_Controller.Instance != null)
        {
            UI_Controller.Instance.ShowFinalResults(currentSession.finalScore, "Scenario Voltooid!");
        }

        currentSession.isPassed = currentSession.finalScore >= 70;

        if (APIService.Instance != null)
        {
            APIService.Instance.SendSessionData(currentSession);
        }
    }

    public void RestartScenario()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}