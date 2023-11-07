using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_InteractionProgressive : ScriptableObject
{
    public InteractionProgression interactionProgression;

    public void ApplyInteractionProgression()
    {
        if (interactionProgression != null)
        {
            interactionProgression.ApplyChanges(InteractionManager.Instance);
        }
    }
}
