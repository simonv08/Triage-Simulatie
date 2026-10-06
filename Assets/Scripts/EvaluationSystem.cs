using System.Collections.Generic;
using UnityEngine;

public class EvaluationSystem : MonoBehaviour
{
    private float accuracyPercentage;
    private float timeBonus;

    public int CalculateFinalScore(List<Patient3D> patients, float timeRemaining)
    {
        if (patients == null || patients.Count == 0) return 0;

        int correctAssignments = 0;

        foreach (var p in patients)
        {
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
        return $"Nauwkeurigheid: {accuracyPercentage:F1}%\nTijdbonus: +{Mathf.RoundToInt(timeBonus)} pt";
    }
}