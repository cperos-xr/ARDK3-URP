using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChannelListContainer", menuName = "Game/AR/Semantic Channel List", order = 4)]
public class ChannelListContainer : ScriptableObject
{
    public List<string> channelNames;
}
