using UnityEngine;

public class DemoNotification : MonoBehaviour
{


    [SerializeField] private UINotificationManager notificationManager;
    [SerializeField] private Sprite notificationIcon;
    private string heading = "Demo Level Notice";
    private string content = "This level is designed as a Demo for editor-based playtesting. If you're in Waikiki and looking to enjoy the complete AR experience, please switch to the Full Experience Scene. The Demo Scene is optimized for development purposes and may not reflect the full richness and functionality of the actual game.";

    private void Start()
    {
        PlayerNotification notification = new PlayerNotification();
        notification.notificationHeading = heading;
        notification.notificationContent = content;
        notification.notificationColor = new Color (137, 207, 240, 255);
        notification.notificationIcon = notificationIcon;
        notification.buttonText0 = "OK";
        notificationManager.GeneralNotification(notification);
    }
}
