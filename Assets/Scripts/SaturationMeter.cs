using UnityEngine;

public class SaturationMeter : TriageTool
{
    private void Awake()
    {
        toolName = "Pulse Oximeter";
    }

    public override VitalSigns UseTool(Patient p)
    {
        measureSpO2(p);
        return p.vitals;
    }

    public int measureSpO2(Patient p)
    {
        return p.vitals.oxygenSaturation;
    }
}