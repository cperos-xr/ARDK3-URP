using Niantic.Lightship.AR.LocationAR;
using Niantic.Lightship.AR.PersistentAnchors;
using Niantic.Lightship.Maps.Coordinates;
using Niantic.Lightship.Maps.Core.Coordinates;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


[Serializable]
public struct ARLocationCoordinateContainer
{
    public ARLocation arLocation;
    public SerializableLatLng gpsCoordinates;
}
public class AnchorTrackingManager : MonoBehaviour
{
    [Tooltip("The location manager")]
    [SerializeField] private ARLocationManager _arLocationManager;

    [SerializeField] private ARLocation _closestLocation;
    [SerializeField] private List <ARLocationCoordinateContainer> _arLocations;

    public TextMeshProUGUI closestLocationText;


    private void OnEnable()
    {
        _arLocationManager.locationTrackingStateChanged += OnLocationTrackingStateChanged;
        LevelManager.OnPlayerPositionChanged += HandlePlayerPositionChange;
    }



    private void OnDisable()
    {
        _arLocationManager.locationTrackingStateChanged += OnLocationTrackingStateChanged;
        LevelManager.OnPlayerPositionChanged += HandlePlayerPositionChange;
    }

    private void HandlePlayerPositionChange(LatLng newPosition)
    {
        List<float> distances = new List<float>();
        foreach (ARLocationCoordinateContainer arLocation in _arLocations)
        {
            LatLng locationCoordinates = arLocation.gpsCoordinates;
            LatLng playerCoordinates = new LatLng(LocationService.Instance.CurrentGPS.x, LocationService.Instance.CurrentGPS.y);

            float distance = GeoUtility.CalculateDistance(locationCoordinates, playerCoordinates);
            distances.Add(distance);
        }

        // Now find the minimum distance
        float minDistance = distances.Min();

        foreach (ARLocationCoordinateContainer arLocation in _arLocations)
        {
            LatLng locationCoordinates = arLocation.gpsCoordinates;
            LatLng playerCoordinates = new LatLng(LocationService.Instance.CurrentGPS.x, LocationService.Instance.CurrentGPS.y);

            float distance = GeoUtility.CalculateDistance(locationCoordinates, playerCoordinates);
            if (distance == minDistance)
            {
                if (!arLocation.arLocation.Equals(_closestLocation))
                {
                    Debug.Log("Closest Ar Entity has changed");
                    _arLocationManager.StopTracking();
                    _closestLocation = arLocation.arLocation;
                    closestLocationText.text = _closestLocation.name;
                    _arLocationManager.SetARLocations(arLocation.arLocation);
                    _arLocationManager.StartTracking();
                    break; // Break after finding and setting the closest location
                }
                else
                {
                    //Debug.Log("Closest Ar Entity has not changed");
                }
            }
        }
    }


    private void OnLocationTrackingStateChanged(ARLocationTrackedEventArgs args)
    {
        if (!args.Tracking)
        {
            // We de-activate the gameObject when we lose tracking.
            // ARLocationManager will not de-activate it automatically
            args.ARLocation.gameObject.SetActive(false);
        }
    }


}
