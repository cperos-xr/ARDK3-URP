using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public List<SO_Task> ActiveTasks = new List<SO_Task>();
    public List<SO_Task> completedTasks = new List<SO_Task>();

    public List<SO_TaskObjective> CompletedTaskObjectives = new List<SO_TaskObjective>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AssignTask(SO_Task task)
    {
        if (task == null)
        {
            Debug.LogError("Task is null.");
            return;
        }

        ActiveTasks.Add(task);
        foreach (var objective in task.objectives)
        {
            if (objective == null)
            {
                Debug.LogError("Objective is null.");
                continue;
            }

            TaskObjectiveHandler.Instance.AssignObjective(task, objective);
        }
    }

    public bool IsTaskCompleted(SO_Task task)
    {
        if (task == null)
        {
            Debug.LogError("Task is null.");
            return false;
        }

        bool allObjectivesCompleted = true; // Assume all objectives are completed initially.

        foreach (var objective in task.objectives)
        {
            if (!TaskObjectiveHandler.Instance.IsObjectiveCompleted(objective))
            {
                allObjectivesCompleted = false; // If any objective is not completed, set the flag to false.
                Debug.Log($"Objective '{objective.objectiveName}' in task '{task.taskName}' is not completed.");
            }
            else
            {
                Debug.Log($"Objective '{objective.objectiveName}' in task '{task.taskName}' is completed.");
                if (!CompletedTaskObjectives.Contains(objective))
                {
                    CompletedTaskObjectives.Add(objective);
                    InteractionManager.Instance.UpdateAllEntityInteractions(objective);
                }

            }
        }

        if (allObjectivesCompleted && !completedTasks.Contains(task))
        {
            completedTasks.Add(task);
            InteractionManager.Instance.UpdateAllEntityInteractions(task);
        }

        return allObjectivesCompleted; // Return the flag indicating whether all objectives are completed.
    }


    public void CheckAllTasksForCompletion()
    {
        List<SO_Task> tasksToRemove = new List<SO_Task>();

        foreach (var task in ActiveTasks)
        {
            if (IsTaskCompleted(task))
            {
                tasksToRemove.Add(task);
            }
        }

        foreach (var taskToRemove in tasksToRemove)
        {
            CompleteTask(taskToRemove);
        }
    }

    public void CompleteTask(SO_Task task)
    {
        Debug.Log($"Task {task.taskName} is complete!");
        completedTasks.Add(task);
        ActiveTasks.Remove(task);

        InteractionManager.Instance.UpdateAllEntityInteractions(task);

    }
}
