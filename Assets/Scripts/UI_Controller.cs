using UnityEngine;
using TMPro;

public class UI_Controller : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameplayPanel;
    public GameObject resultsPanel;

    [Header("Gameplay UI Elements")]
    public TextMeshProUGUI patientIdText;
    public TextMeshProUGUI symptomsText;
    public TextMeshProUGUI vitalsDisplayText;
    public TextMeshProUGUI timerText;

    [Header("Results UI Elements")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI feedbackReportText;

    [Header("Tool References")]
    public SaturationMeter saturationMeter;
    public BloodPressureCuff bpCuff;

    private void Update()
    {
        // Only accept input if the gameplay panel is active
        if (!gameplayPanel.activeSelf) return;

        HandleToolInputs();
        HandleTriageInputs();
    }

    private void HandleToolInputs()
    {
        // Press Q to use Saturation Meter
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Patient current = GameManager.Instance.GetCurrentPatient();
            if (current != null && saturationMeter != null)
            {
                VitalSigns v = saturationMeter.UseTool(current);
                vitalsDisplayText.text = $"[Q] Pulse Oximeter: {v.oxygenSaturation}% SpO2";
            }
        }

        // Press E to use Blood Pressure Cuff
        if (Input.GetKeyDown(KeyCode.E))
        {
            Patient current = GameManager.Instance.GetCurrentPatient();
            if (current != null && bpCuff != null)
            {
                VitalSigns v = bpCuff.UseTool(current);
                vitalsDisplayText.text = $"[E] BP Cuff: {v.bloodPressure}";
            }
        }
    }

    private void HandleTriageInputs()
    {
        // Keyboard shortcuts for Triage Categories:
        // 1 = RED, 2 = ORANGE, 3 = YELLOW, 4 = GREEN
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.Instance.ProcessTriageChoice(TriageCategory.RED);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameManager.Instance.ProcessTriageChoice(TriageCategory.ORANGE);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameManager.Instance.ProcessTriageChoice(TriageCategory.YELLOW);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameManager.Instance.ProcessTriageChoice(TriageCategory.GREEN);
        }
    }

    public void DisplayPatientInfo(Patient p)
    {
        if (p == null) return;

        patientIdText.text = $"Patient ID: {p.patientId}";
        symptomsText.text = $"Symptoms: {p.GetSymptoms()}";
        vitalsDisplayText.text = "Vitals: [Q] Measure SpO2 | [E] Measure BP";
    }

    public void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ShowFinalResults(int score, string feedback)
    {
        gameplayPanel.SetActive(false);
        resultsPanel.SetActive(true);

        finalScoreText.text = $"Final Score: {score}";
        feedbackReportText.text = feedback;
    }
}