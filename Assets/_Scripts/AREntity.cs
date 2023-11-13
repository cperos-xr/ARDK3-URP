using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AREntity : MonoBehaviour
{
    public SO_ArEntityData arEntityData;
    [HideInInspector] public SO_Interaction currentInteraction;


    private void Start()
    {
        IntitializeArEntity();
    }

    void IntitializeArEntity()
    {
        currentInteraction = arEntityData.startingInteraction;

    }

}

[Serializable]
public struct ARInteractionObject
{
    public SO_Interaction arInteraction;
}

