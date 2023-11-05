using Niantic.Lightship.Maps.MapLayers.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PolygonColorSelection : MonoBehaviour
{
    // You can populate this list via the Unity Inspector.
    public List<RenderColor> polygonColors = new List<RenderColor>();

    // A dictionary to map color names to LayerPolygonRenderer instances.
    private Dictionary<string, LayerPolygonRenderer> colorRendererDictionary =
        new Dictionary<string, LayerPolygonRenderer>();

    private void OnEnable()
    {
        // Clear the existing dictionary.
        colorRendererDictionary.Clear();

        // Populate the dictionary with color names and their associated renderers.
        foreach (RenderColor colorData in polygonColors)
        {
            //colorData.polygonRenderer.name = colorData.colorName;
            colorRendererDictionary[colorData.colorName] = colorData.polygonRenderer;
        }
    }

    // A method to get the LayerPolygonRenderer for a given color name.
    public LayerPolygonRenderer GetRendererByColorName(string colorName)
    {
        // Check if the dictionary contains the color name and return the associated renderer.
        if (colorRendererDictionary.ContainsKey(colorName))
        {
            return colorRendererDictionary[colorName];
        }
        else
        {
            // Handle the case where the color name is not found.
            Debug.LogError($"Renderer for color '{colorName}' not found.");
            return null;
        }
    }
}

[Serializable]
public struct RenderColor
{
    [SerializeField] public string colorName;
    [SerializeField] public LayerPolygonRenderer polygonRenderer;
}
