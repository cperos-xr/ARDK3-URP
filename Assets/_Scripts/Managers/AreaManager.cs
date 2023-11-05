using Niantic.Lightship.Maps.Core.Coordinates;
using Niantic.Lightship.Maps.MapLayers.Components;
using Niantic.Lightship.Maps.Samples.CustomMapLayers.StateBoundaries;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public class AreaManager : MonoBehaviour
{
    public static AreaManager Instance;

    public List<SO_AreaData> allAreas;
    public List <SO_AreaData> currentAreas;
    //[SerializeField] private EntityManager entityManager;

    //[SerializeField] private LayerGameObjectPlacement _cubeGOP;  // For the border
    [SerializeField] private LayerLineRenderer lineRenderer; // For the border
    [SerializeField] private PolygonColorSelection polygonColorSelection; // For the border
    [SerializeField] private LayerSetter layerSetter;

    [SerializeField] private TextMeshProUGUI currentAreaName;

    private void Awake()
    {
        // Ensure there is only one instance of QuestManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    public void InitializeArea(SO_AreaData areaData)
    {


        //place area entities
        //entityManager.LoadAllEntities(areaData);
        EntityManager.Instance.MapAllEntities(areaData);

        //Draw the Lines
        Debug.Log("Drawing Lines");

        //Fill the Lines with Area Material
        foreach (var boundary in areaData._boundaries)
        {
            lineRenderer.DrawLoop(boundary.Points, areaData.areaName);

            // Get the LayerPolygonRenderer for the color "red"
            LayerPolygonRenderer polygonRenderer = polygonColorSelection.GetRendererByColorName(areaData.areaColor);
            polygonRenderer.DrawPolygon(boundary.Points, areaData.areaName);
        }

        // Set Line to the minimap Layer
        layerSetter.SetLayerRecursive();

    }
    private void Start()
    {
        foreach(SO_AreaData area in allAreas)
        {
            area.hasBeenVisited = false;
        }
    }

    private void OnEnable()
    {
        LevelManager.OnPlayerPositionChanged += HandlePlayerPositionChanged;
    }

    private void OnDisable()
    {
        LevelManager.OnPlayerPositionChanged -= HandlePlayerPositionChanged;
    }

    private void HandlePlayerPositionChanged(LatLng newPosition)
    {
        Debug.Log("handle player position change");

        List<SO_AreaData> newCurrentAreas = new List<SO_AreaData>();

        foreach (SO_AreaData areaData in allAreas)
        {
            if (IsPlayerInArea(areaData, newPosition))
            {
                newCurrentAreas.Add(areaData);
                areaData.hasBeenVisited = true;

                if (!currentAreas.Contains(areaData))
                {
                    // Player has entered a new area
                    Debug.Log($"Player has entered area {areaData.name}");
                    EntityManager.Instance.LoadAllEntities(areaData);
                }
            }
            else if (currentAreas.Contains(areaData))
            {
                // Player has left an area
                Debug.Log($"Player has left area {areaData.name}");
                EntityManager.Instance.UnloadEntities(areaData);
            }
        }

        // Update the list of current areas
        currentAreas = newCurrentAreas;

        // Update the text to display the names of the current areas
        if (currentAreas.Count > 0)
        {
            currentAreaName.text = string.Join(", ", currentAreas.Select(area => area.name));
        }
        else
        {
            currentAreaName.text = "Not in any area";
        }
    }


    public bool IsPlayerInArea(SO_AreaData area, LatLng playerPosition)
    {
        bool isPlayerInArea = false;
        foreach (Boundary boundary in area._boundaries)
        {
            if (GeoUtility.IsPointInPolygon(playerPosition, boundary.Points)) isPlayerInArea = true;
        }
        return isPlayerInArea;

    }




    //private void PlaceObject(LatLng pos) => _cubeGOP.PlaceInstance(pos);



}
