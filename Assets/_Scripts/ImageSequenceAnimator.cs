using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSequenceAnimator : MonoBehaviour
{
    [SerializeField] private List<Image> images;
    [SerializeField] private float overallDuration = 10f;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        StartCoroutine(AnimateImages());
    }

    private IEnumerator AnimateImages()
    {
        // Calculate the duration for each image to stay on screen
        float displayDuration = (overallDuration - images.Count * fadeDuration) / images.Count;

        for (int i = 0; i < images.Count; i++)
        {
            Image image = images[i];
            // Initially set each image to transparent
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            image.gameObject.SetActive(true);

            // Fade in
            yield return StartCoroutine(FadeImage(image, 1f)); // Fade to opaque

            // If it's not the last image, display it for the duration and then fade out
            if (i < images.Count - 1)
            {
                yield return new WaitForSeconds(displayDuration);
                yield return StartCoroutine(FadeImage(image, 0f)); // Fade to transparent
                image.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator FadeImage(Image image, float targetAlpha)
    {
        float alpha = image.color.a;
        float fadeSpeed = Mathf.Abs(alpha - targetAlpha) / fadeDuration;

        while (!Mathf.Approximately(image.color.a, targetAlpha))
        {
            float newAlpha = Mathf.MoveTowards(image.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
            yield return null;
        }
    }
}
