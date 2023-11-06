#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using Niantic.Lightship.AR.Semantics;
using UnityEditor;
using UnityEngine;
using System.IO;

public class SemanticChannelListLoader : MonoBehaviour
{


    [SerializeField]
    private ARSemanticSegmentationManager _semanticsManager;

    public ChannelListContainer channelListContainer;

    public bool writeToCSV;
    public bool writeToJSON;

    private void Start()
    {
        if (_semanticsManager != null)
        {
            _semanticsManager.MetadataInitialized += SemanticsManager_OnDataInitialized;
        }
    }

    private void OnDestroy()
    {
        if (_semanticsManager != null)
        {
            _semanticsManager.MetadataInitialized -= SemanticsManager_OnDataInitialized;
        }
    }

    private string GetSavePath(string fileName)
    {
        // Path to the Assets folder
        string assetsPath = Application.dataPath;
        // Path to the EditorData folder within the Assets directory
        string editorDataPath = Path.Combine(assetsPath, "EditorData");

        // Check if the EditorData directory exists, if not, create it
        if (!Directory.Exists(editorDataPath))
        {
            Directory.CreateDirectory(editorDataPath);
        }

        // Return the full path to the file within the EditorData directory
        return Path.Combine(editorDataPath, fileName);
    }

    private void SemanticsManager_OnDataInitialized(ARSemanticSegmentationModelEventArgs args)
    {
        var channelNames = _semanticsManager.ChannelNames;
        foreach (var channelName in channelNames)
        {
            if (!channelListContainer.channelNames.Contains(channelName))
            {
                channelListContainer.channelNames.Add(channelName);
            }
        }

        if (writeToCSV)
        {
            string filePath = GetSavePath("SemanticChannels.json");
            WriteToCSV(channelListContainer.channelNames, filePath);
        }

        if (writeToJSON)
        {
            string filePath = GetSavePath("SemanticChannels.csv");
            WriteToJSON(channelListContainer.channelNames, filePath);
        }
    }

    private void WriteToCSV(List<string> channelNames, string filePath)
    {
        StringBuilder sb = new StringBuilder();

        foreach (string name in channelNames)
        {
            sb.AppendLine(name);
        }

        File.WriteAllText(filePath, sb.ToString());
        AssetDatabase.Refresh(); // Refresh the AssetDatabase to show the new file in Unity Editor
    }

    [System.Serializable]
    public class ChannelNamesWrapper
    {
        public List<string> channelNames;
    }

    private void WriteToJSON(List<string> channelNames, string filePath)
    {
        ChannelNamesWrapper wrapper = new ChannelNamesWrapper { channelNames = channelNames };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(filePath, json);
        AssetDatabase.Refresh(); // Refresh the AssetDatabase to show the new file in Unity Editor
    }



#endif
}

