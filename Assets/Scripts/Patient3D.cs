using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Patient3D : MonoBehaviour
{
    [Header("Patient Data")]
    public string patientId = "P001";
    [TextArea] public string symptomsDescription = "Kortademig en druk op de borst";
    public bool isCriticallyIll = true;
    public VitalSigns vitals = new VitalSigns(110, "140/90", 88, 37.2f);

    [Header("Triage Status")]
    public TriageCategory? assignedCategory = null;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void WalkTo(Vector3 destination)
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(destination);
        }
    }

    public void AssignTriageLabel(TriageCategory category)
    {
        assignedCategory = category;
        Debug.Log($"Patiënt {patientId} getrieerd als: {category}");
    }
}