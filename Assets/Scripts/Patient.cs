using UnityEngine;

[System.Serializable]
public class Patient
{
    public string patientId;
    public string symptomsDescription;
    public bool isCriticallyIll;
    public VitalSigns vitals;
    public TriageCategory? assignedCategory;

    public Patient(string id, string symptoms, bool criticallyIll, VitalSigns initialVitals)
    {
        patientId = id;
        symptomsDescription = symptoms;
        isCriticallyIll = criticallyIll;
        vitals = initialVitals;
        assignedCategory = null;
    }

    public string GetSymptoms()
    {
        return symptomsDescription;
    }

    public void AssignTriageLabel(TriageCategory label)
    {
        assignedCategory = label;
    }
}