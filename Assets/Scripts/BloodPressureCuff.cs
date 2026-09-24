using UnityEngine;

public class BloodPressureCuff : TriageTool
{
    private void Awake()
    {
        toolName = "Blood Pressure Cuff";
    }

    public override VitalSigns UseTool(Patient p)
    {
        measureBP(p);
        return p.vitals;
    }

    public string measureBP(Patient p)
    {
        return p.vitals.bloodPressure;
    }
}