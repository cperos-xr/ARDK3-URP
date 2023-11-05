using Niantic.Lightship.Maps.Core.Coordinates;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GeoUtility : MonoBehaviour
{
    public bool test;
    public string point;

    private List<LatLng> polygon = new List<LatLng>();
    // Radius of the Earth in meters
    private const double EarthRadius = 6371000.0;



    private void Start()
    {
        List<LatLng> waikiki = new List<LatLng>
    {
        new LatLng(21.287100718153493, -157.84117787543883),
        new LatLng(21.28845052225643, -157.8337117710136),
        new LatLng(21.27374045050371, -157.81658801907315),
        new LatLng(21.271394072170416, -157.82323736283294),
        new LatLng(21.28102600932316, -157.84028808610893)
    };
        polygon = waikiki;
    }
    private void Update()
    {
        if (test)
        {
            test = false;
            Debug.Log("Is the point in the polygon? " + IsPointInPolygon(ParseString(point), polygon));
        }
    }

    public static LatLng ParseString(string input)
    {
        // Remove parentheses if they exist
        input = input.Replace("(", "").Replace(")", "");

        string[] parts = input.Split(',');
        if (parts.Length == 2)
        {
            if (double.TryParse(parts[0], out double x) && double.TryParse(parts[1], out double y))
            {
                return new LatLng(x, y);
            }
        }

        // If parsing fails, return a default value or throw an exception
        // You can choose how you want to handle parsing errors.
        return new LatLng(0, 0);
    }


    public static bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
    {
        LatLng dv2Point = new LatLng(point.x, point.y);
        List<LatLng> dv2Polygon = new List<LatLng>();

        foreach (Vector2 v in polygon)
        {
            dv2Polygon.Add(new LatLng(v.x, v.y));

        }

        return IsPointInPolygon(dv2Point, dv2Polygon);
    }

    public static bool IsPointInPolygon(LatLng point, List<LatLng> polygon)
    {
        bool isInside = false;
        int n = polygon.Count;
        double x = point.Latitude;
        double y = point.Longitude;

        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            double xi = polygon[i].Latitude;
            double yi = polygon[i].Longitude;
            double xj = polygon[j].Latitude;
            double yj = polygon[j].Longitude;

            bool intersect = ((yi > y) != (yj > y)) &&
                            (x < (xj - xi) * (y - yi) / (yj - yi) + xi);

            if (intersect)
                isInside = !isInside;
        }

        return isInside;
    }


    public static float CalculateDistance(LatLng point1, LatLng point2)
    {
        double lat1 = DegToRad(point1.Latitude);
        double lon1 = DegToRad(point1.Longitude);
        double lat2 = DegToRad(point2.Latitude);
        double lon2 = DegToRad(point2.Longitude);

        double dLat = lat2 - lat1;
        double dLon = lon2 - lon1;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        // Calculate the distance in meters
        double distance = EarthRadius * c;

        return (float)distance;
    }

    private static double DegToRad(double degrees)
    {
        return degrees * (Math.PI / 180.0);
    }

}
