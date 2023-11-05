using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SO_Task", menuName = "Game/Quest System/Task")]
public class SO_Task : ScriptableObject, IButtonObject
{
    [Header("Task Information")]
    public string taskID;
    public string taskName;
    public Sprite icon;
    [TextArea(3, 5)] public string taskDescription;

    [Header("Objectives")]
    public List<SO_TaskObjective> objectives;

    [Header("Reward")]
    public int experienceReward;
    public List<SO_ItemData> itemRewards;

    public Sprite Icon => icon;
    public string ObjectName => taskName;

    // Additional fields and methods related to tasks can be added here.
}
