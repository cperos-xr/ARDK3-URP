#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Niantic.Lightship.Maps.Coordinates;
using Niantic.Lightship.Maps.Samples.CustomMapLayers.StateBoundaries;

[CustomEditor(typeof(SO_MapEntityData))]
public class SO_EntityDataEditor : Editor
{
    private string coordinateString = "";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw the default inspector fields

        SO_MapEntityData myTarget = (SO_MapEntityData)target;

        // Provide a text field for the user to input the coordinate string
        coordinateString = EditorGUILayout.TextField("Enter Coordinate (x,y):", coordinateString);

        if (GUILayout.Button("Parse and Set GPS Coordinates"))
        {
            // Split the input string into x and y components
            string[] splitValues = coordinateString.Split(',');

            if (splitValues.Length == 2)
            {
                float x, y;

                // Attempt to parse x and y as floats
                if (float.TryParse(splitValues[0], out x) && float.TryParse(splitValues[1], out y))
                {
                    // Create a new SerializableLatLng using the parsed x and y values
                    SerializableLatLng newLatLng = new SerializableLatLng(x, y);

                    // Set the gpsCoordinates field to the new coordinate
                    myTarget.gpsCoordinates = newLatLng;

                    coordinateString = ""; // Clear the input field
                }
            }
        }
    }
}
#endif
