using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorruptionMeter : MonoBehaviour
{
    public RectTransform meterTransform; // Assign this in the inspector
    public float maxCorruptionLevel; // Set this to the maximum corruption level
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI nameText;

    // Call this method to update the meter
    private void OnEnable()
    {
        PurificationManager.OnPlayerAttemptsPurification += SetCorruptionLevel;
        PurificationManager.OnCreatedANewPurificationEntity += SetCorruptionLevel;
    }

    private void OnDisable()
    {
        PurificationManager.OnPlayerAttemptsPurification -= SetCorruptionLevel;
        PurificationManager.OnCreatedANewPurificationEntity += SetCorruptionLevel;
    }

    // Call this method to initiate a smooth transition of the meter


    public void SetCorruptionLevel(PurificationEntity purificationEntity)
    {
        
        pointsText.text = purificationEntity.currentCorruptionLevel.ToString();
        nameText.text = purificationEntity.corruptedEntity.corruptEntityName;

        float targetFillAmount = purificationEntity.currentCorruptionLevel / (purificationEntity.currentCorruptionLevel * 1.5f);

        meterTransform.localScale = new Vector3(targetFillAmount, 1f, 1f);

    }


    public void SetCorruptionLevel(float currentLevel)
    {
        pointsText.text = currentLevel.ToString();

        float targetFillAmount = currentLevel / maxCorruptionLevel;

        meterTransform.localScale = new Vector3(targetFillAmount, 1f, 1f);

    }

}