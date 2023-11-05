using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageTrack : MonoBehaviour
{
    public static ARImageTrack Instance;

    [SerializeField] private ARTrackedImageManager _arTrackedImageManager;

    // Create a list to store encountered reference images.
    private List<XRReferenceImage> allEncounteredReferenceImages = new List<XRReferenceImage>();

    private void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void Awake()
    {
        // Ensure there is only one instance of QuestManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (UnityEngine.XR.ARFoundation.ARTrackedImage aRtrackedImage in args.added)
        {
            XRReferenceImage referenceImage = aRtrackedImage.referenceImage;

            if (!allEncounteredReferenceImages.Contains(referenceImage))
            {
                Debug.Log($"*** Tracking {referenceImage.name} ***");

                // Add the encountered reference image to the list.
                allEncounteredReferenceImages.Add(referenceImage);
                // You can also implement your completion logic here
                // and check if the current tracked image matches the objective.
                // If it matches, mark the objective as completed.

                TaskObjectiveHandler.Instance.CheckARTrackedImageObjectivesForCompletion(referenceImage);

            }
        }
    }


    // Create a method to check if a reference image has been encountered.
    public bool HasEncounteredReferenceImage(XRReferenceImage referenceImage)
    {
        return allEncounteredReferenceImages.Contains(referenceImage);
    }
}
