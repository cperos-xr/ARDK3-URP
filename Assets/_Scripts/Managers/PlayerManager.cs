using Niantic.Lightship.Maps.Core.Coordinates;
using UnityEngine;

public enum PlayerState
{
    normal,
    purification,
    notification,
    menuOpen,
}
public class PlayerManager : MonoBehaviour
{
    static public PlayerManager Instance;

    public SO_PlayerData playerData; // Reference to the PlayerData SO
    public Inventory inventory;

    [HideInInspector] public LatLng PlayerGPS;

    public PlayerState currentPlayerState = PlayerState.normal;

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

    public void UseItem(SO_ItemData item)
    {
        // Use item logic
        inventory.RemoveItem(item);
    }

    public void SetPlayerBackToNormal()
    {
        currentPlayerState = PlayerState.normal;
    }

    public void PlacePlayerObject() => playerData._cubeGOP.PlaceInstance(PlayerGPS);

}

