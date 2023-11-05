using System;
using System.Collections;
using System.Collections.Generic;
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

    private void OnEnable()
    {
        ItemManager.OnPlayerGivenItem += ItemNotification;
        InteractionManager.OnPlayerInteraction += InteractionNotification;
    }

    private void OnDisable()
    {
        ItemManager.OnPlayerGivenItem -= ItemNotification;
        InteractionManager.OnPlayerInteraction -= InteractionNotification;
    }

    private void ItemNotification(SO_ItemData item)
    {
        Random r = new Random();
        int index = r.Next(0, item.playerNotifications.Count);

        PlayerNotification playerNotification = item.playerNotifications[index];

        // Replace {0} with item name in all headings and descriptions
        playerNotification.notificationHeading = string.Format(playerNotification.notificationHeading, item.itemName);
        playerNotification.notificationContent = string.Format(playerNotification.notificationContent, item.itemName);

        notificationQueue.Enqueue(playerNotification);

        if (!displayingNotification)
        {
            DisplayNextNotification();
        }
    }

    private void InteractionNotification(Interaction EntityNotifyPlayer)
    {
        notificationQueue.Enqueue(EntityNotifyPlayer.notification);

        if (!displayingNotification)
        {
            DisplayNextNotification();
        }
    }

    private void DisplayNextNotification()
    {
        if (notificationQueue.Count > 0)
        {
            PlayerNotification notification = notificationQueue.Dequeue();
            displayingNotification = true;

            // Set the UI elements to display the current notification
            notificationHeading.text = notification.notificationHeading;
            notificationContent.text = notification.notificationContent;

            if (notification.notificationIcon != null)
            {
                notificationIcon.enabled = true;
                notificationIcon.sprite = notification.notificationIcon;
            }
            else
            {
                notificationIcon.enabled = false;
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

            // You can add a button click event handler to dismiss the notification
            notificationButton0.onClick.AddListener(() =>
            {
                displayingNotification = false;
                notificationPanel.SetActive(false);
                DisplayNextNotification();
            });

            notificationPanel.SetActive(true);
        }
        else
        {
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

}