using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaLens : MonoBehaviour
{
    [Serializable]
    public enum ELensState
    {
        inactive,
        Extracting,
        Scouting
    }
    // Start is called before the first frame update
    public Inventory essencePouch;
    public bool isActive;
    public int lowerScreenLimit;  //Acts as bounds so doesnt work on menu taps.

    public TMP_Dropdown lensStateDropdown; // UI Dropdown

    public List<SO_EssenceMaterialType> essenceMaterialTypes = new List<SO_EssenceMaterialType>();

    public delegate void PlayerGivenEssenceMaterialEvent(SO_EssenceMaterialType essenceMaterial);
    public static event PlayerGivenEssenceMaterialEvent OnPlayerGivenEssenceMaterial;


    public delegate void PlayerEncounterCorruptEntityEvent(SO_CorruptEntity corruptEntity);
    public static event PlayerEncounterCorruptEntityEvent OnPlayerEncounterCorruptEntity;



    public ELensState lensState = ELensState.inactive;

    private void OnEnable()
    {
        SemanticChannelDetector.OnSemanticChannelIdentified += ReceiveEssenceMaterial;
    }

    private void OnDisable()
    {
        SemanticChannelDetector.OnSemanticChannelIdentified -= ReceiveEssenceMaterial;
    }

    private void Start()
    {
        PopulateLensStateDropdown();
        lensStateDropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(lensStateDropdown); });
    }

    private void PopulateLensStateDropdown()
    {
        lensStateDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (var state in Enum.GetValues(typeof(ELensState)))
        {
            options.Add(state.ToString());
        }
        lensStateDropdown.AddOptions(options);
    }

    private void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        lensState = (ELensState)dropdown.value;
        Debug.Log("Lens State changed to: " + lensState.ToString());
    }

    public void ReceiveEssenceMaterial(List<string> semanticChannelList, Vector2 point)
    {
        if (point.y > lowerScreenLimit)
        {
            switch (lensState)
            {
                case ELensState.inactive:
                    break;
                case ELensState.Extracting:
                    ReceiveEssenceMaterial(semanticChannelList);
                    break;
                case ELensState.Scouting:
                    SeekCorruptSpirts(semanticChannelList);
                    break;
            }
        }
    }

    private void SeekCorruptSpirts(List<string> semanticChannelList)
    {
        foreach (string semanticChannel in semanticChannelList)
        {
            foreach (SO_AreaData currentArea in AreaManager.Instance.currentAreas)
            {
                foreach (SO_EssenceMaterialType essenceMaterialType in currentArea.corruptedEssenceMaterialTypes)
                {
                    if (semanticChannel.Equals(essenceMaterialType.essenceMaterialSemanticChannelName))
                    {
                        // Found a corrupt spirit
                        SO_CorruptEntity corruptEntity = CorruptEntityManager.Instance.GetEntityByEssenceAndScarcity(essenceMaterialType, AreaManager.Instance.currentAreas);
                        if (corruptEntity != null)
                        {
                            FoundCorruptEntity(corruptEntity);
                        }
                    }
                }
            }

        }

    }

    private void FoundCorruptEntity(SO_CorruptEntity corruptEntity)
    {
        PurificationManager.Instance.currentCorruptEntity = corruptEntity;
        Debug.Log("Found Corrupt entity " + corruptEntity.corruptEntityName);
        OnPlayerEncounterCorruptEntity?.Invoke(corruptEntity);
    }


    public void ReceiveEssenceMaterial(List<string> semanticChannelList)
    {
        foreach (string semanticChannel in semanticChannelList)
        {
            foreach (SO_EssenceMaterialType essenceMaterialType in essenceMaterialTypes)
            {
                if (semanticChannel.Equals(essenceMaterialType.essenceMaterialSemanticChannelName))
                {

                    if (essencePouch.AddItem(essenceMaterialType))
                    {
                        Debug.Log($"Successfully added {essenceMaterialType.itemName} to the essence pouch");
                        OnPlayerGivenEssenceMaterial?.Invoke(essenceMaterialType);
                    }
                    else
                    {
                        // Pouch is full
                        Debug.Log($"Could not add {essenceMaterialType.itemName} to the essence pouch");
                    }
                }
            }
        }
    }
}



