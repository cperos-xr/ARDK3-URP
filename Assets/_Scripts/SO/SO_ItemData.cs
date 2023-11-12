using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Inventory/Item")]
public class SO_ItemData : InteractionProgression, IButtonObject
{
    public string itemName;
    public Sprite icon;
    public bool isUsable;
    public bool isDuplicable;
    public List<PlayerNotification> playerNotifications; // Assuming Notification is another class or struct you've defined

    // No need to serialize this as it will be set dynamically
    [HideInInspector]
    public string givenByEntityName = "";

    public Sprite Icon => icon;
    public string ObjectName => itemName;

    // Rest of your fields and methods
}
