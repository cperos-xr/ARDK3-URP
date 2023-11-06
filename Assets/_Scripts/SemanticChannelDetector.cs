using Niantic.Lightship.AR.Semantics;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SemanticChannelDetector : MonoBehaviour
{
    [SerializeField]
    private ARSemanticSegmentationManager _semanticsManager;

    [SerializeField]
    private TextMeshProUGUI _semanticsText;

    public delegate void IdentifySemanticChannelEvent(List<string> semanticChannels);
    public static event IdentifySemanticChannelEvent OnSemanticChannelIdentified;

    private void Update()
    {
        // Check for a screen tap
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log($"tap detected");
            Vector2 touchPosition = Input.GetTouch(0).position;
            IdentifySemanticChannelAtPoint(touchPosition);
        }
    }

    private void IdentifySemanticChannelAtPoint(Vector2 point)
    {
        // Convert to integer coordinates
        int x = Mathf.RoundToInt(point.x);
        int y = Mathf.RoundToInt(point.y);
        // Get the semantic channels at the tap position
        List<string> channelsAtPoint = _semanticsManager.GetChannelNamesAt(x, y);
        _semanticsText.text = "";
        if (channelsAtPoint.Count == 0)
        {
            Debug.Log($"No semantic channels at tap");
            _semanticsText.text = "No semantic channels";
        }
        // Process the channels as needed
        foreach (var channel in channelsAtPoint)
        {
            // Check if the channel matches any task objectives, etc.
            Debug.Log($"Detected semantic channel at tap: {channel}");
            _semanticsText.text = _semanticsText.text + channel + " ";
        }

        OnSemanticChannelIdentified(channelsAtPoint);
    }

#if UNITY_EDITOR

    public void TestSemanticBroadcast()
    {
        string loungeable = "loungeable_experimental";
        string grass = "grass";

        List<string> list = new List<string>();

        list.Add(loungeable);
        list.Add(grass);

        OnSemanticChannelIdentified(list);
    }

#endif

    public bool DoesChannelExistAtPoint(Vector2 point, string channelName)
    {
        // Convert to integer coordinates
        int x = Mathf.RoundToInt(point.x);
        int y = Mathf.RoundToInt(point.y);

        // Get the semantic channels at the tap position
        List<string> channelsAtPoint = _semanticsManager.GetChannelNamesAt(x, y);

        // Check if the specified channel exists at the point
        bool channelExists = channelsAtPoint.Contains(channelName);

        // Log the result for debugging purposes
        Debug.Log($"Channel '{channelName}' exists at tap: {channelExists}");

        // Return the result
        return channelExists;
    }

}