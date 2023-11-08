using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManaLens : MonoBehaviour
{
    [Serializable]
    public enum ELensState
    {
        inactive,
        receiveingEssenceMaterials,
        seekingCorruptSpirits
    }
    // Start is called before the first frame update
    public Inventory essencePouch;
    public bool isActive;
    public int lowerScreenLimit;  //Acts as bounds so doesnt work on menu taps.

    public List<SO_EssenceMaterialType> essenceMaterialTypes = new List<SO_EssenceMaterialType>();

    public delegate void PlayerGivenEssenceMaterialEvent(SO_EssenceMaterialType essenceMaterial);
    public static event PlayerGivenEssenceMaterialEvent OnPlayerGivenEssenceMaterial;

    public ELensState lensState = ELensState.inactive;

    private void OnEnable()
    {
        SemanticChannelDetector.OnSemanticChannelIdentified += ReceiveEssenceMaterial;
    }

    private void OnDisable()
    {
        SemanticChannelDetector.OnSemanticChannelIdentified -= ReceiveEssenceMaterial;
    }

    //public void ReceiveEssenceMaterial(List<string> semanticChannelList, Vector2 point)
    //{
    //    if (RectTransformUtility.RectangleContainsScreenPoint(maskRectTransform, point, Camera.main))
    //    {
    //        Debug.Log("Tap is within the mask area.");
    //        // Handle valid tap
    //        ReceiveEssenceMaterial(semanticChannelList);
    //    }
    //}

    public void ReceiveEssenceMaterial(List<string> semanticChannelList, Vector2 point)
    {
        if (point.y > lowerScreenLimit)
        {
            switch(lensState)
            {
                case ELensState.inactive:
                    break;
                case ELensState.receiveingEssenceMaterials:
                    ReceiveEssenceMaterial(semanticChannelList);
                    break;
                case ELensState.seekingCorruptSpirits:
                    SeekCorruptSpirts(semanticChannelList);
                    break;
            }


            
        }
    }

    private void SeekCorruptSpirts(List<string> semanticChannelList)
    {

    }


    public void ReceiveEssenceMaterial(List <string> semanticChannelList)
    {
        foreach (string semanticChannel in semanticChannelList)
        {
            foreach (SO_EssenceMaterialType essenceMaterialType in essenceMaterialTypes)
            {
                if (semanticChannel.Equals(essenceMaterialType.essenceMaterialSemanticChannelName))
                {

                    if (essencePouch.AddItem(essenceMaterialType))
                    {
                        Debug.Log($"Successfully added {essenceMaterialType.itemName} to the essence pouch");
                        OnPlayerGivenEssenceMaterial?.Invoke(essenceMaterialType);
                    }
                    else
                    {
                        // Pouch is full
                        Debug.Log($"Could not add {essenceMaterialType.itemName} to the essence pouch");
                    }
                }
            }
        }
    }
}

