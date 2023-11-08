using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CorruptEntityManager : MonoBehaviour
{
    public static CorruptEntityManager Instance { get; private set; }

    [SerializeField] private List<SO_CorruptEntity> corruptEntities = new List<SO_CorruptEntity>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
        int randomNumber = Random.Range(0, totalWeight);
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