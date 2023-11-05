#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LayerSetter))]
public class LayerSetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Draws default inspector

        LayerSetter layerSetter = (LayerSetter)target;

        string[] layers = UnityEditorInternal.InternalEditorUtility.layers;
        int layerIndex = 0;
        for (int i = 0; i < layers.Length; i++)
        {
            if (layerSetter.targetLayer == LayerMask.NameToLayer(layers[i]))
            {
                layerIndex = i;
                break;
            }
        }

        layerIndex = EditorGUILayout.Popup("Target Layer", layerIndex, layers);
        layerSetter.targetLayer = LayerMask.NameToLayer(layers[layerIndex]);

        if (GUILayout.Button("Set Layer On All Children"))
        {
            layerSetter.SetLayerRecursive();
        }
    }
}
#endif
