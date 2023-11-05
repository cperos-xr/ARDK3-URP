using UnityEngine;
using UnityEngine.XR.ARSubsystems;

[CreateAssetMenu(fileName = "SO_TaskObjective_TrackedARImage", menuName = "Game/Quest System/Task Objective/TrackedARImage")]
public class SO_TaskObjective_TrackedARImage : SO_TaskObjective
{
    public bool isRetroactive;

    [HideInInspector]
    public XRReferenceImage targetReferenceImage;

    [HideInInspector]
    public int selectedImageIndex = -1;

    [HideInInspector]
    public XRReferenceImageLibrary imageLibrary; // Reference to the XRReferenceImageLibrary


}
