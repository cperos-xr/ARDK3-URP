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

    public Dictionary<BaseEntityData, int> interactionProgressionDictionary = new Dictionary<BaseEntityData, int>();

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

        Debug.Log("Handling Entity Interaction" + entity.entityName);
        OnPlayerEntityInteraction?.Invoke(entity);
        if (!entityInteractionCounts.ContainsKey(entity))
        {
            entityInteractionCounts[entity] = new Dictionary<SO_Interaction, int>();
        }

        SO_Interaction interaction = entity.interactions[entity.startingInteractionIndex];

        if (!entityInteractionCounts[entity].ContainsKey(interaction))
        {
            entityInteractionCounts[entity][interaction] = 0;
        }

        // Check if this interaction is related to tasks.
        bool hasQuest = interaction.quest != null;

        // Check if this interaction provides items.
        bool hasItems = interaction.itemDatas != null && interaction.itemDatas.Count > 0;

        if (entityInteractionCounts[entity][interaction] < interaction.maxInteractions)
        {
            OnPlayerInteraction?.Invoke(interaction);
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

            entityInteractionCounts[entity][interaction]++; // Increment the interaction count for this interaction.
        }
        else
        {
            // Interaction limit reached, handle it (e.g., display a message).
            Debug.Log("Interaction limit reached.");
        }
    }

    internal void UpdateInteractionIndex(BaseEntityData entity, int newInteractionIndex)
    {
        if (interactionProgressionDictionary.ContainsKey(entity))
        {
            interactionProgressionDictionary[entity] = newInteractionIndex;
        }
        else
        {
            interactionProgressionDictionary.Add(entity, newInteractionIndex);
        }

        // Here you can add additional code to handle the interaction update, such as triggering events or saving the state
    }
}
