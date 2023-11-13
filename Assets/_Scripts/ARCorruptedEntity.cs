using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ARCorruptedEntity : AREntity
{
    SO_CorruptEntity corruptEntity;
    //[SerializeField] private Animator anim;
    // Start is called before the first frame update
    private void OnEnable()
    {
        PurificationManager.OnPlayerPurifiesEntity += ARPurification;
    }

    private void OnDisable()
    {
        PurificationManager.OnPlayerPurifiesEntity -= ARPurification;
    }

    public override void ARInteraction(SO_InteractionAREntity arEntityInteraction)
    {
        //if (arEntityInteraction.arEntity.Equals(arEntityData))
        //{
        //    if (arEntityInteraction.anim != null)
        //    {
        //        anim = arEntityInteraction.anim;
        //        if (!string.IsNullOrEmpty(arEntityInteraction.animationName))
        //        {
        //            anim.Play(arEntityInteraction.animationName);
        //        }
        //    }

        //}
    }

    private void ARPurification(PurificationEntity purificationEntity)
    {
        if (corruptEntity.Equals(purificationEntity.corruptEntity))
        {
            if (setObjectsActiveStatus.Count > 0)
            {
                foreach (ObjectToSetActive obj in setObjectsActiveStatus)
                {
                    obj.objectToSet.SetActive(obj.setStatus);
                }
            }
        }
    }
}
