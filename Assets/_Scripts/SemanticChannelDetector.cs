using Niantic.Lightship.AR.Semantics;
using System.Collections.Generic;
using UnityEngine;

public class SemanticChannelDetector : MonoBehaviour
{
    [SerializeField]
    private ARSemanticSegmentationManager _semanticsManager;

    private void Update()
    {
        // Check for a screen tap
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
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

        // Process the channels as needed
        foreach (var channel in channelsAtPoint)
        {
            // Check if the channel matches any task objectives, etc.
            Debug.Log($"Detected semantic channel at tap: {channel}");
        }
    }
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