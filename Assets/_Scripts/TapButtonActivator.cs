using UnityEngine;
using System.Collections.Generic;

public class TapButtonActivator : MonoBehaviour
{
    public delegate void PlayerDebugModeEvent();
    public static event PlayerDebugModeEvent OnPlayerEnterDebugMode;

    public List<GameObject> gameObjectsToActivate; // Assign in Inspector
    public float activationTimeWindow = 3f; // Time window for 5 taps

    private int tapCount = 0;
    private float timeSinceFirstTap = 0f;

    void Update()
    {
        // Update the timer if it's started
        if (tapCount > 0)
        {
            timeSinceFirstTap += Time.deltaTime;

            // Reset if time window has elapsed
            if (timeSinceFirstTap > activationTimeWindow)
            {
                ResetTapping();
            }
        }
    }

    public void OnButtonTap()
    {
        // Increment tap count and start the timer if it's the first tap
        if (tapCount == 0)
        {
            timeSinceFirstTap = 0f;
        }
        tapCount++;

        // Check if tapped 5 times within the time window
        if (tapCount >= 5)
        {
            ActivateGameObjects();
            OnPlayerEnterDebugMode?.Invoke();
            ResetTapping();
        }
    }

    private void ActivateGameObjects()
    {
        foreach (GameObject obj in gameObjectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    private void ResetTapping()
    {
        tapCount = 0;
        timeSinceFirstTap = 0f;
    }
}
