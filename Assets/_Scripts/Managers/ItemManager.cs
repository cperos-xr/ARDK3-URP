using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    [SerializeField] public List<SO_MapEntityData> currentInteractiveEntities = new List<SO_MapEntityData>();
    [SerializeField] private ItemStateManager itemStateManager;

    public delegate void PlayerGivenItemEvent(SO_ItemData item);
    public static event PlayerGivenItemEvent OnPlayerGivenItem;




    private void OnEnable()
    {
        InteractionManager.OnPlayerReceiveItem += AddItemToPlayerInventory;
    }

    private void OnDisable()
    {
        InteractionManager.OnPlayerReceiveItem -= AddItemToPlayerInventory;
    }

    public void AddItemToPlayerInventory(SO_ItemData itemData, BaseEntityData entity)
    {

        // Set which entity gives this item
        itemData.givenByEntityName = entity.entityName;

        // Give the item to the player
        PlayerManager.Instance.inventory.AddItem(itemData);

        OnPlayerGivenItem?.Invoke(itemData);

        Debug.Log($"Adding Item {itemData.itemName} to player inventory");

        //updating any interactions that may have changed
        InteractionManager.Instance.UpdateAllEntityInteractions(itemData);

        QuestUIHandler.Instance.CheckQuests();


    }



}

