using UnityEngine;

public class BloodPressureCuff : TriageTool
{
    private void Awake()
    {
        toolName = "Blood Pressure Cuff";
    }

    public override VitalSigns UseTool(Patient3D patient)
    {
        measureBP(patient);
        return patient.vitals;
    }

    public string measureBP(Patient3D patient)
    {
        return patient.vitals.bloodPressure;
    }
}