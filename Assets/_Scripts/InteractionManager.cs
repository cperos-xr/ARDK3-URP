using System.Collections.Generic;
using System;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;
    [SerializeField] ItemManager itemManager;

    public delegate void PlayerInteractionEvent(SO_Interaction EntityNotifyPlayer);
    public static event PlayerInteractionEvent OnPlayerInteraction;

    public delegate void PlayerEntityInteractionEvent(BaseEntityData entity);
    public static event PlayerEntityInteractionEvent OnPlayerEntityInteraction;

    private Dictionary<BaseEntityData, Dictionary<SO_Interaction, int>> entityInteractionCounts = new Dictionary<BaseEntityData, Dictionary<SO_Interaction, int>>();

    public Dictionary<BaseEntityData, SO_Interaction> interactionProgressionDictionary = new Dictionary<BaseEntityData, SO_Interaction>();

    //[SerializeField] QuestManager questManager;


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

    public void HandleEntityInteraction(BaseEntityData entity)
    {
        if (entity == null)
        {
            Debug.LogError("Entity is null.");
            return; // Exit the method to avoid further issues.
        }

        Debug.Log("Handling Entity Interaction for " + entity.entityName);
        OnPlayerEntityInteraction?.Invoke(entity);

        // Get the current interaction from the progression dictionary or the default starting interaction
        SO_Interaction currentInteraction = interactionProgressionDictionary.ContainsKey(entity)
            ? interactionProgressionDictionary[entity]
            : entity.interactions[entity.startingInteractionIndex];

        // Perform the interaction
        OnPlayerInteraction?.Invoke(currentInteraction);

        // ... (rest of your existing code for handling items and quests)

        // Check if there's a next interaction defined for the current interaction
        if (currentInteraction.interactionModifications != null && currentInteraction.interactionModifications.Count > 0)
        {
            foreach (var interactionModification in currentInteraction.interactionModifications)
            {
                // Update the interaction to the next one
                UpdateInteraction(entity, interactionModification.newInteraction);
            }
        }
        else
        {
            // If there's no next interaction, this interaction should occur only once
            // You can remove the entity from the dictionary to prevent further interactions
            interactionProgressionDictionary.Remove(entity);
            Debug.Log("No next interaction specified for " + entity.entityName + ", interaction will not repeat.");
        }
    }

    internal void UpdateInteraction(BaseEntityData entity, SO_Interaction newInteraction)
    {
        if (newInteraction == null)
        {
            // If newInteraction is null, it means this interaction should not repeat
            interactionProgressionDictionary.Remove(entity);
            Debug.Log("Interaction for " + entity.entityName + " is set to not repeat and will be removed from progression.");
        }
        else if (interactionProgressionDictionary.ContainsKey(entity))
        {
            // Update the interaction in the progression dictionary
            interactionProgressionDictionary[entity] = newInteraction;
            Debug.Log("Updating Entity Interaction for " + entity.entityName + " to " + newInteraction.name);
        }
        else
        {
            // If the entity is not in the progression dictionary, add it with the new interaction
            interactionProgressionDictionary.Add(entity, newInteraction);
            Debug.Log("Setting initial Entity Interaction for " + entity.entityName + " to " + newInteraction.name);
        }
    }



}

