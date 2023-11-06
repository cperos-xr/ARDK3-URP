using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SemanticChannelDetector))]
public class SemanticChannelDetectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SemanticChannelDetector detectorScript = (SemanticChannelDetector)target;

        if (GUILayout.Button("Test Semantic Broadcast"))
        {
            detectorScript.TestSemanticBroadcast();
        }
    }
}
