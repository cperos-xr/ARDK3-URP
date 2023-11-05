using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChannelListContainer", menuName = "AR/Semantic Channel List")]
public class ChannelListContainer : ScriptableObject
{
    public List<string> channelNames;
}
