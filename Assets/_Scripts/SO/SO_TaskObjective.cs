using System;
using UnityEngine;


[Serializable]
public class SO_TaskObjective : ScriptableObject
{
    public string objectiveName;
    public string objectiveDescription;
    //public ObjectiveType type;
    public int requiredProgress;

    // Additional properties specific to different objective types (e.g., GPS location).
}

// Add more specific task objective classes as needed.


