using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Quest", menuName = "Game/Quest System/Quest")]
[Serializable]
public class SO_Quest : ScriptableObject
{
    [Header("Quest Information")]
    public string questID;
    public string questTitle;
    [TextArea(3, 5)] public string questDescription;

    [Header("Tasks")]
    public List <SO_Task> tasks = new List<SO_Task>();

    [Header("Reward")]
    public int experienceReward;
    public SO_ItemData rewardItem;

    // Additional fields and methods related to quests can be added here.
}
