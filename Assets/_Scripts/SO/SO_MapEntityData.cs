using Niantic.Lightship.Maps.MapLayers.Components;
using Niantic.Lightship.Maps.Samples.CustomMapLayers.StateBoundaries;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level/Entity/MapEntity")]
public class SO_MapEntityData : BaseEntityData
{
    //public GameObject prefab;
    //public DV.dv2 gpsCoordinates;
    public Niantic.Lightship.Maps.Coordinates.SerializableLatLng gpsCoordinates;
    public float interactionRadius;


    public List<SO_ItemData> associatedItems = new List<SO_ItemData>();

    public LayerGameObjectPlacement _cubeGOP;

}

