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
        string assetsPath = Application.dataPath;
        string editorDataPath = Path.Combine(assetsPath, "SemanticData");
        if (!Directory.Exists(editorDataPath))
        {
            Directory.CreateDirectory(editorDataPath);
        }
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
            string csvFilePath = GetSavePath("SemanticChannels.csv");
            WriteToCSV(channelListContainer.channelNames, csvFilePath);
        }

        if (writeToJSON)
        {
            string jsonFilePath = GetSavePath("SemanticChannels.json");
            WriteToJSON(channelListContainer.channelNames, jsonFilePath);
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
        AssetDatabase.Refresh();
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
        AssetDatabase.Refresh();
    }
}
#endif
