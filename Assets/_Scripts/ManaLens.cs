using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaLens : MonoBehaviour
{
    // Start is called before the first frame update
    Inventory essencePouch;
    public bool isActive;

    public List<SO_EssenceMaterialType> essenceMaterialTypes = new List<SO_EssenceMaterialType>();



    private void OnEnable()
    {
        SemanticChannelDetector.OnSemanticChannelIdentified += ReceiveEssenceMaterial;
    }

    private void OnDisable()
    {
        SemanticChannelDetector.OnSemanticChannelIdentified -= ReceiveEssenceMaterial;
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
