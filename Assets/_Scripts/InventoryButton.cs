
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
            buttonText.text = iButtonObject.ObjectName;
        }
    }

    public void InitialIzeItemButton(IButtonObject iButtonObject)
    {
        this.iButtonObject = iButtonObject;
        icon.sprite = this.iButtonObject.Icon;
        buttonText.text = this.iButtonObject.ObjectName;
    }
}

public interface IButtonObject
{
    Sprite Icon { get; }
    string ObjectName { get; }
}

