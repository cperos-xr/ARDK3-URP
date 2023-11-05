using UnityEngine;

public class LayerSetter : MonoBehaviour
{
    [SerializeField] private GameObject _parentObject; 
    public LayerMask targetLayer;

    public void SetLayerRecursive()
    {
        if (_parentObject == null)
        {
            Debug.LogError("LightshipMap GameObject is not set!");
            return;
        }

        SetLayerOnAllChildren(_parentObject, targetLayer);
    }

    private void SetLayerOnAllChildren(GameObject obj, LayerMask layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerOnAllChildren(child.gameObject, layer);
        }
    }
}
