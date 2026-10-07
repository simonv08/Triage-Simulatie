using System;
using System.Collections.Generic;

[Serializable]
public class ProtocolStepLog
{
    public string stepName;      // Bijv. "Measure_SpO2", "Assign_Triage_RED"
    public string patientId;     // Bijv. "P001"
    public bool isCorrect;       // True/False
    public float timestamp;      // Tijdstip in seconden sinds start
    public string details;       // Bijv. "Gekozen: GREEN, Verwacht: RED"
}

[Serializable]
public class TriageSessionData
{
    public string studentId;           // ID van de ingelogde cursist
    public string protocolType;        // "TRIAGE" of "MEDICATIE"
    public float completionTime;       // Totale tijdsduur
    public int finalScore;             // Eindscore
    public bool isPassed;              // Voldoende/Onvoldoende
    public List<ProtocolStepLog> steps = new List<ProtocolStepLog>();
}