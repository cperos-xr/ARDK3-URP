using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionController : MonoBehaviour
{
    [SerializeField] private List<GameObject> initialObjects;
    [SerializeField] private List<GameObject> secondSetObjects;
    [SerializeField] private List<GameObject> finalSetObjects;
    [SerializeField] private Image blackPanel;

    [SerializeField] private float initialSetDuration = 5f;
    [SerializeField] private float secondSetDuration = 5f;
    [SerializeField] private float fadeDuration = 2f;

    private bool lastTime = false;

    private void Start()
    {
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        SetActiveObjects(initialObjects, true);

        yield return new WaitForSeconds(initialSetDuration);

        // Fade in black panel, then switch to second set
        yield return StartCoroutine(FadePanel(true)); // Fade in
        SwitchObjects(initialObjects, secondSetObjects);
        yield return StartCoroutine(FadePanel(false)); // Fade out

        yield return new WaitForSeconds(secondSetDuration);

        // Fade in black panel, then switch to final set
        yield return StartCoroutine(FadePanel(true)); // Fade in
        SwitchObjects(secondSetObjects, finalSetObjects);
        lastTime = true;
        yield return StartCoroutine(FadePanel(false)); // Fade out
    }

    private IEnumerator FadePanel(bool fadeIn)
    {
        float endAlpha = fadeIn ? 1f : 0f;
        float startAlpha = blackPanel.color.a;
        float rate = 1.0f / fadeDuration;

        for (float t = 0f; t < 1.0f; t += Time.deltaTime * rate)
        {
            Color newColor = new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, Mathf.Lerp(startAlpha, endAlpha, t));
            blackPanel.color = newColor;
            yield return null;
        }

        blackPanel.color = new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, endAlpha); // Ensure final alpha is set

        if (lastTime)
        {
            blackPanel.gameObject.SetActive(false);
        }
    }

    private void SetActiveObjects(List<GameObject> objects, bool active)
    {
        foreach (var obj in objects)
        {
            obj.SetActive(active);
        }
    }

    private void SwitchObjects(List<GameObject> deactivateObjects, List<GameObject> activateObjects)
    {
        SetActiveObjects(deactivateObjects, false);
        SetActiveObjects(activateObjects, true);
    }
}
