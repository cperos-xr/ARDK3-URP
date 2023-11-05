using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ItemManager : MonoBehaviour
{
    [SerializeField] public List<SO_MapEntityData> currentInteractiveEntities = new List<SO_MapEntityData>();
    //[SerializeField] public PlayerManager playerManager;
    [SerializeField] private ItemStateManager itemStateManager;

    public delegate void PlayerGivenItemEvent(SO_ItemData item);
    public static event PlayerGivenItemEvent OnPlayerGivenItem;

    public void LoadEntityItems(SO_MapEntityData entityData)
    {
        if (!currentInteractiveEntities.Contains(entityData))
        {
            currentInteractiveEntities.Add(entityData);
        }

        foreach (SO_ItemData itemData in entityData.associatedItems)
        {
            if (!itemData.isLocked)
            {
                if (itemData.isDuplicable || !itemStateManager.HasEntityGivenItem(entityData, itemData))
                {
                    AddItemToPlayerInventory(itemData, entityData);
                }
            }
        }
    }

    public void RemoveEntity(SO_MapEntityData entity)
    {
        if (currentInteractiveEntities.Contains(entity))
        {
            currentInteractiveEntities.Remove(entity);
        }
    }

    public void AddItemToPlayerInventory(SO_ItemData itemData, BaseEntityData entity)
    {

        // Set which entity gives this item
        itemData.givenByEntityName = entity.entityName;

        // Give the item to the player
        PlayerManager.Instance.inventory.AddItem(itemData);

        OnPlayerGivenItem?.Invoke(itemData);

        Debug.Log($"Adding Item {itemData.itemName} to player inventory");


    }



}

