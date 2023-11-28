using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class MenuPanelController : MonoBehaviour
{
    public RectTransform menuPanel;
    public Transform hideIcon;
    public float moveDistance = 100f; // Distance to move off-screen
    public float duration = 1f; // Duration of the animation

    public Image centerImage;
    public Color seekingColor;
    public Color extractingColor;

    public TextMeshProUGUI manaLensStateText;

    [SerializeField] ManaLens manaLensScript; 

    private int counter = 0;


    private bool isMenuVisible = true; // Track the visibility state

    // Call this to toggle the menu visibility
    public void ToggleMenu()
    {
        Debug.Log("Toggle Menu");
        StartCoroutine(MoveMenu(isMenuVisible));
        isMenuVisible = !isMenuVisible; // Update the visibility state after each toggle
    }

    private IEnumerator MoveMenu(bool isCurrentlyVisible)
    {

        float time = 0;
        Vector3 startPosition = menuPanel.localPosition;
        Vector3 endPosition = isCurrentlyVisible
            ? startPosition - new Vector3(moveDistance, 0, 0) // Move left to hide
            : startPosition + new Vector3(moveDistance, 0, 0); // Move right to show

        Vector3 startRotation = hideIcon.localEulerAngles;
        Vector3 endRotation = startRotation + new Vector3(0, 0, 180);

        while (time < duration)
        {
            time += Time.deltaTime;
            menuPanel.localPosition = Vector3.Lerp(startPosition, endPosition, time / duration);
            hideIcon.localEulerAngles = Vector3.Lerp(startRotation, endRotation, time / duration);
            yield return null;
        }

        menuPanel.localPosition = endPosition;
        hideIcon.localEulerAngles = endRotation;
    }

    public void OnClickManaLensIcon()
    {
        counter++;
        if (counter % 3 == 2)
        {
            manaLensScript.lensState = ManaLens.ELensState.Scouting;
            manaLensStateText.text = "Seeking";
            centerImage.color = seekingColor;
        }
        else if (counter % 3 == 1)
        {
            manaLensScript.lensState = ManaLens.ELensState.Extracting;
            manaLensStateText.text = "Extracting";
            centerImage.color = extractingColor;
        }
        else
        {
            manaLensScript.lensState = ManaLens.ELensState.inactive;
            manaLensStateText.text = "Off";
            Color color = Color.clear;
            centerImage.color = color;
        }
    }
}
