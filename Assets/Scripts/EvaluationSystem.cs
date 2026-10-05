using UnityEngine;
using System.Collections.Generic;

public class EvaluationSystem : MonoBehaviour
{
    private float accuracyPercentage;
    private float timeBonus;

    public int CalculateFinalScore(List<Patient> patients, float timeRemaining)
    {
        if (patients == null || patients.Count == 0) return 0;

        int correctAssignments = 0;

        foreach (var p in patients)
        {
            // Simple evaluation logic: Critical patients should be RED or ORANGE
            if (p.isCriticallyIll && (p.assignedCategory == TriageCategory.RED || p.assignedCategory == TriageCategory.ORANGE))
            {
                correctAssignments++;
            }
            else if (!p.isCriticallyIll && (p.assignedCategory == TriageCategory.YELLOW || p.assignedCategory == TriageCategory.GREEN))
            {
                correctAssignments++;
            }
        }

        accuracyPercentage = ((float)correctAssignments / patients.Count) * 100f;
        timeBonus = Mathf.Max(0, timeRemaining * 10f);

        int baseScore = Mathf.RoundToInt(accuracyPercentage * 10f);
        return baseScore + Mathf.RoundToInt(timeBonus);
    }

    public string GenerateFeedbackReport()
    {
        return $"Accuracy: {accuracyPercentage:F1}%\nTime Bonus: +{Mathf.RoundToInt(timeBonus)} pts";
    }
}