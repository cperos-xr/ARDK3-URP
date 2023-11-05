using UnityEngine;

[ExecuteAlways] // So the change can be previewed in the editor itself, if desired.
public class LayerOnEnable : MonoBehaviour
{
    [SerializeField]
    private LayerMask targetLayer; // This will show a dropdown of layers in the inspector.

    private void OnEnable()
    {
        // Set the layer of the GameObject.
        gameObject.layer = GetSelectedLayer(targetLayer);
    }

    // Helper function to extract the selected layer from LayerMask.
    private int GetSelectedLayer(LayerMask mask)
    {
        int layerNumber = 0;
        int layer = mask.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber - 1;
    }
}
