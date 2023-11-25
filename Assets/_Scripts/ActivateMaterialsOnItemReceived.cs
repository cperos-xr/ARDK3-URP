using System.Collections.Generic;
using UnityEngine;

public class ActivateMaterialsOnItemReceived : MonoBehaviour
{
    [SerializeField] private SO_ItemData itemReceived;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] private Material setMeshRendererMaterial;

    private void OnEnable()
    {
        InteractionManager.OnPlayerReceiveItem += ActivateObjectUponItemReceived;
    }

    private void OnDisable()
    {
        InteractionManager.OnPlayerReceiveItem -= ActivateObjectUponItemReceived;
    }

    private void ActivateObjectUponItemReceived(SO_ItemData itemData, BaseEntityData entityData)
    {
        if (itemData.Equals(itemReceived)) 
        {
            meshRenderer.material = setMeshRendererMaterial;
        }
    }
}
