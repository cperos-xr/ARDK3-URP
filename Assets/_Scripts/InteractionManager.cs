using System.Collections.Generic;
using System;
using UnityEngine;
using static InteractionProgression;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;
    [SerializeField] ItemManager itemManager;

    public delegate void PlayerInteractionEvent(SO_Interaction EntityNotifyPlayer);
    public static event PlayerInteractionEvent OnPlayerInteraction;

    public delegate void PlayerEntityInteractionEvent(BaseEntityData entity);
    public static event PlayerEntityInteractionEvent OnPlayerEntityInteraction;

    public Dictionary<BaseEntityData, SO_Interaction> interactionProgressionDictionary = new Dictionary<BaseEntityData, SO_Interaction>();

    public Dictionary<BaseEntityData, SO_Interaction> previousInteractionDictionary = new Dictionary<BaseEntityData, SO_Interaction>();

    public struct InteractionContainer
    {
        public SO_Interaction interaction;
        public SO_Interaction previousInteraction;
    }
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
        // Check if the entity is null
        if (entity == null)
        {
            Debug.LogError("Entity is null.");
            return; // Exit the method to avoid further issues.
        }

        Debug.Log("Handling Entity Interaction for " + entity.entityName);
        OnPlayerEntityInteraction?.Invoke(entity);

        // Check if this is the first interaction with the entity
        if (!interactionProgressionDictionary.ContainsKey(entity))
        {
            // If it is, use the starting interaction
            if (entity.startingInteraction != null)
            {
                previousInteractionDictionary.Add(entity, null); // no previous interaction
                interactionProgressionDictionary.Add(entity, entity.startingInteraction);
            }
            else
            {
                Debug.Log("Starting interaction for entity " + entity.entityName + " is null.");
                return; // Exit the method to avoid further issues.
            }
        }

        // Retrieve the current interaction for the entity
        SO_Interaction interaction = interactionProgressionDictionary[entity];

        // Ensure that the interaction is not null
        if (interaction == null)
        {
            Debug.Log("Interaction for entity " + entity.entityName + " is null.");
            return; // Exit the method to avoid further issues.
        }

        // Invoke the interaction event
        OnPlayerInteraction?.Invoke(interaction);

        //Update all entity interations
        if (interaction.entityInteractionUpdates.Count > 0)
        {
            Debug.Log("entityInteractionUpdates has count greater than 0 " + interaction.InteractionName);
            UpdateAllEntityInteractions(interaction);

        }



        // Handle items if any are associated with this interaction
        if (interaction.itemDatas != null)
        {
            foreach (SO_ItemData itemData in interaction.itemDatas)
            {
                if (!itemData.isLocked)
                {
                    // Ensure itemManager is not null
                    if (itemManager != null)
                    {
                        itemManager.AddItemToPlayerInventory(itemData, entity);
                    }
                    else
                    {
                        Debug.LogError("ItemManager is not set in InteractionManager.");
                        return; // Exit the method to avoid further issues.
                    }
                }
            }
        }

        // Handle quests if any are associated with this interaction
        if (interaction.quest != null)
        {
            // Ensure QuestManager.Instance is not null
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.AssignQuest(interaction.quest);
            }
            else
            {
                Debug.LogError("QuestManager.Instance is null.");
                return; // Exit the method to avoid further issues.
            }
        }

        // If you want to update the interaction after handling, you can do so here
        // For example, if the interaction should change after being handled once:
        // UpdateInteractionIndex(entity, interaction.nextInteraction);
    }

    public void UpdateAllEntityInteractions(InteractionProgression interactionProgression)
    {
        foreach (EntityInteractionChange entityInteractionUpdate in interactionProgression.entityInteractionUpdates)
        {
            if (entityInteractionUpdate.returnToPreviousInsteadOfNewInteraction 
                && previousInteractionDictionary.TryGetValue(entityInteractionUpdate.entity, out var value))
            {
                Debug.Log($"New interaction is now the previous interaction again for {entityInteractionUpdate.entity.entityName}...");
                UpdateInteraction(entityInteractionUpdate.entity, previousInteractionDictionary[entityInteractionUpdate.entity]);
            }
            else if (entityInteractionUpdate.newInteraction != null)
            {
                Debug.Log($"New interaction is not null for {entityInteractionUpdate.entity.entityName}...");
                UpdateInteraction(entityInteractionUpdate.entity, entityInteractionUpdate.newInteraction);
            }
            else
            {
                Debug.Log($"New interaction is null for {entityInteractionUpdate.entity.entityName}...");
                UpdateInteraction(entityInteractionUpdate.entity, null);
            }


        }
    }

    internal void UpdateInteraction(BaseEntityData entity, SO_Interaction newInteraction)
    {
        InteractionContainer interactionContainer = new InteractionContainer();
        interactionContainer.interaction = newInteraction;

        if (newInteraction == null)
        {
            Debug.Log("null interaction occured, no current interaction assigned for entity...");
        }

        if (previousInteractionDictionary.ContainsKey(entity) && interactionProgressionDictionary.ContainsKey(entity))
        {
            previousInteractionDictionary[entity] = interactionProgressionDictionary[entity];
        }
        else
        {
            previousInteractionDictionary.Add(entity, null);
        }

        if (interactionProgressionDictionary.ContainsKey(entity))
        {

            //interactionContainer.previousInteraction = interactionProgressionDictionary[entity].interaction;
            interactionProgressionDictionary[entity] = newInteraction;
        }
        else
        {
            //interactionContainer.previousInteraction = null;
            interactionProgressionDictionary.Add(entity, newInteraction);
        }

        // Here you can add additional code to handle the interaction update, such as triggering events or saving the state
    }
}
