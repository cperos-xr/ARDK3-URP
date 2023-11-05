
using Niantic.Lightship.Maps.Core.Coordinates;
using Niantic.Lightship.Maps.Samples.CustomMapLayers.StateBoundaries;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    //[SerializeField] private PlayerManager playerManager;
    //[SerializeField] private AreaManager areaManager;
    [SerializeField] private Transform areaTransform;

    //[SerializeField] private LocationService locationService;

    public delegate void PlayerPositionChangedEvent(LatLng newPosition);
    public static event PlayerPositionChangedEvent OnPlayerPositionChanged;

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

    private void Start()
    {
        foreach (SO_AreaData areaData in AreaManager.Instance.allAreas)
        {
            // Create a new game object
            GameObject areaObject = new GameObject(areaData.name);
            areaObject.transform.SetParent(areaTransform, false);

            // Attach the Geospatial Creator Anchor script to the game object
            foreach (Boundary boundary in areaData._boundaries)
            {
                foreach (LatLng gps in boundary.Points)
                {
                    GameObject boundaryPoint = new GameObject("BoundaryPoint"); // Optionally, provide a name

                    boundaryPoint.transform.SetParent(areaObject.transform);

                }

            }

        }
    }

    private void Update()
    {
        DV.dv2 oldPosition = new DV.dv2(PlayerManager.Instance.PlayerGPS.Latitude, PlayerManager.Instance.PlayerGPS.Longitude);
        PlayerManager.Instance.PlayerGPS = new
            (LocationService.Instance.CurrentGPS.x, LocationService.Instance.CurrentGPS.y);


        if (oldPosition.x != PlayerManager.Instance.PlayerGPS.Latitude || oldPosition.y != PlayerManager.Instance.PlayerGPS.Longitude)
        {
            // Player position has changed, broadcast the event
            OnPlayerPositionChanged?.Invoke(PlayerManager.Instance.PlayerGPS);
            Debug.Log("Player position has changed");
        }

    }


    public void PlaceAllMarkers()
    {
   
        foreach (SO_AreaData areaData in AreaManager.Instance.allAreas)
        {
            Debug.Log("Loading area " + areaData.name);
            AreaManager.Instance.InitializeArea(areaData);

        }

    }
}
