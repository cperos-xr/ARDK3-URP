using System;
using System.Collections.Generic;
using UnityEngine;

public class AREntity : MonoBehaviour
{
    public SO_ArEntityData arEntityData;
    [SerializeField] private Animator anim;
    [HideInInspector] public SO_Interaction currentInteraction;

    public List<ObjectToSetActive> setObjectsActiveStatus = new List<ObjectToSetActive>();


    private void OnEnable()
    {
        InteractionManager.OnPlayerAREntityInteraction += ARInteraction;
    }
    private void OnDisable()
    {
        InteractionManager.OnPlayerAREntityInteraction -= ARInteraction;
    }
    private void ARInteraction(SO_InteractionAREntity arEntityInteraction)
    {
        if (arEntityInteraction.arEntity.Equals(arEntityData))
        {
            if (arEntityInteraction.anim != null)
            {
                anim = arEntityInteraction.anim;
                if (!string.IsNullOrEmpty(arEntityInteraction.animationName))
                {
                    anim.Play(arEntityInteraction.animationName);
                }
            }

            if (setObjectsActiveStatus.Count > 0)
            {
                foreach (ObjectToSetActive obj in setObjectsActiveStatus)
                {
                    obj.objectToSet.SetActive(obj.setStatus);
                }
            }
        }
    }


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
public struct ObjectToSetActive
{
    public GameObject objectToSet;
    public bool setStatus;
}

[Serializable]
public struct ARInteractionObject
{
    public SO_Interaction arInteraction;
}

