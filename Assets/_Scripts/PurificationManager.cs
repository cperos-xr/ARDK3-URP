using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurificationManager : MonoBehaviour
{
    public SO_CorruptEntity currentCorruptEntity;
    public static PurificationManager Instance;

    public GameObject purificationPanel;
    public Image entityImage;

    public List<SO_EssenceMaterialType> selectedEssenceMaterials;
    public List<SO_EssenceMaterialType> playerEssencePouch;

    [SerializeField] private ManaLens manaLens;

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
        UINotificationManager.OnPlayerChoosesPurify += BeginPurification;
    }

    private void OnDisable()
    {
        UINotificationManager.OnPlayerChoosesPurify -= BeginPurification;
    }

    void BeginPurification()
    {
        selectedEssenceMaterials.Clear();

        CopyPlayerEssencePouch();


        purificationPanel.SetActive(true);
        entityImage.sprite = currentCorruptEntity.corruptedStateSprite;
        purificationPanel.gameObject.SetActive(true);

        Debug.Log("Begining the purification of " + currentCorruptEntity.corruptEntityName);


    }

    public void SelectEssenceMaterial(SO_EssenceMaterialType essenceMaterialType)
    {
        selectedEssenceMaterials.Add(essenceMaterialType);
        playerEssencePouch.Remove(essenceMaterialType);
    }

    public void DeselectEssenceMaterial(SO_EssenceMaterialType essenceMaterialType)
    {
        playerEssencePouch.Add(essenceMaterialType);
        selectedEssenceMaterials.Remove(essenceMaterialType);
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
