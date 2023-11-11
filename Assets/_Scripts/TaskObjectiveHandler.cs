using Niantic.Lightship.Maps.Core.Coordinates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARSubsystems;

public class TaskObjectiveHandler : MonoBehaviour
{
    public static TaskObjectiveHandler Instance;

    public Dictionary<SO_TaskObjective_GPSLocation, bool> gpsLocationObjectives = new Dictionary<SO_TaskObjective_GPSLocation, bool>();

    public Dictionary<SO_TaskObjective_ItemCollection, bool> itemCollectionObjectives = new Dictionary<SO_TaskObjective_ItemCollection, bool>();

    public Dictionary<SO_TaskObjective_TrackedARImage, bool> arTrackedImageObjectives = new Dictionary<SO_TaskObjective_TrackedARImage, bool>();

    public Dictionary<SO_TaskObjective_EnterArea, bool> enterAreaObjectives = new Dictionary<SO_TaskObjective_EnterArea, bool>();

    public Dictionary<SO_TaskObjective_EntityInteraction, bool> entityInteractionObjectives = new Dictionary<SO_TaskObjective_EntityInteraction, bool>();

    public Dictionary<SO_TaskObjective_Interaction, bool> interactionObjectives = new Dictionary<SO_TaskObjective_Interaction, bool>();

    public Dictionary<SO_TaskObjective_Semantic, bool> semanticObjectives = new Dictionary<SO_TaskObjective_Semantic, bool>();

    // Start is called before the first frame update
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


    private void OnEnable()
    {
        LevelManager.OnPlayerPositionChanged += HandlePlayerPositionChanged;
        ItemManager.OnPlayerGivenItem += HandlePlayerGivenItem;
        InteractionManager.OnPlayerInteraction += HandlePlayerInteraction;
        InteractionManager.OnPlayerEntityInteraction += HandlePlayerEntityInteraction;
        SemanticChannelDetector.OnSemanticChannelIdentified += HandleSemanticChannelIdentified;

        //AR Image Tracking is called from ARTrack

        //targetLatLng = new LatLng(targetLocation.Latitude, targetLocation.Longitude);
    }

    private void OnDisable()
    {
        LevelManager.OnPlayerPositionChanged -= HandlePlayerPositionChanged;
        ItemManager.OnPlayerGivenItem -= HandlePlayerGivenItem;
        InteractionManager.OnPlayerInteraction -= HandlePlayerInteraction;
        InteractionManager.OnPlayerEntityInteraction -= HandlePlayerEntityInteraction;
        SemanticChannelDetector.OnSemanticChannelIdentified -= HandleSemanticChannelIdentified;

    }

    private void HandlePlayerEntityInteraction(BaseEntityData entity)
    {
        var keys = new List<SO_TaskObjective_EntityInteraction>(entityInteractionObjectives.Keys);
        foreach (var key in keys)
        {
            if (!entityInteractionObjectives[key]) // If the task has not been completed
            {
                if (entity == key.targetEntity)
                {
                    entityInteractionObjectives[key] = true;
                    Debug.Log($"Entity Interaction task complete {key.objectiveName}");
                }
            }
        }
    }

    private void HandlePlayerInteraction(SO_Interaction interaction)
    {
        var keys = new List<SO_TaskObjective_Interaction>(interactionObjectives.Keys);
        foreach (var key in keys)
        {
            if (!interactionObjectives[key]) // If the task has not been completed
            {
                if (interaction.Equals(key.targetInteraction))
                {
                    interactionObjectives[key] = true;
                    Debug.Log($"Interaction task complete {key.objectiveName}");
                }
            }
        }
    }

    private void HandlePlayerGivenItem(SO_ItemData item)
    {
        var keys = new List<SO_TaskObjective_ItemCollection>(itemCollectionObjectives.Keys);
        foreach (var key in keys)
        {
            if (!itemCollectionObjectives[key]) // If the task has not been completed
            {
                if (PlayerManager.Instance.inventory.CheckForItem(key.itemToCollect) >= key.requiredProgress)
                {
                    itemCollectionObjectives[key] = true;
                    Debug.Log($"Item Collection task complete {key.objectiveName}");
                }
            }
        }
    }

    public void CheckGPSLocationObjectivesForCompletions(LatLng newPosition)
    {
        var keys = new List<SO_TaskObjective_GPSLocation>(gpsLocationObjectives.Keys);
        foreach (var key in keys)
        {
            if (!gpsLocationObjectives[key]) // If the task has not been completed
            {
                float distance = GeoUtility.CalculateDistance(key.targetLatLng, newPosition);

                if (distance <= key.radius)
                {
                    gpsLocationObjectives[key] = true;
                    Debug.Log($"GPS task Complete {key.objectiveName}");
                }
            }
        }
    }

    public void CheckEnterAreaObjectivesForCompletions()
    {
        var keys = new List<SO_TaskObjective_EnterArea>(enterAreaObjectives.Keys);
        foreach (var key in keys)
        {
            if (!enterAreaObjectives[key]) // If the task has not been completed
            {
                foreach (SO_AreaData area in AreaManager.Instance.currentAreas)
                {
                    if (area == key.targetArea)
                    {
                        enterAreaObjectives[key] = true;
                        Debug.Log($"Enter area task complete {key.objectiveName}");
                    }
                }
            }
        }
    }

    public void CheckARTrackedImageObjectivesForCompletion(XRReferenceImage referenceImage)
    {
        var keys = new List<SO_TaskObjective_TrackedARImage>(arTrackedImageObjectives.Keys);
        foreach (var key in keys)
        {
            if (!arTrackedImageObjectives[key]) // If the task has not been completed
            {
                if (key.targetReferenceImage.Equals(referenceImage)) //Does this Reference image complete the objective?
                {
                    arTrackedImageObjectives[key] = true;
                    Debug.Log($"AR Tracking task complete based on reference image {key.objectiveName}");
                }
            }
        }
    }

    private void HandleSemanticChannelIdentified(string identifiedChannel)
    {
        var keys = new List<SO_TaskObjective_Semantic>(semanticObjectives.Keys);
        foreach (var key in keys)
        {
            if (!semanticObjectives[key]) // If the task has not been completed
            {
                if (DoesChannelMatch(identifiedChannel, key.selectedChannel))
                {
                    semanticObjectives[key] = true; // Mark the task as complete
                    Debug.Log($"Semantic {identifiedChannel} task complete {key.objectiveName}");
                }
            }
        }
    }

    private void HandleSemanticChannelIdentified(List <string> identifiedChannels)
    {
        Debug.Log("Handle Sematic Channel");
        // Assuming you have a dictionary to track the completion status of semantic objectives
        foreach (var channel in identifiedChannels)
        {
            Debug.Log("channel is " + channel);
            HandleSemanticChannelIdentified (channel);
        }
    }

    private void HandlePlayerPositionChanged(LatLng newPosition)
    {
        CheckGPSLocationObjectivesForCompletions(newPosition);
        CheckEnterAreaObjectivesForCompletions();
    }

    private bool DoesChannelMatch(string identifiedChannel, string taskChannel)
    {
        identifiedChannel = identifiedChannel.Trim();
        taskChannel = taskChannel.Trim();

        Debug.Log($"Checking for match with '{identifiedChannel}' and '{taskChannel}'");
        Debug.Log($"Lengths - identifiedChannel: {identifiedChannel.Length}, taskChannel: {taskChannel.Length}");

        // Check for an exact match
        if (string.Equals(identifiedChannel, taskChannel, System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Exact match: true");
            return true;
        }

        // Check for a match with the experimental suffix removed
        if (taskChannel.EndsWith("_experimental", System.StringComparison.OrdinalIgnoreCase))
        {
            string nonExperimentalChannel = taskChannel.Replace("_experimental", "");
            if (string.Equals(identifiedChannel, nonExperimentalChannel, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("Match with experimental suffix removed: true");
                return true;
            }
        }

        // Check for a match with the experimental suffix added
        if (!identifiedChannel.EndsWith("_experimental", System.StringComparison.OrdinalIgnoreCase))
        {
            string experimentalChannel = identifiedChannel + "_experimental";
            if (string.Equals(taskChannel, experimentalChannel, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("Match with experimental suffix added: true");
                return true;
            }
        }

        Debug.Log("No match: false");
        return false;
    }




    public bool IsObjectiveCompleted(SO_TaskObjective objective)
    {
        if (objective == null)
        {
            Debug.LogError("Objective is null!");
            return false;
        }

        if (objective is SO_TaskObjective_ItemCollection taskObjective_ItemCollection)
        {
            if (itemCollectionObjectives.TryGetValue(taskObjective_ItemCollection, out bool isCompleted))
            {
                return isCompleted;
            }
            else
            {
                Debug.LogError("Task objective not found in itemCollectionObjectives dictionary!");
                return false;
            }
        }
        else if (objective is SO_TaskObjective_GPSLocation taskObjective_GPSLocation)
        {
            if (gpsLocationObjectives.TryGetValue(taskObjective_GPSLocation, out bool isCompleted))
            {
                return isCompleted;
            }
            else
            {
                Debug.LogError("Task objective not found in gpsLocationObjectives dictionary!");
                return false;
            }
        }
        else if (objective is SO_TaskObjective_TrackedARImage taskObjective_TrackedARImage)
        {
            if (arTrackedImageObjectives.TryGetValue(taskObjective_TrackedARImage, out bool isCompleted))
            {
                return isCompleted;
            }
            else
            {
                Debug.LogError("Task objective not found in arTrackedImageObjectives dictionary!");
                return false;
            }
        }
        else if (objective is SO_TaskObjective_EnterArea taskObjective_EnterArea)
        {
            if (enterAreaObjectives.TryGetValue(taskObjective_EnterArea, out bool isCompleted))
            {
                return isCompleted;
            }
            else
            {
                Debug.LogError("Task objective not found in enterAreaObjectives dictionary!");
                return false;
            }
        }
        else if (objective is SO_TaskObjective_EntityInteraction taskObjective_EntityInteraction)
        {
            if (entityInteractionObjectives.TryGetValue(taskObjective_EntityInteraction, out bool isCompleted))
            {
                return isCompleted;
            }
            else
            {
                Debug.LogError("Task objective not found in entityInteractionObjectives dictionary!");
                return false;
            }
        }
        else if (objective is SO_TaskObjective_Interaction taskObjective_Interaction)
        {
            if (interactionObjectives.TryGetValue(taskObjective_Interaction, out bool isCompleted))
            {
                return isCompleted;
            }
            else
            {
                Debug.LogError("Task objective not found in interactionObjectives dictionary!");
                return false;
            }
        }
        else if (objective is SO_TaskObjective_Semantic taskObjective_Semantic)
        {
            if (semanticObjectives.TryGetValue(taskObjective_Semantic, out bool isCompleted))
            {
                return isCompleted;
            }
            else
            {
                Debug.LogError("Task objective not found in interactionObjectives dictionary!");
                return false;
            }
        }

        // Add similar checks for other objective types...

        Debug.LogError("Unsupported objective type!");
        return false;
    }

    // Method to assign an objective to a task
    public void AssignObjective(SO_Task task, SO_TaskObjective objective)
    {
        if (task == null || objective == null)
        {
            Debug.LogError("Task or objective is null.");
            return;
        }

        // Check and add for SO_TaskObjective_ItemCollection
        if (objective is SO_TaskObjective_ItemCollection taskObjective_ItemCollection)
        {
            if (!itemCollectionObjectives.ContainsKey(taskObjective_ItemCollection))
            {
                itemCollectionObjectives.Add(taskObjective_ItemCollection, false);
            }
        }
        // Check and add for SO_TaskObjective_GPSLocation
        else if (objective is SO_TaskObjective_GPSLocation taskObjective_GPSLocation)
        {
            if (!gpsLocationObjectives.ContainsKey(taskObjective_GPSLocation))
            {
                gpsLocationObjectives.Add(taskObjective_GPSLocation, false);
            }
        }
        // Check and add for SO_TaskObjective_TrackedARImage
        else if (objective is SO_TaskObjective_TrackedARImage taskObjective_TrackedARImage)
        {
            if (!arTrackedImageObjectives.ContainsKey(taskObjective_TrackedARImage))
            {
                arTrackedImageObjectives.Add(taskObjective_TrackedARImage, false);
            }
        }
        // Check and add for SO_TaskObjective_EnterArea
        else if (objective is SO_TaskObjective_EnterArea taskObjective_EnterArea)
        {
            if (!enterAreaObjectives.ContainsKey(taskObjective_EnterArea))
            {
                enterAreaObjectives.Add(taskObjective_EnterArea, false);
            }
        }
        // Check and add for SO_TaskObjective_EntityInteraction
        else if (objective is SO_TaskObjective_EntityInteraction taskObjective_EntityInteraction)
        {
            if (!entityInteractionObjectives.ContainsKey(taskObjective_EntityInteraction))
            {
                entityInteractionObjectives.Add(taskObjective_EntityInteraction, false);
            }
        }
        // Check and add for SO_TaskObjective_Interaction
        else if (objective is SO_TaskObjective_Interaction taskObjective_Interaction)
        {
            if (!interactionObjectives.ContainsKey(taskObjective_Interaction))
            {
                interactionObjectives.Add(taskObjective_Interaction, false);
            }
        }
        // Check and add for SO_TaskObjective_Semantic
        else if (objective is SO_TaskObjective_Semantic taskObjective_Semantic)
        {
            if (!semanticObjectives.ContainsKey(taskObjective_Semantic))
            {
                semanticObjectives.Add(taskObjective_Semantic, false);
            }
        }
        // Add similar cases for other objective types...
    }




}
