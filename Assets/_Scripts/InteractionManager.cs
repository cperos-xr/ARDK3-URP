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

        // Determine the current interaction for the entity
        SO_Interaction interaction = interactionProgressionDictionary.ContainsKey(entity)
            ? interactionProgressionDictionary[entity]
            : entity.interactions[entity.startingInteractionIndex];

        // Trigger the interaction
        OnPlayerInteraction?.Invoke(interaction);

        // Handle items and quests associated with the interaction
        HandleInteractionItemsAndQuests(interaction, entity);

        // Check for the next interaction and update if necessary
        if (interaction.changes != null)
        {
            foreach(InteractionProgression.EntityInteractionChange interactionChange in  interaction.changes)
            {
                UpdateInteraction(entity, interactionChange.newInteraction);
            }


        }
        else
        {
            Debug.Log("No next interaction specified, or interaction is meant to repeat.");
        }
    }



    internal void UpdateInteraction(BaseEntityData entity, SO_Interaction newInteraction)
    {
        Debug.Log($"Updating Entity Interaction for {entity.entityName} and changing current interaction to {newInteraction.InteractionName}");

        // Check if the entity is already in the progression dictionary
        if (interactionProgressionDictionary.ContainsKey(entity))
        {
            // Get the current interaction
            SO_Interaction currentInteraction = interactionProgressionDictionary[entity];

            // Check if the current interaction is different from the new interaction
            if (currentInteraction != newInteraction)
            {
                // Reset the interaction count for the new interaction
                if (entityInteractionCounts.ContainsKey(entity))
                {
                    // Remove the current interaction count to reset it
                    entityInteractionCounts[entity].Remove(currentInteraction);
                    Debug.Log($"Removed interaction count for current interaction: {currentInteraction.InteractionName}");

                    // Initialize or reset the count for the new interaction
                    entityInteractionCounts[entity][newInteraction] = 0;
                    Debug.Log($"Set interaction count to 0 for new interaction: {newInteraction.InteractionName}");
                }
                else
                {
                    // If the entity is not in the dictionary, add it with the new interaction count initialized
                    entityInteractionCounts.Add(entity, new Dictionary<SO_Interaction, int>
                {
                    { newInteraction, 0 }
                });
                    Debug.Log($"Added new entity to interaction counts with interaction: {newInteraction.InteractionName}");
                }
            }
            else
            {
                Debug.Log("New interaction is the same as the current interaction, no update needed.");
            }

            // Update the interaction in the progression dictionary
            interactionProgressionDictionary[entity] = newInteraction;
            Debug.Log($"Interaction progression dictionary updated for entity: {entity.entityName}");
        }
        else
        {
            // If the entity is not in the progression dictionary, add it with the new interaction
            interactionProgressionDictionary.Add(entity, newInteraction);

            // Ensure the entity is also added to the interaction counts dictionary
            if (!entityInteractionCounts.ContainsKey(entity))
            {
                entityInteractionCounts.Add(entity, new Dictionary<SO_Interaction, int>());
                Debug.Log($"Added new entity to interaction counts dictionary: {entity.entityName}");
            }

            // Initialize or reset the count for the new interaction
            entityInteractionCounts[entity][newInteraction] = 0;
            Debug.Log($"Set interaction count to 0 for new entity: {newInteraction.InteractionName}");
        }
    }

    private void HandleInteractionItemsAndQuests(SO_Interaction interaction, BaseEntityData entity)
    {
        // Check if this interaction is related to tasks.
        bool hasQuest = interaction.quest != null;

        // Check if this interaction provides items.
        bool hasItems = interaction.itemDatas != null && interaction.itemDatas.Count > 0;

        if (hasItems)
        {
            // Handle item assignment to the player's inventory here.
            foreach (SO_ItemData itemData in interaction.itemDatas)
            {
                if (!itemData.isLocked)
                {
                    itemManager.AddItemToPlayerInventory(itemData, entity);
                }
            }
        }

        if (hasQuest)
        {
            // Handle quest assignment from TaskManager.
            QuestManager.Instance.AssignQuest(interaction.quest);
        }
    }


}

