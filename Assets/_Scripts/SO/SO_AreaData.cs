using Niantic.Lightship.Maps.Core.Coordinates;
using Niantic.Lightship.Maps.Samples.CustomMapLayers.StateBoundaries;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level/Area")]
[Serializable]
public class SO_AreaData : ScriptableObject
{
    public string areaName;
    public string areaColor; //Colors must be attached and set in PolygonColorSelection object in scene

    [SerializeField]
    public List<Boundary> _boundaries = new();// For 3D objects representation
    public List<SO_MapEntityData> areaEntities; // Other SOs representing NPCs, items, etc.

    public bool hasBeenVisited;
    // Add other properties as needed

    // This is called when the ScriptableObject is initialized
}
