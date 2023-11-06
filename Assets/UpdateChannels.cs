using UnityEngine;
using System.Collections.Generic;
using Niantic.Lightship.AR.Semantics;

public class UpdateChannels : MonoBehaviour
{
    //public ChannelListContainer channelListContainer;
    public ARSemanticSegmentationManager _semanticMan;

    public List<string> _channels = new List<string>();

    public bool getCurrentChannelList;


    private void Update()
    {
        if (getCurrentChannelList)
        {
            getCurrentChannelList = false;
            GetChannels();
            // channelListContainer.channelNames = _channels;
        }
    }

    private void GetChannels()
    {
        foreach (var cName in _semanticMan.ChannelNames)
        {
            string channelName = cName;
            _channels.Add(channelName);
        }
    }
}