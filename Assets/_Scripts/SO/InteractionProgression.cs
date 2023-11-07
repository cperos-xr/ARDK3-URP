using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractionProgression : ScriptableObject
{
    public List<EntityInteractionChange> entityInteractionUpdates;

    [System.Serializable]
    public struct EntityInteractionChange
    {
        public BaseEntityData entity; // Unique identifier for the entity
        public SO_Interaction newInteraction; // The new interaction index for the entity
    }


    public void UpdateAllEntityInteractions()
    {
        foreach (EntityInteractionChange entityInteractionUpdate in entityInteractionUpdates) 
        {
            if (entityInteractionUpdate.newInteraction != null)
            {
                Debug.Log($"New interaction is not null for {entityInteractionUpdate.entity.entityName}...");
                InteractionManager.Instance.UpdateInteractionIndex(entityInteractionUpdate.entity, entityInteractionUpdate.newInteraction);
            }
            else
            {
                Debug.Log($"New interaction is null for {entityInteractionUpdate.entity.entityName}...");
                InteractionManager.Instance.UpdateInteractionIndex(entityInteractionUpdate.entity, null);
            }

        
        }
    }
}
