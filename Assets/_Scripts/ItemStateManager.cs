using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStateManager : MonoBehaviour
{
    private Dictionary<string, HashSet<string>> givenItems = new Dictionary<string, HashSet<string>>();

    public void MarkItemAsGiven(BaseEntityData entity, SO_ItemData item)
    {
        string entityId = entity.name;
        string itemId = item.itemName;  // Assuming SO_ItemData has an itemName property

        if (!givenItems.ContainsKey(entityId))
        {
            givenItems[entityId] = new HashSet<string>();
        }

        givenItems[entityId].Add(itemId);
    }

    public bool HasEntityGivenItem(BaseEntityData entity, SO_ItemData item)
    {
        string entityId = entity.name;
        string itemId = item.itemName;  // Assuming SO_ItemData has an itemName property

        if (givenItems.TryGetValue(entityId, out var itemsGiven))
        {
            return itemsGiven.Contains(itemId);
        }

        return false;
    }
}


