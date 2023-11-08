using UnityEngine;

[CreateAssetMenu(fileName = "New Essence Material Type", menuName = "Game/Inventory/Essence Material Type", order = 1)]
public class SO_EssenceMaterialType : SO_ItemData
{
    public string essenceMaterialSemanticChannelName; // The type of essence, matching with semantic segmentation channels
    public Color essenceMaterialColor;
    // You can add more essence-specific properties here
}
