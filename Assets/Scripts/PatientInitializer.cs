using UnityEngine;

[RequireComponent(typeof(Patient3D))]
public class PatientInitializer : MonoBehaviour
{
    [Tooltip("Kies patiënttype 1 t/m 5")]
    [Range(1, 5)] public int patientPresetNumber = 1;

    private void Awake()
    {
        Patient3D patient = GetComponent<Patient3D>();

        switch (patientPresetNumber)
        {
            case 1:
                patient.patientId = "P001";
                patient.symptomsDescription = "Aanval van hevige druk op de borst met uitstraling naar de linkerarm. Bezweet en angstig.";
                patient.isCriticallyIll = true;
                patient.vitals = new VitalSigns(118, "165/95", 89, 36.8f);
                break;

            case 2:
                patient.patientId = "P002";
                patient.symptomsDescription = "Ernstige benauwdheid en piepende ademhaling. Kan niet in volzinnen spreken.";
                patient.isCriticallyIll = true;
                patient.vitals = new VitalSigns(125, "130/85", 84, 37.1f);
                break;

            case 3:
                patient.patientId = "P003";
                patient.symptomsDescription = "Sinds vanmorgen hevige, snijdende pijn in de rechter onderbuik. Misselijk en overgegeven.";
                patient.isCriticallyIll = false;
                patient.vitals = new VitalSigns(88, "125/80", 98, 38.3f);
                break;

            case 4:
                patient.patientId = "P004";
                patient.symptomsDescription = "Gevallen op de rechterpols. Pols is gezwollen en gevoelig, maar geen open wond.";
                patient.isCriticallyIll = false;
                patient.vitals = new VitalSigns(72, "120/78", 99, 36.6f);
                break;

            case 5:
                patient.patientId = "P005";
                patient.symptomsDescription = "Sinds gisteren toenemende rillingen en duizeligheid bij het opstaan. Bleke huidgelaat.";
                patient.isCriticallyIll = true;
                patient.vitals = new VitalSigns(135, "85/50", 93, 39.4f);
                break;
        }
    }
}
