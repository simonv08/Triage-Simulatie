using UnityEngine;

[System.Serializable]
public class VitalSigns
{
    public int heartRate;
    public string bloodPressure;
    public int oxygenSaturation;
    public float bodyTemperature;

    public VitalSigns(int hr, string bp, int spO2, float temp)
    {
        heartRate = hr;
        bloodPressure = bp;
        oxygenSaturation = spO2;
        bodyTemperature = temp;
    }

    public string ReadVitals()
    {
        return $"HR: {heartRate} bpm | BP: {bloodPressure} | SpO2: {oxygenSaturation}% | Temp: {bodyTemperature:F1}°C";
    }
}