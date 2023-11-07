using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractionProgression : ScriptableObject
{
    public List<EntityInteractionChange> changes;

    [System.Serializable]
    public struct EntityInteractionChange
    {
        public BaseEntityData entity; // Unique identifier for the entity
        public int newInteractionIndex; // The new interaction index for the entity
    }

    // Method to apply changes to the interaction indices
    public void ApplyChanges(InteractionManager interactionManager)
    {
        foreach (var change in changes)
        {
            interactionManager.UpdateInteractionIndex(change.entity, change.newInteractionIndex);
        }
    }
}
