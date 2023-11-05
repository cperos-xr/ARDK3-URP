using Niantic.Lightship.Maps.Core.Coordinates;
using System;
using UnityEngine;

public class SerializeLatLng : MonoBehaviour
{
    [Serializable]
    public struct SerializableLatLng
    {
        public double Latitude;
        public double Longitude;

        public SerializableLatLng(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }

        // Convert SerializableLatLng to LatLng
        public LatLng ToLatLng()
        {
            return new LatLng(Latitude, Longitude);
        }

        // Convert LatLng to SerializableLatLng
        public static SerializableLatLng FromLatLng(LatLng latLng)
        {
            return new SerializableLatLng(latLng.Latitude, latLng.Longitude);
        }


    }

    public SerializableLatLng coordinates;
}
