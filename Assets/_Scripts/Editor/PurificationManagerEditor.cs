using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PurificationManager))]
public class PurificationManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        PurificationManager manager = (PurificationManager)target;

        // Test Purification Button
        if (GUILayout.Button("Test Purification"))
        {
            if (manager.testCorruptEntity != null)
            {
                manager.InitializeAndBeginPurification(manager.testCorruptEntity);
            }
            else
            {
                Debug.LogWarning("No Corrupt Entity selected for testing.");
            }
        }
    }
}
