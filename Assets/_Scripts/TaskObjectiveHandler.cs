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

        //AR Image Tracking is called from ARTrack

        //targetLatLng = new LatLng(targetLocation.Latitude, targetLocation.Longitude);
    }

    private void OnDisable()
    {
        LevelManager.OnPlayerPositionChanged -= HandlePlayerPositionChanged;
        ItemManager.OnPlayerGivenItem -= HandlePlayerGivenItem;
        InteractionManager.OnPlayerInteraction -= HandlePlayerInteraction;
        InteractionManager.OnPlayerEntityInteraction -= HandlePlayerEntityInteraction;

    }

    private void HandlePlayerEntityInteraction(BaseEntityData entity)
    {
        //Check Player Interactions
        foreach (KeyValuePair<SO_TaskObjective_EntityInteraction, bool> kvp in entityInteractionObjectives)
        {
            if (!kvp.Value) // If the task has not been completed
            {
                SO_TaskObjective_EntityInteraction taskObjective = kvp.Key;

                if (entity == taskObjective.targetEntity)  // this line here???
                {
                    entityInteractionObjectives[taskObjective] = true;
                    Debug.Log($"Entity Interaction task complete {taskObjective.objectiveName}");
                }

            }
        }
    }

    private void HandlePlayerInteraction(Interaction interaction)
    {
        //Check Player Interactions
        foreach (KeyValuePair<SO_TaskObjective_Interaction, bool> kvp in interactionObjectives)
        {
            if (!kvp.Value) // If the task has not been completed
            {
                SO_TaskObjective_Interaction taskObjective = kvp.Key;

                if (interaction.Equals(taskObjective.targetInteraction))
                {
                    interactionObjectives[taskObjective] = true;
                    Debug.Log($"Interaction task complete {taskObjective.objectiveName}");
                }

            }
        }
    }

    private void HandlePlayerGivenItem(SO_ItemData item)
    {
        foreach (KeyValuePair<SO_TaskObjective_ItemCollection, bool> kvp in itemCollectionObjectives)
        {
            if (!kvp.Value) // If the task has not been completed
            {
                SO_TaskObjective_ItemCollection taskObjective = kvp.Key;

                if (PlayerManager.Instance.inventory.CheckForItem(taskObjective.itemToCollect) >= taskObjective.requiredProgress)
                {
                    itemCollectionObjectives[taskObjective] = true;
                    Debug.Log($"Item Collection task complete {taskObjective.objectiveName}");
                }
            }
        }
    }

    private void HandlePlayerPositionChanged(LatLng newPosition)
    {
        CheckGPSLocationObjectivesForCompletions(newPosition);
        CheckEnterAreaObjectivesForCompletions();
    }

    public void CheckGPSLocationObjectivesForCompletions(LatLng newPosition)
    {
        foreach (KeyValuePair<SO_TaskObjective_GPSLocation, bool> kvp in gpsLocationObjectives)
        {
            if (!kvp.Value) // If the task has not been completed
            {
                SO_TaskObjective_GPSLocation taskObjective = kvp.Key;

                float distance = GeoUtility.CalculateDistance(taskObjective.targetLatLng, newPosition);

                if (distance <= taskObjective.radius)
                {
                    Debug.Log($"GPS task Complete {taskObjective.objectiveName}");

                    // Update the value in the dictionary
                    gpsLocationObjectives[taskObjective] = true;
                }
            }
        }
    }

    public void CheckEnterAreaObjectivesForCompletions()
    {
        foreach (KeyValuePair<SO_TaskObjective_EnterArea, bool> kvp in enterAreaObjectives)
        {
            if (!kvp.Value) // If the task has not been completed
            {
                SO_TaskObjective_EnterArea taskObjective = kvp.Key;

                foreach (SO_AreaData area in AreaManager.Instance.currentAreas)
                {
                    if (area == taskObjective.targetArea)
                    {
                        Debug.Log($"Enter area task complete {taskObjective.objectiveName}");

                        // Update the value in the dictionary
                        enterAreaObjectives[taskObjective] = true;
                    }
                }

            }
        }
    }

    public void CheckARTrackedImageObjectivesForCompletion(XRReferenceImage referenceImage)
    {
        Debug.Log("Checking arTrackedImageObjectives for reference image " + referenceImage.name);

        foreach (KeyValuePair<SO_TaskObjective_TrackedARImage, bool> kvp in arTrackedImageObjectives)
        {

            if (!kvp.Value) // If the task has not been completed
            {
                SO_TaskObjective_TrackedARImage taskObjective = kvp.Key;

                Debug.Log($"Checking {referenceImage.name} against {taskObjective.targetReferenceImage.name}");

                if (taskObjective.targetReferenceImage.Equals(referenceImage)) //Does this Reference image complete the objective?
                {
                    arTrackedImageObjectives[taskObjective] = true;
                    Debug.Log($"AR Tracking task complete based on reference image {taskObjective.objectiveName}");
                }
            }
        }
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

        if (objective is SO_TaskObjective_ItemCollection taskObjective_ItemCollection)
        {
            itemCollectionObjectives.Add(taskObjective_ItemCollection, false);
        }
        else if (objective is SO_TaskObjective_GPSLocation taskObjective_GPSLocation)
        {
            gpsLocationObjectives.Add(taskObjective_GPSLocation, false);
        }
        else if (objective is SO_TaskObjective_TrackedARImage taskObjective_TrackedARImage)
        {
            arTrackedImageObjectives.Add(taskObjective_TrackedARImage, false);
        }
        else if (objective is SO_TaskObjective_EnterArea taskObjective_EnterArea)
        {
            enterAreaObjectives.Add(taskObjective_EnterArea, false);
        }
        else if (objective is SO_TaskObjective_EntityInteraction taskObjective_EntityInteraction)
        {
            entityInteractionObjectives.Add(taskObjective_EntityInteraction, false);
        }
        else if (objective is SO_TaskObjective_Interaction taskObjective_Interaction)
        {
            interactionObjectives.Add(taskObjective_Interaction, false);
        }
        // Add similar cases for other objective types...
    }



}
