using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public struct PurificationEntity
{
    public SO_CorruptEntity corruptionEntity;
    public float currentCorruptionLevel;

}

public class PurificationManager : MonoBehaviour
{
    public PurificationEntity currentPurificationEntity;

    public static PurificationManager Instance;

    public GameObject purificationPanel;

    public GameObject HorizontalLayoutObject;
    public GameObject selectedEssenceImagePrefab;

    public int maxSelectedEssences;

    public List<SO_EssenceMaterialType> selectedEssenceMaterials;
    public List<SO_EssenceMaterialType> playerEssencePouch;

    public Image corruptEntityImage;

    public Image corruptionMeter;



    private Dictionary<SO_EssenceMaterialType, GameObject> 
        SelectedEssenceToSelectedEssenceImageDictionary = new Dictionary<SO_EssenceMaterialType, GameObject>();

    [SerializeField] private ManaLens manaLens;


    // Define a delegate and an event
    public delegate void PlayerAttemptsPurification(float currentCoruptionLevel);
    public static event PlayerAttemptsPurification OnPlayerAttemptsPurification;

    public delegate void PlayerAddsEssenceBackToPouch(SO_EssenceMaterialType essenceMaterialType);
    public static event PlayerAddsEssenceBackToPouch OnPlayerAddsEssenceBackToPouch;


    public delegate void PlayerRemovesEssenceFromPouch(SO_EssenceMaterialType essenceMaterialType);
    public static event PlayerRemovesEssenceFromPouch OnPlayerRemovesEssenceFromPouch;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnEnable()
    {
        ManaLens.OnPlayerEncounterCorruptEntity += InitializePurificationEntity;
        UINotificationManager.OnPlayerChoosesPurify += BeginPurification;
    }

    private void InitializePurificationEntity(SO_CorruptEntity corruptEntity)
    {
        currentPurificationEntity = CreateNewPurificationEntity(corruptEntity);
    }

    private void OnDisable()
    {
        ManaLens.OnPlayerEncounterCorruptEntity -= InitializePurificationEntity;
        UINotificationManager.OnPlayerChoosesPurify -= BeginPurification;
    }

    void BeginPurification()
    {
        selectedEssenceMaterials.Clear();
        CopyPlayerEssencePouch();


        purificationPanel.SetActive(true);
        corruptEntityImage.sprite = currentPurificationEntity.corruptionEntity.corruptedStateSprite;
        purificationPanel.gameObject.SetActive(true);

        Debug.Log("Begining the purification of " + currentPurificationEntity.corruptionEntity.corruptEntityName);



    }

    public void AttemptPurification()
    {
        float purificationPoints = CalculatePurificationPoints();
        currentPurificationEntity.currentCorruptionLevel -= purificationPoints;


        if (currentPurificationEntity.currentCorruptionLevel <= 0)
        {
            Debug.Log("Corrupt Entity hath been Purified!");
        }

        OnPlayerAttemptsPurification?.Invoke(currentPurificationEntity.currentCorruptionLevel);
    }

    private float CalculatePurificationPoints()
    {
        float purificationPoints = 0;

        // Calculate base healing points
        foreach (SO_EssenceMaterialType selectedEssenceMaterialType in selectedEssenceMaterials)
        {
            foreach (EssenceEffectiveness corruptedEntityEffectiveEssence in currentPurificationEntity.corruptionEntity.essenceEffectivenessList)
            {
                if (corruptedEntityEffectiveEssence.essenceMaterialType.Equals(selectedEssenceMaterialType))
                {
                    purificationPoints += corruptedEntityEffectiveEssence.effectiveness;
                }
            }
        }

        // Apply combination multipliers
        for (int i = 0; i < selectedEssenceMaterials.Count; i++)
        {
            for (int j = i + 1; j < selectedEssenceMaterials.Count; j++)
            {
                SO_EssenceMaterialType material1 = selectedEssenceMaterials[i];
                SO_EssenceMaterialType material2 = selectedEssenceMaterials[j];

                foreach (CombinationMultiplier combinationMultiplier in currentPurificationEntity.corruptionEntity.combinationMultipliersList)
                {
                    if ((combinationMultiplier.essenceMaterialType1.Equals(material1) && combinationMultiplier.essenceMaterialType2.Equals(material2)) ||
                        (combinationMultiplier.essenceMaterialType1.Equals(material2) && combinationMultiplier.essenceMaterialType2.Equals(material1)))
                    {
                        purificationPoints *= combinationMultiplier.multiplier;
                    }
                }
            }
        }

        return purificationPoints;
    }


    PurificationEntity CreateNewPurificationEntity(SO_CorruptEntity corruptEntity)
    {
        PurificationEntity purificationEntity = new PurificationEntity();
        purificationEntity.corruptionEntity = corruptEntity;
        purificationEntity.currentCorruptionLevel = Random.Range(corruptEntity.corruptionLevelRangeMinMax.x, corruptEntity.corruptionLevelRangeMinMax.y);
        return purificationEntity;
    }

    public void SelectEssenceMaterial(SO_EssenceMaterialType essenceMaterialType)
    {
        if (selectedEssenceMaterials.Count <= maxSelectedEssences)
        {

            GameObject currentEssenceImagePrefab = Instantiate(selectedEssenceImagePrefab, HorizontalLayoutObject.transform);
            selectedEssenceMaterials.Add(essenceMaterialType);

            Image image = currentEssenceImagePrefab.GetComponent<Image>();
            image.color = essenceMaterialType.essenceMaterialColor;

            Button button = currentEssenceImagePrefab.GetComponent<Button>();
            button.onClick.AddListener(() => DeselectEssenceMaterial(essenceMaterialType));

            SelectedEssenceToSelectedEssenceImageDictionary.Add(essenceMaterialType, currentEssenceImagePrefab);
            playerEssencePouch.Remove(essenceMaterialType);

            OnPlayerRemovesEssenceFromPouch?.Invoke(essenceMaterialType);  // currently no subscribers that I know of
        }

    }

    public void DeselectEssenceMaterial(SO_EssenceMaterialType essenceMaterialType)
    {
        playerEssencePouch.Add(essenceMaterialType);
        selectedEssenceMaterials.Remove(essenceMaterialType);

        GameObject currentEssenceImagePrefab = SelectedEssenceToSelectedEssenceImageDictionary[essenceMaterialType];
        Destroy(currentEssenceImagePrefab);
        OnPlayerAddsEssenceBackToPouch?.Invoke(essenceMaterialType);  //add button back to inventory
    }


    private void CopyPlayerEssencePouch()
    {
        playerEssencePouch.Clear();
        foreach(ItemSlot item in manaLens.essencePouch.items)
        {
            if (item.item is SO_EssenceMaterialType essenceMaterialType)
            {
                for (int i = 0; i < item.amount; i++)
                {
                    playerEssencePouch.Add(essenceMaterialType);
                }
            }
        }
    }
}
