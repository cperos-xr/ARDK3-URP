using Niantic.Lightship.AR.Semantics;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


[Serializable]
public enum ESemanticChannel
{
    sky,
    ground,
    natural_ground,
    artificial_ground,
    water,
    person,
    building,
    foliage,
    grass,
    flower_experimental,
    tree_trunk_experimental,
    pet_experimental,
    sand_experimental,
    tv_experimental,
    dirt_experimental,
    vehicle_experimental,
    food_experimental,
    loungeable_experimental,
    snow_experimental,
    harmony
}


public class SemanticChannelDetector : MonoBehaviour
{
    [SerializeField]
    private ARSemanticSegmentationManager _semanticsManager;

    [SerializeField]
    private TextMeshProUGUI _semanticsText;

    public delegate void IdentifySemanticChannelEvent(List<string> semanticChannels, Vector2 point);
    public static event IdentifySemanticChannelEvent OnSemanticChannelIdentified;

    [SerializeField] private ManaLens manaLens;

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

// For in editor testing only, will send a random channel
#if UNITY_EDITOR

        ESemanticChannel randomChannel = (ESemanticChannel)UnityEngine.Random.Range(0, Enum.GetNames(typeof(ESemanticChannel)).Length);
        channelsAtPoint.Add(randomChannel.ToString());
        _semanticsText.text = randomChannel.ToString();

#endif
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

    [Serializable]
    struct ChannelIntergerConatainer
    {
        [SerializeField]
        public ESemanticChannel selectedChannel;

        [SerializeField]
        public int channelCount;

    }


    [SerializeField] private List <ChannelIntergerConatainer> testChannels = new List<ChannelIntergerConatainer>();



    public void TestSemanticBroadcast()
    {
        List<ESemanticChannel> semanticChannelList = new List<ESemanticChannel>();

        // Add each enum value to the list

        foreach (ChannelIntergerConatainer selectedChannel in testChannels)
        {
            for (int i = 0; i <= selectedChannel.channelCount; i++)
            {
                semanticChannelList.Add(selectedChannel.selectedChannel);
            }

        }

        // Convert the enum list to a string list for the method call
        List<string> semanticChannelStringList = semanticChannelList.Select(c => c.ToString()).ToList();

        OnSemanticChannelIdentified(semanticChannelStringList, new Vector2(600f, 600f));
    }

#endif

    public void GivePono()
    {
        ManaLens.ELensState originalLensState = manaLens.lensState;
        manaLens.lensState = ManaLens.ELensState.Extracting;
        ESemanticChannel pono = ESemanticChannel.harmony;
        List<string> semanticChannelStringList = new List<string>();
        semanticChannelStringList.Add(pono.ToString());
        OnSemanticChannelIdentified(semanticChannelStringList, new Vector2(600f, 600f));
        manaLens.lensState = originalLensState;
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