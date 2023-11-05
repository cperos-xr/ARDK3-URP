using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TaskManager))]
public class TaskManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TaskManager taskManager = (TaskManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Check All Task Completion"))
        {
            taskManager.CheckAllTasksForCompletion();
        }
    }
}
