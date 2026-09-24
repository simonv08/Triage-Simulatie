using UnityEngine;

public abstract class TriageTool : MonoBehaviour
{
    public string toolName;
    public bool isAvailable = true;

    public abstract VitalSigns UseTool(Patient p);
}