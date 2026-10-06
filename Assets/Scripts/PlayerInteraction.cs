using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactDistance = 3.0f;
    public LayerMask patientLayer;
    public Camera playerCamera;

    [Header("Tools Equipment")]
    public SaturationMeter saturationMeter;
    public BloodPressureCuff bpCuff;

    private Patient3D currentTargetPatient;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        DetectPatient();
        HandleInput();
    }

    private void DetectPatient()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, patientLayer))
        {
            Patient3D patient = hit.collider.GetComponent<Patient3D>();
            if (patient != null)
            {
                currentTargetPatient = patient;
                UI_Controller.Instance.ShowInteractionPrompt(patient);
                return;
            }
        }

        // Geen patiënt in het vizier
        currentTargetPatient = null;
        UI_Controller.Instance.HideInteractionPrompt();
    }

    private void HandleInput()
    {
        if (currentTargetPatient == null) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Metingen uitvoeren met gereedschap (Q & E)
        if (keyboard.qKey.wasPressedThisFrame && saturationMeter != null)
        {
            VitalSigns v = saturationMeter.UseTool(currentTargetPatient);
            UI_Controller.Instance.DisplayVitalResult($"SpO2 Meting: {v.oxygenSaturation}%");
        }

        if (keyboard.eKey.wasPressedThisFrame && bpCuff != null)
        {
            VitalSigns v = bpCuff.UseTool(currentTargetPatient);
            UI_Controller.Instance.DisplayVitalResult($"Bloeddruk Meting: {v.bloodPressure}");
        }

        // Triage Toewijzen (1 = RED, 2 = ORANGE, 3 = YELLOW, 4 = GREEN)
        if (keyboard.digit1Key.wasPressedThisFrame) AssignTriage(TriageCategory.RED);
        else if (keyboard.digit2Key.wasPressedThisFrame) AssignTriage(TriageCategory.ORANGE);
        else if (keyboard.digit3Key.wasPressedThisFrame) AssignTriage(TriageCategory.YELLOW);
        else if (keyboard.digit4Key.wasPressedThisFrame) AssignTriage(TriageCategory.GREEN);
    }

    private void AssignTriage(TriageCategory category)
    {
        currentTargetPatient.AssignTriageLabel(category);
        GameManager.Instance.RegisterTriagedPatient(currentTargetPatient);

        UI_Controller.Instance.HideInteractionPrompt();
        currentTargetPatient = null;
    }
}