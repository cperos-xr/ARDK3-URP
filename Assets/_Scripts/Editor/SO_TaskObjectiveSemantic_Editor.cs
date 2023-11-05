using static UnityEngine.GraphicsBuffer;
using UnityEditor;

[CustomEditor(typeof(SO_TaskObjectiveSemantic))]
public class SO_TaskObjectiveSemanticEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ChannelListContainer channelListContainer = AssetDatabase.LoadAssetAtPath<ChannelListContainer>("path/to/ChannelListContainer.asset");
        SO_TaskObjectiveSemantic taskObj = (SO_TaskObjectiveSemantic)target;

        int currentSelectedIndex = channelListContainer.channelNames.IndexOf(taskObj.selectedChannel);

        if (currentSelectedIndex < 0) currentSelectedIndex = 0;

        int newSelectedIndex = EditorGUILayout.Popup("Channel", currentSelectedIndex, channelListContainer.channelNames.ToArray());

        if (newSelectedIndex >= 0 && newSelectedIndex < channelListContainer.channelNames.Count)
        {
            taskObj.selectedChannel = channelListContainer.channelNames[newSelectedIndex];
        }
    }
}
