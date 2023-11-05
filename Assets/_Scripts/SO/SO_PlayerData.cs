using Niantic.Lightship.Maps.MapLayers.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level/PlayerData")]
public class SO_PlayerData : ScriptableObject
{
    public int initialHealth;
    public int initialMagic;
    public LayerGameObjectPlacement _cubeGOP;

    public List<SO_AreaData> visitedAreas = new List<SO_AreaData>();


}
