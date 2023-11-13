using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class UINotificationManager : MonoBehaviour
{
    public GameObject notificationPanel;
    public TextMeshProUGUI notificationHeading;
    public Image notificationIcon;
    public TextMeshProUGUI notificationContent;
    public TextMeshProUGUI notificationButtonText0;
    public TextMeshProUGUI notificationButtonText1;
    public GameObject notificationButton1_GO;
    public Button notificationButton0;
    public Button notificationButton1;


    [SerializeField] private List<PlayerNotification> playerNotifications = new List<PlayerNotification>();
    private Queue<PlayerNotification> notificationQueue = new Queue<PlayerNotification>();
    private bool displayingNotification = false;

    // Define a delegate and an event
    public delegate void PlayerChoosesHealEvent();
    public static event PlayerChoosesHealEvent OnPlayerChoosesPurify;

    private void OnEnable()
    {
        ItemManager.OnPlayerGivenItem += ItemNotification;
        InteractionManager.OnPlayerInteraction += InteractionNotification;
        ManaLens.OnPlayerGivenEssenceMaterial += ItemNotification;
        ManaLens.OnPlayerEncounterCorruptEntity += CorruptEntity;
        PurificationManager.OnPlayerPurifiesEntity += PurifyEntity;
        InteractionManager.OnPlayerCorruptEntityInteraction += CorruptEntity;
    }

    private void OnDisable()
    {
        ItemManager.OnPlayerGivenItem -= ItemNotification;
        InteractionManager.OnPlayerInteraction -= InteractionNotification;
        ManaLens.OnPlayerGivenEssenceMaterial -= ItemNotification;
        ManaLens.OnPlayerEncounterCorruptEntity -= CorruptEntity;
        PurificationManager.OnPlayerPurifiesEntity -= PurifyEntity;
        InteractionManager.OnPlayerCorruptEntityInteraction -= CorruptEntity;
    }

    private void PurifyEntity(PurificationEntity purificationEntity)
    {
        PlayerNotification playerNotification = new PlayerNotification();

        playerNotification.notificationHeading = "Did you Know?";
        playerNotification.notificationContent = purificationEntity.corruptionEntity.funFact;
        playerNotification.notificationIcon = purificationEntity.corruptionEntity.healedStateSprite;
        playerNotification.notificationColor = Color.white;
        playerNotification.notificationType = NotificationType.PurifySuccess;

        //playerNotification.buttonText1 = "Mahalo";
        playerNotification.buttonText0 = "Mahalo";

        notificationQueue.Enqueue(playerNotification);

        if (!displayingNotification)
        {
            DisplayNextNotification();
        }
    }


    private void CorruptEntity(SO_CorruptEntity corruptEntity)
    {
        if (PlayerManager.Instance.currentPlayerState == PlayerState.normal)
        {
            PlayerManager.Instance.currentPlayerState = PlayerState.purification;

            PlayerNotification playerNotification = new PlayerNotification();

            playerNotification.notificationHeading = corruptEntity.corruptEntityName;
            playerNotification.notificationContent = corruptEntity.corruptEntityDescription;
            playerNotification.notificationIcon = corruptEntity.corruptedStateSprite;
            playerNotification.notificationColor = Color.white;
            playerNotification.notificationType = NotificationType.CorruptEntity;

            playerNotification.buttonText0 = "Purify";
            playerNotification.buttonText1 = "Run";

            notificationQueue.Enqueue(playerNotification);

            if (!displayingNotification)
            {
                DisplayNextNotification();
            }
        }
    }

    private void ItemNotification(SO_ItemData item)
    {
        Random r = new Random();
        int index = r.Next(0, item.playerNotifications.Count);

        PlayerNotification playerNotification = item.playerNotifications[index];
        playerNotification.notificationType = NotificationType.Item;

        if (item is SO_EssenceMaterialType essenceMaterialType)
        {
            playerNotification.notificationIcon = essenceMaterialType.Icon;
            playerNotification.notificationColor = essenceMaterialType.essenceMaterialColor;

        }
        else
        {
            playerNotification.notificationColor = Color.white;
        }

        // Replace {0} with item name in all headings and descriptions
        playerNotification.notificationHeading = string.Format(playerNotification.notificationHeading, item.itemName);
        playerNotification.notificationContent = string.Format(playerNotification.notificationContent, item.itemName);

        notificationQueue.Enqueue(playerNotification);
        Debug.Log("Enqueued ItemNotification: " + playerNotification.notificationHeading);

        if (!displayingNotification)
        {
            DisplayNextNotification();
        }
    }

    private void InteractionNotification(SO_Interaction EntityNotifyPlayer)
    {

        if (EntityNotifyPlayer == null)
        {
            Debug.LogError("EntityNotifyPlayer is null in InteractionNotification.");
            return;
        }

        notificationQueue.Enqueue(EntityNotifyPlayer.notification);

        if (!displayingNotification)
        {
            DisplayNextNotification();
        }
    }

    private void DisplayNextNotification()
    {
        // Remove all existing listeners from the buttons to prevent overlap of events
        notificationButton0.onClick.RemoveAllListeners();
        notificationButton1.onClick.RemoveAllListeners();

        if (notificationQueue.Count > 0)
        {
            Debug.Log("Displaying next notification. Queue size: " + notificationQueue.Count);
            PlayerNotification notification = notificationQueue.Dequeue();
            displayingNotification = true;

            // Set the UI elements to display the current notification
            notificationHeading.text = notification.notificationHeading;
            notificationContent.text = notification.notificationContent;

            if (notification.notificationIcon != null)
            {
                notificationIcon.enabled = true;
                notificationIcon.sprite = notification.notificationIcon;
                notificationIcon.color = notification.notificationColor;
            }
            else
            {
                notificationIcon.enabled = false;
            }

            if (!string.IsNullOrEmpty(notification.buttonText0))
            {
                notificationButtonText0.text = notification.buttonText0;
            }

            if (string.IsNullOrEmpty(notification.buttonText1))
            {
                notificationButton1_GO.SetActive(false);
            }
            else
            {
                notificationButton1_GO.SetActive(true);
                notificationButtonText1.text = notification.buttonText1;
            }



            
            switch(notification.notificationType)
            {

                case NotificationType.CorruptEntity:
                    // Player chooses purify
                    notificationButton0.onClick.AddListener(() =>
                    {
                        OnPlayerChoosesPurify?.Invoke();
                        displayingNotification = false;
                        notificationPanel.SetActive(false);
                        DisplayNextNotification();
                    });

                    // Button That runs away
                    notificationButton1.onClick.AddListener(() =>
                    {
                        PurificationManager.Instance.purificationPanel.SetActive(false);
                        PlayerManager.Instance.currentPlayerState = PlayerState.normal;
                        displayingNotification = false;
                        notificationPanel.SetActive(false);
                        DisplayNextNotification();
                    });

                    break;
                case NotificationType.PurifySuccess:
                    // You can add a button click event handler to dismiss the notification
                    notificationButton0.onClick.AddListener(() =>
                    {
                        PurificationManager.Instance.purificationPanel.SetActive(false);
                        displayingNotification = false;
                        notificationPanel.SetActive(false);
                        DisplayNextNotification();
                    });

                    break;
                default:
                    // You can add a button click event handler to dismiss the notification
                    notificationButton0.onClick.AddListener(() =>
                    {
                        displayingNotification = false;
                        notificationPanel.SetActive(false);
                        DisplayNextNotification();
                    });
                    break;

            }



            notificationPanel.SetActive(true);
        }
        else
        {
            Debug.Log("No more notifications to display.");
            displayingNotification = false;
            notificationPanel.SetActive(false);
        }
    }
}




[Serializable]
public struct PlayerNotification
{
    public string notificationHeading;
    public string notificationContent;
    public string buttonText0;
    public string buttonText1;
    public Sprite notificationIcon;
    public Color notificationColor;
    public NotificationType notificationType;

}

[Serializable]

public enum NotificationType
{
    Normal,
    Interaction,
    Item,
    CorruptEntity,
    PurifySuccess,
    PurifyFail,
}