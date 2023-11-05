#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelManager levelManager = (LevelManager)target;

        if (GUILayout.Button("Place All Markers"))
        {
            levelManager.PlaceAllMarkers();
        }
    }
}
#endif
