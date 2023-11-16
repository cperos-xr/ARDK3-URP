using System;
using System.Collections.Generic;
using UnityEngine;

public class AREntity : MonoBehaviour
{
    public SO_ArEntityData arEntityData;
    [SerializeField] private Animator anim;
    [HideInInspector] public SO_Interaction currentInteraction;

    public List<ObjectToSetActiveAndActivationStatus> setObjectsActiveStatus = new List<ObjectToSetActiveAndActivationStatus>();

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

            if (setObjectsActiveStatus.Count > 0)
            {
                foreach (ObjectToSetActiveAndActivationStatus obj in setObjectsActiveStatus)
                {
                    if (obj.uponThisInteractionProgression.Equals(arEntityInteraction))
                    {
                        obj.objectToSet.SetActive(obj.setStatus);
                    }

                    foreach(AnimatorAnimationPair animatorAnimationPair in obj.animationEvents)
                    {
                        animatorAnimationPair.anim.Play(animatorAnimationPair.animationName);
                    }

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
public struct ObjectToSetActiveAndActivationStatus
{
    public GameObject objectToSet;
    //public Collider objectCollider;
    public InteractionProgression uponThisInteractionProgression;
    public bool setStatus;

    public List<AnimatorAnimationPair> animationEvents;

}

[Serializable]
public struct AnimatorAnimationPair
{
    public Animator anim;
    public string animationName;
}

[Serializable]
public struct ARInteractionObject
{
    public SO_Interaction arInteraction;
}

