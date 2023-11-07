using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AREntity : MonoBehaviour
{
    public SO_ArEntityData arEntityData;
    public SO_Interaction currentInteraction;


    private void Start()
    {
        IntitializeArEntity();
    }

    void IntitializeArEntity()
    {
        currentInteraction = arEntityData.startingInteraction;

        //foreach (SO_Interaction arInteraction in arEntityData.interactions)
        //{
        //    ARInteractionObject arInteractionObject;
        //    arInteractionObject.arInteraction = arInteraction;
        //    interactionObjects.Add(arInteractionObject);

        //}
    }

}

[Serializable]
public struct ARInteractionObject
{
    public SO_Interaction arInteraction;
}

