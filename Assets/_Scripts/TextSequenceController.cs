using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TextSequenceController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private List<TextDisplayInfo> textDisplaySequence;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float initialDelay = 1f; // Initial delay before the first message

    private void OnEnable()
    {
        StartCoroutine(DisplayTextSequence());
    }

    private IEnumerator DisplayTextSequence()
    {
        // Wait for the initial delay
        yield return new WaitForSeconds(initialDelay);

        foreach (var textInfo in textDisplaySequence)
        {
            textMeshPro.text = textInfo.textToDisplay;

            // Fade in
            yield return StartCoroutine(FadeText(0, 1, fadeInDuration));

            // Wait for the duration specified in delayUntilNext
            yield return new WaitForSeconds(textInfo.delayUntilNext);

            // Fade out
            yield return StartCoroutine(FadeText(1, 0, fadeOutDuration));
        }
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float currentTime = 0;
        Color currentColor = textMeshPro.color;

        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / duration);
            textMeshPro.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is set
        textMeshPro.color = new Color(currentColor.r, currentColor.g, currentColor.b, endAlpha);
    }
}

[System.Serializable]
public struct TextDisplayInfo
{
    public string textToDisplay;
    public float delayUntilNext;
}
