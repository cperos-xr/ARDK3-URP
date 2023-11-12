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
    public GameObject inventoryPanel;

    public GameObject HorizontalLayoutObject;
    public GameObject selectedEssenceImagePrefab;

    private List <GameObject> selectedEssenceImages = new List<GameObject>();


    public int maxSelectedEssences;

    public List<SO_EssenceMaterialType> selectedEssenceMaterials;
    public List<SO_EssenceMaterialType> playerEssencePouch;

    public Image corruptEntityImage;

    public Image corruptionMeter;

    [SerializeField] private ManaLens manaLens;


    // Define a delegate and an event
    public delegate void PlayerAttemptsPurification(float currentCoruptionLevel);
    public static event PlayerAttemptsPurification OnPlayerAttemptsPurification;

    public delegate void CreatedANewPurificationEntity(PurificationEntity purificationEntity);
    public static event CreatedANewPurificationEntity OnCreatedANewPurificationEntity;

    public delegate void PlayerAddsEssenceBackToPouch(SO_EssenceMaterialType essenceMaterialType);
    public static event PlayerAddsEssenceBackToPouch OnPlayerAddsEssenceBackToPouch;

    public delegate void PlayerRemovesEssenceFromPouch(SO_EssenceMaterialType essenceMaterialType);
    public static event PlayerRemovesEssenceFromPouch OnPlayerRemovesEssenceFromPouch;


    public delegate void PlayerPurifiesEntity(PurificationEntity purificationEntity);
    public static event PlayerPurifiesEntity OnPlayerPurifiesEntity;

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

        purificationPanel.SetActive(true);
        corruptEntityImage.sprite = currentPurificationEntity.corruptionEntity.corruptedStateSprite;


        OnCreatedANewPurificationEntity(currentPurificationEntity);

    }

    private void OnDisable()
    {
        ManaLens.OnPlayerEncounterCorruptEntity -= InitializePurificationEntity;
        UINotificationManager.OnPlayerChoosesPurify -= BeginPurification;
    }

    void BeginPurification()
    {
        PlayerManager.Instance.currentPlayerState = PlayerState.purification;
        InitializePurificationEntity(currentPurificationEntity.corruptionEntity);
        selectedEssenceMaterials.Clear();
        CopyPlayerEssencePouch();
        Debug.Log("Begining the purification of " + currentPurificationEntity.corruptionEntity.corruptEntityName);

    }

    public void AttemptPurification()
    {
        float purificationPoints = CalculatePurificationPoints();
        Debug.Log("Attempting to purify with " +  purificationPoints + "pts");
        
        currentPurificationEntity.currentCorruptionLevel -= purificationPoints;

        foreach(SO_EssenceMaterialType essenceMaterialType in selectedEssenceMaterials)
        {
            manaLens.essencePouch.RemoveItem(essenceMaterialType);
        }

        // Destroy all essence images
        foreach (GameObject selectedEssenceImage in selectedEssenceImages)
        {
            Destroy(selectedEssenceImage);
        }

        if (currentPurificationEntity.currentCorruptionLevel <= 0)
        {
            Debug.Log("Corrupt Entity hath been Purified!");
            corruptEntityImage.sprite = currentPurificationEntity.corruptionEntity.healedStateSprite;
            PlayerManager.Instance.currentPlayerState = PlayerState.normal;
            OnPlayerPurifiesEntity?.Invoke(currentPurificationEntity);
        }

        // Now that we've finished iterating, clear the collections
        selectedEssenceImages.Clear();
        selectedEssenceMaterials.Clear();


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
                    Debug.Log($"Added effectiveness {corruptedEntityEffectiveEssence.effectiveness} from {selectedEssenceMaterialType.name}");
                }
                else
                {
                    // purificationPoints += Random.Range(-1, 2f);
                }
            }
        }

        Debug.Log($"Base purification points: {purificationPoints}");

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
                        Debug.Log($"Applying multiplier {combinationMultiplier.multiplier} for pair {material1.name}, {material2.name}");
                    }
                }
            }
        }

        Debug.Log($"Total purification points after multipliers: {purificationPoints}");
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
        if (selectedEssenceMaterials.Count < maxSelectedEssences)
        {

            GameObject currentEssenceImagePrefab = Instantiate(selectedEssenceImagePrefab, HorizontalLayoutObject.transform);
            selectedEssenceMaterials.Add(essenceMaterialType);

            Image image = currentEssenceImagePrefab.GetComponent<Image>();
            image.color = essenceMaterialType.essenceMaterialColor;

            Button button = currentEssenceImagePrefab.GetComponent<Button>();
            button.onClick.AddListener(() => DeselectEssenceMaterial(essenceMaterialType));
            button.onClick.AddListener(() => Destroy(currentEssenceImagePrefab));

            selectedEssenceImages.Add(currentEssenceImagePrefab);
            playerEssencePouch.Remove(essenceMaterialType);

            if (selectedEssenceMaterials.Count == maxSelectedEssences)
            {
                inventoryPanel.SetActive(false);


            }

            OnPlayerRemovesEssenceFromPouch?.Invoke(essenceMaterialType);  // currently no subscribers that I know of
        }


    }

    public void DeselectEssenceMaterial(SO_EssenceMaterialType essenceMaterialType)
    {
        playerEssencePouch.Add(essenceMaterialType);
        selectedEssenceMaterials.Remove(essenceMaterialType);

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
