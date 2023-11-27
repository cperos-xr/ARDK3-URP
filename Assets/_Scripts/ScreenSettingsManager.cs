using UnityEngine;

public class ScreenSettingsManager : MonoBehaviour
{
    void Start()
    {
        // Prevent the screen from dimming and turning off
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Lock the screen orientation
        // You can change this to your desired orientation
        Screen.orientation = ScreenOrientation.Portrait; // or Landscape, PortraitUpsideDown, etc.
    }
}
