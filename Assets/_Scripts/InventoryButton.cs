
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public IButtonObject iButtonObject;
    public Image icon;
    public TextMeshProUGUI buttonText;
    public Button button;

    private void OnEnable()
    {
        if (iButtonObject != null)
        {
            icon.sprite = iButtonObject.Icon;

            // Check if iButtonObject is of type SO_EssenceMaterialType to include channel name
            if (iButtonObject is SO_EssenceMaterialType essenceMaterialType)
            {
                string channelName = essenceMaterialType.essenceMaterialSemanticChannelName.Replace("_experimental", "");
                buttonText.text = $"{iButtonObject.ObjectName}\n({channelName})";
            }
            else
            {
                buttonText.text = iButtonObject.ObjectName;
            }
        }
    }


    public void InitialIzeItemButton(IButtonObject iButtonObject)
    {
        this.iButtonObject = iButtonObject;
        icon.sprite = this.iButtonObject.Icon;

        if (iButtonObject is SO_EssenceMaterialType essenceMaterialType)
        {
            string channelName = essenceMaterialType.essenceMaterialSemanticChannelName.Replace("_experimental", "");
            Debug.Log("Channel Name: " + channelName); // Debugging channel name

            buttonText.text = $"{iButtonObject.ObjectName}\n({channelName})";
            icon.color = essenceMaterialType.essenceMaterialColor;
        }
        else
        {
            buttonText.text = iButtonObject.ObjectName;
        }

        // Debugging to check if text is set correctly
        Debug.Log("Button Text: " + buttonText.text);
    }

}

public interface IButtonObject
{
    Sprite Icon { get; }
    string ObjectName { get; }
}

