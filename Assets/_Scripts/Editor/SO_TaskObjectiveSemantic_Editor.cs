using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(SO_TaskObjectiveSemantic))]
public class SO_TaskObjectiveSemanticEditor : Editor
{
    private const string CurrentFileRelativePath = "SemanticData/SemanticChannels.csv";
    private const string BackupFileRelativePath = "SemanticData/SemanticChannels-BackUp.csv";

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        SO_TaskObjectiveSemantic script = (SO_TaskObjectiveSemantic)target;

        // Attempt to load the channel names from the current CSV data or fall back to the backup
        string[] options = LoadCsvData(CurrentFileRelativePath, BackupFileRelativePath);

        // Find the index of the currently selected channel, or default to 0 if not found
        int selectedIndex = Mathf.Max(0, System.Array.IndexOf(options, script.selectedChannel));

        // Create the dropdown menu for the channels
        selectedIndex = EditorGUILayout.Popup("Selected Channel", selectedIndex, options);

        // Save the selected value
        script.selectedChannel = options[selectedIndex];

        // If the inspector is changed, mark the object as dirty to enable saving
        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
    }

    private string[] LoadCsvData(string currentRelativePath, string backupRelativePath)
    {
        string csvData = TryLoadCsv(currentRelativePath);
        if (string.IsNullOrWhiteSpace(csvData))
        {
            csvData = TryLoadCsv(backupRelativePath);
        }

        if (!string.IsNullOrWhiteSpace(csvData))
        {
            // Split the CSV data into an array of strings
            return csvData.Split(new char[] { ',', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        }

        // Fallback to an empty array if both CSV loads fail
        return new string[0];
    }

    private string TryLoadCsv(string relativePath)
    {
        string fullPath = Path.Combine(Application.dataPath, relativePath);
        if (File.Exists(fullPath))
        {
            return File.ReadAllText(fullPath);
        }
        return string.Empty;
    }
}
