#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Niantic.Lightship.Maps.Samples.CustomMapLayers.StateBoundaries;
using Niantic.Lightship.Maps.Core.Coordinates;

[CustomEditor(typeof(SO_AreaData))]
public class SO_AreaDataEditor : Editor
{
    private string latLngString = "";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();  // Draw the default inspector

        SO_AreaData myTarget = (SO_AreaData)target;

        // Provide a text field for the user to paste the "lat,lng" string
        latLngString = EditorGUILayout.TextField("Enter LatLng:", latLngString);

        if (GUILayout.Button("Parse and Add"))
        {
            string[] splitValues = latLngString.Split(',');
            if (splitValues.Length == 2)
            {
                float lat, lon;
                if (float.TryParse(splitValues[0], out lat) && float.TryParse(splitValues[1], out lon))
                {
                    // Assuming LatLng has a constructor that takes lat and lon
                    LatLng newLatLng = new LatLng(lat, lon);

                    // Add the new LatLng point to the boundaries list
                    if (myTarget._boundaries.Count == 0)
                    {
                        // If there are no boundaries yet, create one
                        Boundary newBoundary = new Boundary();
                        newBoundary.Points.Add(newLatLng);
                        myTarget._boundaries.Add(newBoundary);
                    }
                    else
                    {
                        // Otherwise, add the point to the existing boundary
                        myTarget._boundaries[0].Points.Add(newLatLng);
                    }

                    latLngString = ""; // Clear the string
                }
            }
        }
    }
}
#endif
