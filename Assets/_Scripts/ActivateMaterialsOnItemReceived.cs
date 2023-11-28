using UnityEngine;

public class ActivateMaterialsOnItemReceived : MonoBehaviour
{
    [SerializeField] private SO_ItemData itemReceived;
    [SerializeField] private Material MaterialToMakeVisible;
    [SerializeField] private Color originalMaterialColor;

    private void OnEnable()
    {
        InteractionManager.OnPlayerReceiveItem += ActivateObjectUponItemReceived;
    }

    private void OnDisable()
    {
        InteractionManager.OnPlayerReceiveItem -= ActivateObjectUponItemReceived;
    }

    private void Start()
    {
        MaterialToMakeVisible.color = Color.clear;
    }

    private void ActivateObjectUponItemReceived(SO_ItemData itemData, BaseEntityData entityData)
    {
        if (itemData.Equals(itemReceived)) 
        {
            MaterialToMakeVisible.color = originalMaterialColor;
        }
    }
}
