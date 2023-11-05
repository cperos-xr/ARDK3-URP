using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

[CustomEditor(typeof(SO_TaskObjective_TrackedARImage))]
public class SO_TaskObjective_TrackedARImage_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        SO_TaskObjective_TrackedARImage myTarget = (SO_TaskObjective_TrackedARImage)target;

        // Allow the user to select an XRReferenceImageLibrary
        myTarget.imageLibrary = (XRReferenceImageLibrary)EditorGUILayout.ObjectField("Image Library", myTarget.imageLibrary, typeof(XRReferenceImageLibrary), false);

        if (myTarget.imageLibrary != null)
        {
            string[] options = new string[myTarget.imageLibrary.count];
            for (int i = 0; i < myTarget.imageLibrary.count; i++)
            {
                options[i] = myTarget.imageLibrary[i].name;
            }

            // Display a dropdown of the reference images
            myTarget.selectedImageIndex = EditorGUILayout.Popup("Select Image", myTarget.selectedImageIndex, options);

            if (myTarget.selectedImageIndex != -1)
            {
                // Save the selected reference image to the scriptable object
                myTarget.targetReferenceImage = myTarget.imageLibrary[myTarget.selectedImageIndex];
            }
        }

        DrawDefaultInspector();
    }
}
