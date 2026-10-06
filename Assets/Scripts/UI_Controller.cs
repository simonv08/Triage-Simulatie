using UnityEngine;
using TMPro;

public class UI_Controller : MonoBehaviour
{
    public static UI_Controller Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject gameplayHUD;
    public GameObject interactionPromptPanel;
    public GameObject resultsPanel;

    [Header("HUD Text Elements")]
    public TextMeshProUGUI patientInfoText;
    public TextMeshProUGUI vitalsResultText;
    public TextMeshProUGUI timerText;

    [Header("Results Elements")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI feedbackReportText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowInteractionPrompt(Patient3D patient)
    {
        if (interactionPromptPanel != null) interactionPromptPanel.SetActive(true);

        if (patientInfoText != null)
        {
            patientInfoText.text = $"<b>Patient ID: {patient.patientId}</b>\n" +
                                   $"Symptomen: {patient.symptomsDescription}\n\n" +
                                   $"<i>[Q] SpO2  |  [E] Bloeddruk  |  [1] Rood  [2] Oranje  [3] Geel  [4] Groen</i>";
        }
    }

    public void HideInteractionPrompt()
    {
        if (interactionPromptPanel != null) interactionPromptPanel.SetActive(false);
        if (vitalsResultText != null) vitalsResultText.text = "";
    }

    public void DisplayVitalResult(string result)
    {
        if (vitalsResultText != null) vitalsResultText.text = result;
    }

    public void UpdateTimerDisplay(float time)
    {
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ShowFinalResults(int score, string feedback)
    {
        if (gameplayHUD != null) gameplayHUD.SetActive(false);
        if (resultsPanel != null) resultsPanel.SetActive(true);

        if (finalScoreText != null) finalScoreText.text = $"Eindscore: {score}";
        if (feedbackReportText != null) feedbackReportText.text = feedback;
    }
}