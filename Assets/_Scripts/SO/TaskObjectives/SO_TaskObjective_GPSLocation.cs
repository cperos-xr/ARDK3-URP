using Niantic.Lightship.Maps.Coordinates;
using Niantic.Lightship.Maps.Core.Coordinates;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TaskObjective_GPSLocation", menuName = "Game/Quest System/Task Objective/GPSLocation")]
public class SO_TaskObjective_GPSLocation : SO_TaskObjective
{
    [SerializeField]
    private SerializableLatLng targetLocation;
    public float radius;

    [HideInInspector]
    public LatLng targetLatLng;


    private void OnEnable()
    {
        targetLatLng = new LatLng(targetLocation.Latitude, targetLocation.Longitude);
    }

    // Additional properties and methods specific to GPS location objectives go here.
}