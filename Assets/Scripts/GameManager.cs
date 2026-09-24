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
    public float scenarioDuration = 120f; // 2 minutes

    [Header("References")]
    public UI_Controller uiController;
    public EvaluationSystem evaluationSystem;

    private Queue<Patient> patientQueue = new Queue<Patient>();
    private List<Patient> processedPatients = new List<Patient>();
    private Patient currentPatient;

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
        processedPatients.Clear();

        // Mock data setup
        patientQueue.Enqueue(new Patient("P001", "Shortness of breath, chest pressure", true, new VitalSigns(110, "140/90", 88, 37.2f)));
        patientQueue.Enqueue(new Patient("P002", "Minor laceration on forearm", false, new VitalSigns(72, "120/80", 99, 36.6f)));
        patientQueue.Enqueue(new Patient("P003", "Severe abdominal pain and fever", true, new VitalSigns(105, "100/65", 95, 38.9f)));

        NextPatient();
    }

    private void NextPatient()
    {
        if (patientQueue.Count > 0)
        {
            currentPatient = patientQueue.Dequeue();
            uiController.DisplayPatientInfo(currentPatient);
        }
        else
        {
            EndScenario();
        }
    }

    public void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        uiController.UpdateTimerDisplay(timeRemaining);

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            EndScenario();
        }
    }

    public void ProcessTriageChoice(TriageCategory category)
    {
        if (!isGameActive || currentPatient == null) return;

        currentPatient.AssignTriageLabel(category);
        processedPatients.Add(currentPatient);

        NextPatient();
    }

    public Patient GetCurrentPatient()
    {
        return currentPatient;
    }

    public void EndScenario()
    {
        isGameActive = false;
        
        currentScore = evaluationSystem.CalculateFinalScore(processedPatients, timeRemaining);
        string report = evaluationSystem.GenerateFeedbackReport();

        uiController.ShowFinalResults(currentScore, report);
    }
}