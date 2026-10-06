using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    private int currentScore;
    private float timeRemaining;
    private bool isGameActive;

    [Header("Settings")]
    public float scenarioDuration = 180f; // 3 minuten
    public int totalPatientsInScenario = 3;

    [Header("References")]
    public EvaluationSystem evaluationSystem;

    private List<Patient3D> triagedPatients = new List<Patient3D>();

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
        if (isGameActive)
        {
            UpdateTimer();
        }
    }

    public void StartScenario()
    {
        currentScore = 0;
        timeRemaining = scenarioDuration;
        isGameActive = true;
        triagedPatients.Clear();
    }

    private void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        UI_Controller.Instance.UpdateTimerDisplay(timeRemaining);

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            EndScenario();
        }
    }

    public void RegisterTriagedPatient(Patient3D patient)
    {
        if (!isGameActive || triagedPatients.Contains(patient)) return;

        triagedPatients.Add(patient);

        // Scenario eindigt als alle patiënten getrieerd zijn
        if (triagedPatients.Count >= totalPatientsInScenario)
        {
            EndScenario();
        }
    }

    public void EndScenario()
    {
        isGameActive = false;

        currentScore = evaluationSystem.CalculateFinalScore(triagedPatients, timeRemaining);
        string report = evaluationSystem.GenerateFeedbackReport();

        UI_Controller.Instance.ShowFinalResults(currentScore, report);
    }
}