using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class CorruptEntityManager : MonoBehaviour
{
    public static CorruptEntityManager Instance { get; private set; }

    [SerializeField] private List<SO_CorruptEntity> corruptEntities = new List<SO_CorruptEntity>();

    // New structure to hold dictionaries for each SO_CorruptEntity
    private Dictionary<SO_CorruptEntity, EntityData> entitiesData = new Dictionary<SO_CorruptEntity, EntityData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeEntityData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeEntityData()
    {
        foreach (var corruptEntity in corruptEntities)
        {
            // Initialize essence effectiveness dictionary
            var essenceEffectiveness = corruptEntity.essenceEffectivenessList
                .Where(e => e.essenceMaterialType != null)
                .ToDictionary(e => e.essenceMaterialType, e => e.effectiveness);

            // Initialize combination multipliers dictionary
            var combinationMultipliers = corruptEntity.combinationMultipliersList
                .Where(c => c.essenceMaterialType1 != null && c.essenceMaterialType2 != null)
                .ToDictionary(c => Tuple.Create(c.essenceMaterialType1, c.essenceMaterialType2), c => c.multiplier);

            // Add to the entitiesData dictionary
            entitiesData.Add(corruptEntity, new EntityData(essenceEffectiveness, combinationMultipliers));
        }
    }

    // New method to get essence effectiveness
    public float GetEssenceEffectiveness(SO_CorruptEntity entity, SO_EssenceMaterialType essenceType)
    {
        if (entitiesData.TryGetValue(entity, out EntityData data))
        {
            if (data.EssenceEffectiveness.TryGetValue(essenceType, out float effectiveness))
            {
                return effectiveness;
            }
        }
        return 0f; // Default or error value
    }

    // New method to get combination multipliers
    public float GetCombinationMultiplier(SO_CorruptEntity entity, SO_EssenceMaterialType type1, SO_EssenceMaterialType type2)
    {
        if (entitiesData.TryGetValue(entity, out EntityData data))
        {
            var comboKey = Tuple.Create(type1, type2);
            if (data.CombinationMultipliers.TryGetValue(comboKey, out float multiplier))
            {
                return multiplier;
            }
        }
        return 1f; // Default or error value
    }

    // New helper struct to store entity data
    private struct EntityData
    {
        public Dictionary<SO_EssenceMaterialType, float> EssenceEffectiveness;
        public Dictionary<Tuple<SO_EssenceMaterialType, SO_EssenceMaterialType>, float> CombinationMultipliers;

        public EntityData(Dictionary<SO_EssenceMaterialType, float> essenceEffectiveness, Dictionary<Tuple<SO_EssenceMaterialType, SO_EssenceMaterialType>, float> combinationMultipliers)
        {
            EssenceEffectiveness = essenceEffectiveness;
            CombinationMultipliers = combinationMultipliers;
        }
    }

    // Method to get all entities that can spawn in a given list of areas
    public List<SO_CorruptEntity> GetEntitiesByAreas(List<SO_AreaData> areas)
    {
        return corruptEntities.Where(entity => entity.spawnableAreas.Any(area => areas.Contains(area.areaData))).ToList();
    }

    // Method to select an entity based on essence type and scarcity
    public SO_CorruptEntity GetEntityByEssenceAndScarcity(SO_EssenceMaterialType essenceType, List<SO_AreaData> areas)
    {
        List<SO_CorruptEntity> possibleEntities = GetEntitiesByAreas(areas)
            .Where(entity => entity.essenceMaterialType == essenceType)
            .ToList();

        // Define weights for each rarity level
        Dictionary<Scarcity.Level, int> rarityWeights = new Dictionary<Scarcity.Level, int>
    {
        { Scarcity.Level.Abundant, 1000 },
        { Scarcity.Level.Plentiful, 500 },
        { Scarcity.Level.Common, 250 },
        { Scarcity.Level.Uncommon, 125 },
        { Scarcity.Level.Scarce, 60 },
        { Scarcity.Level.Rare, 30 },
        { Scarcity.Level.VeryRare, 15 },
        { Scarcity.Level.UltraRare, 7 },
        { Scarcity.Level.Legendary, 3 },
        { Scarcity.Level.Unique, 1 }
    };

        // Calculate the total weight
        int totalWeight = possibleEntities
            .Sum(entity => rarityWeights[entity.spawnableAreas.Min(area => area.scarcityLevel.ScarcityLevel)]);

        // Generate a random number between 0 and the total weight
        int randomNumber = UnityEngine.Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        // Iterate through the entities and select one based on the random number
        foreach (var entity in possibleEntities)
        {
            int entityWeight = rarityWeights[entity.spawnableAreas.Min(area => area.scarcityLevel.ScarcityLevel)];
            cumulativeWeight += entityWeight;

            if (randomNumber <= cumulativeWeight)
            {
                return entity;
            }
        }

        // Fallback in case no entity is selected (should not happen, but it's good to have a fallback)
        return null;
    }

}