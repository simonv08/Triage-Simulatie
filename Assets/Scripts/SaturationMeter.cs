using UnityEngine;

public class SaturationMeter : TriageTool
{
    private void Awake()
    {
        toolName = "Pulse Oximeter";
    }

    public override VitalSigns UseTool(Patient3D patient)
    {
        measureSpO2(patient);
        return patient.vitals;
    }

    public int measureSpO2(Patient3D patient)
    {
        return patient.vitals.oxygenSaturation;
    }
}