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
        public bool returnToPreviousInsteadOfNewInteraction; //alternative to utilizing newInteraction


    }

}
