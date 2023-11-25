using TMPro;
using UnityEngine;

public class ScreenTapRaycaster : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera
    private LayerMask layerMask; // LayerMask for raycasting

    public TextMeshProUGUI hitTextHit;
    public TextMeshProUGUI hitTextCenter;

    [SerializeField] InteractionManager interactionManager;

    private void Start()
    {
        // Get the reference to the main camera
        mainCamera = Camera.main;

        // Set up the layer mask to exclude the "Map Objects" layer
        layerMask = 1 << LayerMask.NameToLayer("Map Objects");
        layerMask = ~layerMask; // Invert the mask to exclude the specified layer
    }

    private void Update()
    {
        // Check for a screen tap on mobile
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) //liun 27
        {
            Vector2 touchPosition = Input.GetTouch(0).position;
            PerformRaycastFromTap(touchPosition);  // line 30
        }

    }

    public void PerformRaycastFromTap(Vector2 tapPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(tapPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            GameObject hitObject = hit.collider.gameObject;
            // ...

            AREntity arEntity = hitObject.GetComponent<AREntity>();
            if (arEntity && PlayerManager.Instance.currentPlayerState == PlayerState.normal)
            {
                SO_ArEntityData sO_ArEntityData = arEntity.arEntityData;
                if (interactionManager != null)
                {
                    interactionManager.HandleEntityInteraction(sO_ArEntityData);
                }
                else
                {
                    Debug.LogError("interactionManager is null.");
                }
            }
        }
        else
        {
            Debug.Log("No object was hit (from tap).");
            hitTextHit.text = "No Hit";
        }
    }
}
