using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler: MonoBehaviour
{
    [SerializeField] private GameObject inventoryItemButtonTemplate;
    [SerializeField] private GameObject scrollBarInventoryContent;

    //[SerializeField] private List<InventoryButton> InventoryItems;

    private void OnEnable()
    {
        ItemManager.OnPlayerGivenItem += MakeNewInventoryItemButton;
        ManaLens.OnPlayerGivenEssenceMaterial += MakeNewInventoryItemButton;
        PurificationManager.OnPlayerAddsEssenceBackToPouch += MakeNewInventoryItemButton;
    }

    private void OnDisable()
    {
        ItemManager.OnPlayerGivenItem -= MakeNewInventoryItemButton;
        ManaLens.OnPlayerGivenEssenceMaterial -= MakeNewInventoryItemButton;
        PurificationManager.OnPlayerAddsEssenceBackToPouch -= MakeNewInventoryItemButton;
    }

    public void MakeNewInventoryItemButton(SO_ItemData itemData)
    {
        GameObject newItemButton = Instantiate(inventoryItemButtonTemplate, scrollBarInventoryContent.transform);
        InventoryButton inventoryButton = newItemButton.GetComponent<InventoryButton>();
        inventoryButton.InitialIzeItemButton(itemData);
        //InventoryItems.Add(inventoryButton);
        if (itemData is SO_EssenceMaterialType essenceMaterialType)
        {
            EssenceMaterialTypeContainer essenceMaterialTypeContainer = newItemButton.AddComponent<EssenceMaterialTypeContainer>();
            essenceMaterialTypeContainer.essenceMaterialType = essenceMaterialType;
            Button button = newItemButton.GetComponent<Button>();
            button.onClick.AddListener(essenceMaterialTypeContainer.SelectEssenceMaterialType);
            button.onClick.AddListener(() => Destroy(newItemButton));

        }
        newItemButton.SetActive(true);
        Debug.Log("Created inventory item", newItemButton);
    }

}
