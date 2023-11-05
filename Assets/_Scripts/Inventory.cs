using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<ItemSlot> items = new List<ItemSlot>();

    public void AddItem(SO_ItemData item, int amount = 1)
    {
        // Add item logic
        var itemSlot = items.Find(i => i.item == item);
        if (itemSlot != null)
        {
            itemSlot.amount += amount;
        }
        else
        {
            items.Add(new ItemSlot(item, amount));
        }
    }

    public void RemoveItem(SO_ItemData item)
    {
        // Remove item logic
        var itemSlot = items.Find(i => i.item == item);
        if (itemSlot != null)
        {
            itemSlot.amount--;
            if (itemSlot.amount <= 0)
                items.Remove(itemSlot);
        }
    }

    public int CheckForItem(SO_ItemData item)
    {
        var itemSlot = items.Find(i => i.item == item);
        if (itemSlot != null)
        {
            return itemSlot.amount;
        }
        else
        {
            return 0; // Item not found in the inventory.
        }
    }

}

[System.Serializable]
public class ItemSlot
{
    public SO_ItemData item;
    public int amount;

    public ItemSlot(SO_ItemData item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

}
