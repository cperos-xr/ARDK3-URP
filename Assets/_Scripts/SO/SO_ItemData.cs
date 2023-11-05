using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Inventory/Item")]
public class SO_ItemData : ScriptableObject, IButtonObject
{
    public string itemName;
    public Sprite icon;
    public bool isUsable;
    public bool isLocked;
    public bool isDuplicable;
    public int healthRecovery;
    public List<PlayerNotification> playerNotifications; // Assuming Notification is another class or struct you've defined

    // No need to serialize this as it will be set dynamically
    [HideInInspector]
    public string givenByEntityName = "";

    public Sprite Icon => icon;
    public string ObjectName => itemName;

    // Rest of your fields and methods
}
