using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "New Corrupt Entity", menuName = "Game/Level/Entity/Corrupt Entity")]
public class SO_CorruptEntity : BaseEntityData
{
    public string corruptEntityName;
    public string corruptEntityDescription;

    public string funFact;

    public SO_EssenceMaterialType essenceMaterialType; // The type of essence associated with the corrupt entity
    public List<SpawnableArea> spawnableAreas; // Areas where the entity can appear

    public List<ItemDrop> ItemsDroppedUponPurification;
    public SO_Interaction InteractionToCompleteUponPurification;

    public List<EssenceEffectiveness> essenceEffectivenessList;
    public List<CombinationMultiplier> combinationMultipliersList;

    public Sprite corruptedStateSprite;
    public Sprite healedStateSprite;

    public Vector2 corruptionLevelRangeMinMax;   // The min and maximum corruption level (e.g., when the entity is fully corrupted).
    public GameObject model;



}


[System.Serializable]
public struct ItemDrop
{
    public SO_ItemData item;
    public float dropRate; // 1 = 100%, .5 = 50%... 
}


[System.Serializable]
public struct SpawnableArea
{
    public SO_AreaData areaData;
    public Scarcity scarcityLevel;
}

[System.Serializable]
public struct Scarcity
{
    public enum Level
    {
        Abundant,    // 0
        Plentiful,   // 1
        Common,      // 2
        Uncommon,    // 3
        Scarce,      // 4
        Rare,        // 5
        VeryRare,    // 6
        UltraRare,   // 7
        Legendary,   // 8
        Unique       // 9
    }

    public Level ScarcityLevel;
}

[System.Serializable]
public struct EssenceEffectiveness
{
    public SO_EssenceMaterialType essenceMaterialType;
    public float effectiveness;
}

[System.Serializable]
public struct CombinationMultiplier
{
    public SO_EssenceMaterialType essenceMaterialType1;
    public SO_EssenceMaterialType essenceMaterialType2;
    public float multiplier;
}

