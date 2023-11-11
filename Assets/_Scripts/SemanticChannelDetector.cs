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

    public delegate void IdentifySemanticChannelEvent(List<string> semanticChannels, Vector2 point);
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
            Debug.Log($"No semantic channels at tap {x},{y}");
            _semanticsText.text = "No semantic channels";
        }
        else
        {
            // Process the channels as needed
            foreach (var channel in channelsAtPoint)
            {
                // Check if the channel matches any task objectives, etc.
                Debug.Log($"Detected semantic channel at tap {x},{y}: {channel}");
                _semanticsText.text = _semanticsText.text + channel + " ";
            }

            OnSemanticChannelIdentified(channelsAtPoint, point);
        }

    }

#if UNITY_EDITOR

    public void TestSemanticBroadcast()
    {
        string sky = "sky";
        string ground = "ground";
        string natural_ground = "natural_ground";
        string artificial_ground = "artificial_ground";
        string water = "water";
        string person = "person";
        string building = "building";
        string foliage = "foliage";
        string grass = "grass";
        string flower_experimental = "flower_experimental";
        string tree_trunk_experimental = "tree_trunk_experimental";
        string pet_experimental = "pet_experimental";
        string sand_experimental = "sand_experimental";
        string tv_experimental = "tv_experimental";
        string dirt_experimental = "dirt_experimental";
        string vehicle_experimental = "vehicle_experimental";
        string food_experimental = "food_experimental";
        string loungeable_experimental = "loungeable_experimental";
        string snow_experimental = "snow_experimental";

        List<string> semanticChannelList = new List<string>();

        semanticChannelList.Add(sky);
        semanticChannelList.Add(ground);
        semanticChannelList.Add(natural_ground);
        semanticChannelList.Add(artificial_ground);
        semanticChannelList.Add(water);
        semanticChannelList.Add(person);
        semanticChannelList.Add(building);
        semanticChannelList.Add(foliage);
        semanticChannelList.Add(grass);
        semanticChannelList.Add(flower_experimental);
        semanticChannelList.Add(tree_trunk_experimental);
        semanticChannelList.Add(pet_experimental);
        semanticChannelList.Add(sand_experimental);
        semanticChannelList.Add(tv_experimental);
        semanticChannelList.Add(dirt_experimental);
        semanticChannelList.Add(vehicle_experimental);
        semanticChannelList.Add(food_experimental);
        semanticChannelList.Add(loungeable_experimental);
        semanticChannelList.Add(snow_experimental);

        OnSemanticChannelIdentified(semanticChannelList, new Vector2(600f, 600f));
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