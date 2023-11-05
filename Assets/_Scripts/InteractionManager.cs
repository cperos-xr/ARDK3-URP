using System.Collections.Generic;
using System;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] ItemManager itemManager;

    public delegate void PlayerInteractionEvent(Interaction EntityNotifyPlayer);
    public static event PlayerInteractionEvent OnPlayerInteraction;

    public delegate void PlayerEntityInteractionEvent(BaseEntityData entity);
    public static event PlayerEntityInteractionEvent OnPlayerEntityInteraction;

    private Dictionary<BaseEntityData, Dictionary<Interaction, int>> entityInteractionCounts = new Dictionary<BaseEntityData, Dictionary<Interaction, int>>();

    //[SerializeField] QuestManager questManager;


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
            entityInteractionCounts[entity] = new Dictionary<Interaction, int>();
        }

        Interaction interaction = entity.interactions[entity.interactionIndex];

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




}


[Serializable]
public struct Interaction
{
    public string name;
    public int maxInteractions;
    public List<SO_ItemData> itemDatas;
    public SO_Quest quest; // List of tasks associated with this interaction.
    public PlayerNotification notification;
    public bool interactionIsLocked;

}