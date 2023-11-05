using UnityEngine;
using UnityEditor;
using Niantic.Lightship.Maps.Core.Coordinates;

[CustomPropertyDrawer(typeof(LatLng))]
public class LatLngDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var halfWidth = position.width / 2;

        var latRect = new Rect(position.x, position.y, halfWidth - 2, position.height);
        var lonRect = new Rect(position.x + halfWidth, position.y, halfWidth, position.height);

        EditorGUI.PropertyField(latRect, property.FindPropertyRelative("Latitude"), GUIContent.none);
        EditorGUI.PropertyField(lonRect, property.FindPropertyRelative("Longitude"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}
