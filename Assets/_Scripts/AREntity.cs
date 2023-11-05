using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AREntity : MonoBehaviour
{
    public SO_ArEntityData arEntityData;
    public List<ARInteractionObject> interactionObjects;
    public int currentInteraction;


    private void Start()
    {
        IntitializeArEntity();
    }

    void IntitializeArEntity()
    {
        foreach (Interaction arInteraction in arEntityData.interactions)
        {
            ARInteractionObject arInteractionObject;
            arInteractionObject.arInteraction = arInteraction;
            arInteractionObject.interactionIsLocked = arInteraction.interactionIsLocked;
            interactionObjects.Add(arInteractionObject);

        }
    }

}

[Serializable]
public struct ARInteractionObject
{
    public Interaction arInteraction;
    public bool interactionIsLocked;
}

