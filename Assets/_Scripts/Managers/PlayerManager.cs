using Niantic.Lightship.Maps.Core.Coordinates;
using UnityEngine;

public enum PlayerState
{
    normal,
    purification,
}
public class PlayerManager : MonoBehaviour
{
    static public PlayerManager Instance;

    public SO_PlayerData playerData; // Reference to the PlayerData SO
    public Inventory inventory;

    [HideInInspector] public LatLng PlayerGPS;

    public PlayerState currentPlayerState = PlayerState.normal;

    // Other variables such as health, magic, etc.
    public int health;
    public int magic;
    public int stepsTaken;

    // Initialization, update player stats, etc.
    void Start()
    {
        // Load data from PlayerData
        //health = playerData.initialHealth;
        //magic = playerData.initialMagic;
        // ... more initialization
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

    public void UseItem(SO_ItemData item)
    {
        // Use item logic
        inventory.RemoveItem(item);
    }

    public void PlacePlayerObject() => playerData._cubeGOP.PlaceInstance(PlayerGPS);

}

