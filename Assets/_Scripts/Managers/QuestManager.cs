using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    static public QuestManager Instance;
    public List<SO_Quest> AssignedQuests = new List<SO_Quest>();

    public delegate void PlayerAssignedQuestEvent(SO_Quest quest);
    public static event PlayerAssignedQuestEvent OnPlayerAssignedQuest;

    private void Awake()
    {
        // Ensure there is only one instance of QuestManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    public void AssignQuest(SO_Quest quest)
    {
        AssignedQuests.Add(quest);
        OnPlayerAssignedQuest?.Invoke(quest);

        foreach(SO_Task task in quest.tasks)
        {
            TaskManager.Instance.AssignTask(task);
        }

    }
}
