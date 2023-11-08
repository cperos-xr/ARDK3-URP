using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManaLens : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory essencePouch;
    public bool isActive;
    public RectTransform maskRectTransform;

    public List<SO_EssenceMaterialType> essenceMaterialTypes = new List<SO_EssenceMaterialType>();

    public delegate void PlayerGivenEssenceMaterialEvent(SO_EssenceMaterialType essenceMaterial);
    public static event PlayerGivenEssenceMaterialEvent OnPlayerGivenEssenceMaterial;

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
        if (!isActive) return;


        // Convert the screen point to the local point in the context of the RectTransform
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(maskRectTransform, point, Camera.main, out localPoint))
        {
            if (maskRectTransform.rect.Contains(localPoint))
            {
                Debug.Log("Tap is within the mask area.");
                ReceiveEssenceMaterial(semanticChannelList);
            }
            else
            {
                Debug.Log("Tap is outside the mask area.");
            }
        }
    }

    public void ReceiveEssenceMaterial(List <string> semanticChannelList)
    {
        if (isActive)
        {
            foreach(string semanticChannel in semanticChannelList)
            {
                foreach (SO_EssenceMaterialType essenceMaterialType in essenceMaterialTypes)
                {
                    if (semanticChannel.Equals(essenceMaterialType.essenceMaterialSemanticChannelName))
                    {

                        if(essencePouch.AddItem(essenceMaterialType))
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
}

