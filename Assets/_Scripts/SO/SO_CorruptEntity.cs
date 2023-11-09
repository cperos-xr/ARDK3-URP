using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "New Corrupt Entity", menuName = "Game/Level/Entity/Corrupt Entity")]
public class SO_CorruptEntity : ScriptableObject
{
    
    public string corruptEntityName;
    public SO_EssenceMaterialType essenceMaterialType; // The type of essence associated with the corrupt entity
    public List<SpawnableArea> spawnableAreas; // Areas where the entity can appear


    public List<EssenceEffectiveness> essenceEffectivenessList;
    public List<CombinationMultiplier> combinationMultipliersList;

    public Vector2 corruptionLevelRangeMinMax;   // The min and maximum corruption level (e.g., when the entity is fully corrupted).

    private Dictionary<SO_EssenceMaterialType, float> essenceEffectiveness;
    private Dictionary<Tuple<SO_EssenceMaterialType, SO_EssenceMaterialType>, float> combinationMultipliers;

    public PlayerNotification playerNotification;

    public void OnEnable()
    {
        InitializeDictionaries();
    }

    private void InitializeDictionaries()
    {
        essenceEffectiveness = essenceEffectivenessList.ToDictionary(e => e.essenceMaterialType, e => e.effectiveness);

        combinationMultipliers = combinationMultipliersList.ToDictionary(
            c => Tuple.Create(c.essenceMaterialType1, c.essenceMaterialType2),
            c => c.multiplier
        );
    }



    // You can add more properties related to the corrupt entity here
    // For example, health, damage, movement patterns, etc.
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
    public string Description;

    public Scarcity(Level level)
    {
        ScarcityLevel = level;
        Description = level switch
        {
            Level.Abundant => "Commonly found everywhere with high availability.",
            Level.Plentiful => "Easily found in many areas, but not as ubiquitous as abundant resources.",
            Level.Common => "Regularly encountered, no special effort needed to locate.",
            Level.Uncommon => "Less frequently encountered, may require some effort to locate.",
            Level.Scarce => "Not readily available, found in specific areas or under certain conditions.",
            Level.Rare => "Infrequently found, often sought after for their value or utility.",
            Level.VeryRare => "Seldom encountered, highly valued for their scarcity.",
            Level.UltraRare => "Extremely rare, often unique or with very limited sources.",
            Level.Legendary => "Mythically rare, might be one of a kind or limited to a handful of instances.",
            Level.Unique => "One-of-a-kind item or resource, with no other known examples.",
            _ => "Unknown"
        };
    }
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

