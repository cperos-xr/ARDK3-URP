using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractionProgression : ScriptableObject
{
    public List<EntityInteractionChange> interactionModifications;

    [System.Serializable]
    public struct EntityInteractionChange
    {
        public BaseEntityData entity; // Unique identifier for the entity
        public SO_Interaction newInteraction; // The new interaction index for the entity
    }

    // Method to apply changes to the interaction indices
    public void UpdateAllAssociatedEntityInteractions()
    {
        Debug.Log("Updating All Associated Entity Interactions...");
        foreach (var change in interactionModifications)
        {
            Debug.Log($"Updating Entity Interaction for {change.entity.entityName} and changing current interaction to {change.newInteraction.InteractionName}");
            InteractionManager.Instance.UpdateInteraction(change.entity, change.newInteraction);
        }
    }
}
