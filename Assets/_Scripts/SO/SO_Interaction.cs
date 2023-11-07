using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInteraction", menuName = "Game/Level/Entity/Interaction", order = 2)]
[Serializable]
public class SO_Interaction : InteractionProgression
{
    public string InteractionName;
    public bool returnToPreviousInteractionAfter;
    public List<SO_ItemData> itemDatas;
    public SO_Quest quest; // List of tasks associated with this interaction.
    public PlayerNotification notification;
}
