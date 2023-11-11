using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CorruptionMeter : MonoBehaviour
{
    public Image meterImage; // Assign this in the inspector
    public float maxCorruptionLevel; // Set this to the maximum corruption level
    public float transitionDuration = 1.0f; // Duration of the lerp transition

    private Coroutine transitionCoroutine;

    // Call this method to update the meter
    private void OnEnable()
    {
        PurificationManager.OnPlayerAttemptsPurification += SetCorruptionLevel;
    }

    private void OnDisable()
    {
        PurificationManager.OnPlayerAttemptsPurification -= SetCorruptionLevel;
    }

    // Call this method to initiate a smooth transition of the meter
    public void SetCorruptionLevel(float currentLevel)
    {
        float targetFillAmount = currentLevel / maxCorruptionLevel;

        // If there is an ongoing transition, stop it
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        // Start a new transition
        transitionCoroutine = StartCoroutine(LerpMeter(targetFillAmount));
    }

    private IEnumerator LerpMeter(float targetFillAmount)
    {
        float time = 0;
        float startFillAmount = meterImage.fillAmount;

        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            meterImage.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, time / transitionDuration);
            yield return null;
        }

        meterImage.fillAmount = targetFillAmount; // Ensure it ends exactly at the target value
    }
}