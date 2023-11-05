using Niantic.Lightship.Maps;
using Niantic.Lightship.Maps.Core.Coordinates;
using Niantic.Lightship.Maps.MapLayers.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance;
    //[SerializeField] private List<SO_EntityData> currentInteractiveEntities = new List<SO_EntityData>();
    [SerializeField] private List<SO_MapEntityData> allCurrentAreaEntities = new List<SO_MapEntityData>();
    [SerializeField] private List<SO_MapEntityData> loadedEntities = new List<SO_MapEntityData>();
    [SerializeField] private InteractionManager interactionManager;
    [SerializeField] private TextMeshProUGUI currentEntityText;
    
    private List<SO_AreaData> currentAreas = new List<SO_AreaData>();

    //[SerializeField] private ItemStateManager itemStateManager;

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

    public void OnEnterArea(SO_AreaData area)
    {
        if (!currentAreas.Contains(area))
        {
            currentAreas.Add(area);
            LoadAllEntities(area);
        }
    }

    // Call this method when the player leaves an area
    public void OnLeaveArea(SO_AreaData area)
    {
        if (currentAreas.Contains(area))
        {
            currentAreas.Remove(area);
            UnloadEntities(area);
        }
    }

    public void LoadAllEntities(SO_AreaData currentArea)
    {
        foreach (SO_MapEntityData entity in currentArea.areaEntities)
        {
            if (!loadedEntities.Contains(entity))
            {
                allCurrentAreaEntities.Add(entity);
                loadedEntities.Add(entity);
                PlaceObject(entity.gpsCoordinates, entity); // Load entity to map
            }
        }
    }

    public void MapAllEntities(SO_AreaData currentArea)
    {
        foreach (SO_MapEntityData entity in currentArea.areaEntities)
        {
            PlaceObject(entity.gpsCoordinates, entity);
        }
    }

    private void PlaceObject(LatLng pos, SO_MapEntityData sO_EntityData) => sO_EntityData._cubeGOP.PlaceInstance(pos);



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
        foreach (SO_MapEntityData entity in allCurrentAreaEntities)
        {
            float distanceFromEnity = GeoUtility.CalculateDistance(newPosition, entity.gpsCoordinates);
            Debug.Log($"Player is {distanceFromEnity} meters from {entity.name}");
            if (distanceFromEnity < entity.interactionRadius)
            {
                Debug.Log($"Entity {entity.name} is within interaction distance");
                
                
                //GiveItemsFromEntityToPlayer(entity);
                interactionManager.HandleEntityInteraction(entity);
                
                
                currentEntityText.text = entity.name;
            }
            else
            {
                //itemManager.RemoveEntity(entity);
                
                
                currentEntityText.text = "No entities in range";
            }
        }
    }



    // You might want to call this when the player leaves an area
    public void UnloadEntities(SO_AreaData area)
    {
        foreach (SO_MapEntityData entity in area.areaEntities)
        {
            if (loadedEntities.Contains(entity))
            {
                // Here you'd put code to actually remove the entity from the game,
                // such as destroying its GameObject or disabling it.

                loadedEntities.Remove(entity);
                allCurrentAreaEntities.Remove(entity);
            }
        }
    }
}

