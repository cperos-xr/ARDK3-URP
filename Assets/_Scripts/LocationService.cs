using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocationService : MonoBehaviour
{
    public bool IsLocationServiceEnabled { get; private set; } = false;
    public DV.dv2 CurrentGPS { get; private set; }
    public TextMeshProUGUI coordinateText;

    public static LocationService Instance;

#if UNITY_EDITOR
    public bool useMockCoodinatesIfLocationServisesNotEnabled;
    public DV.dv2 mockCoordinates = new DV.dv2();
    public float movementSpeed;  // Adjust this value to change the speed of coordinate modification

    void HandleEditorInput()
    {

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            mockCoordinates.x += movementSpeed;
        }
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            mockCoordinates.x -= movementSpeed;
        }
        if (Keyboard.current.aKey.isPressed|| Keyboard.current.leftArrowKey.isPressed)
        {
            mockCoordinates.y -= movementSpeed;
        }
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            mockCoordinates.y += movementSpeed;
        }
    }
#endif

    private void Start()
    {
        StartLocationServices();
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

    public void StartLocationServices()
    {
        StartCoroutine(StartService());
    }


    private IEnumerator StartService()
    {
        // Check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled the GPS");
            coordinateText.text = "User has not enabled the GPS";
            yield break;
        }

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20; // You can set this to your preferred timeout value
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in time
        if (maxWait < 1)
        {
            Debug.Log("Location service timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            IsLocationServiceEnabled = true;
        }
    }

    private void Update()
    {
        if (IsLocationServiceEnabled)
        {
            // Fetch current GPS coordinates
            CurrentGPS = FetchCurrentGPS();
            //Debug.Log("Current Gps is " + CurrentGPS);

            string formattedGps = string.Format("Lat: {0:0.########}, Lon: {1:0.########}", CurrentGPS.x, CurrentGPS.y);
            coordinateText.text = formattedGps;

        }
#if UNITY_EDITOR
        else if (useMockCoodinatesIfLocationServisesNotEnabled)
        {
            HandleEditorInput();
            CurrentGPS = new DV.dv2(mockCoordinates.x, mockCoordinates.y);
            //Debug.Log($"Current Gps is ({CurrentGPS.x},{CurrentGPS.y})");
            string formattedGps = string.Format("Lat: {0:0.########}, Lon: {1:0.########}", CurrentGPS.x, CurrentGPS.y);
            coordinateText.text = formattedGps;
        }
#endif
    }

    private DV.dv2 FetchCurrentGPS()
    {

        return new DV.dv2(Input.location.lastData.latitude, Input.location.lastData.longitude);
    }


    private void OnDisable()
    {
        if (IsLocationServiceEnabled)
        {
            Input.location.Stop();
        }
    }
}
