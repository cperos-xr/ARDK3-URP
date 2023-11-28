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
    public int upperScreenLimit;  //Acts as bounds so doesnt work on menu taps.

    public TMP_Dropdown lensStateDropdown; // UI Dropdown

    public List<SO_EssenceMaterialType> essenceMaterialTypes = new List<SO_EssenceMaterialType>();

    public delegate void PlayerGivenEssenceMaterialEvent(SO_EssenceMaterialType essenceMaterial);
    public static event PlayerGivenEssenceMaterialEvent OnPlayerGivenEssenceMaterial;


    public delegate void PlayerEncounterCorruptEntityEvent(SO_CorruptEntity corruptEntity);
    public static event PlayerEncounterCorruptEntityEvent OnPlayerEncounterCorruptEntity;

    [SerializeField] private SO_ItemData manaLensItemData;
    public ELensState lensState = ELensState.inactive;

    [SerializeField] private bool playerHasManaLens = false;

    private void OnEnable()
    {
        SemanticChannelDetector.OnSemanticChannelIdentified += ReceiveEssenceMaterial;
        InteractionManager.OnPlayerReceiveItem += CheckForManaLens;
        TapButtonActivator.OnPlayerEnterDebugMode += EnableManaLens;
    }

    private void OnDisable()
    {
        SemanticChannelDetector.OnSemanticChannelIdentified -= ReceiveEssenceMaterial;
        InteractionManager.OnPlayerReceiveItem -= CheckForManaLens;
        TapButtonActivator.OnPlayerEnterDebugMode -= EnableManaLens;
    }

    private void EnableManaLens()
    {
        playerHasManaLens = true;
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
        if (point.y > lowerScreenLimit  && point.y < upperScreenLimit)
        {
            switch (lensState)
            {
                case ELensState.inactive:
                    break;
                case ELensState.Extracting:
                    ReceiveEssenceMaterial(semanticChannelList);
                    break;
                case ELensState.Scouting:
                    SeekCorruptSpirits(semanticChannelList);
                    break;
            }
        }
    }

    public void CheckForManaLens(SO_ItemData itemData, BaseEntityData entityData)
    {
        if (!playerHasManaLens && 
            itemData.Equals(manaLensItemData))
        { 
            playerHasManaLens = true;
        }
    }

    private void SeekCorruptSpirits(List<string> semanticChannelList)
    {
        if (!playerHasManaLens)
        {
            Debug.Log("Player doesnt have a Mana Lens");
            return;
        }
        if (semanticChannelList.Count == 0)
        {
            Debug.Log("No semantic channels to search for.");
            return;
        }
        if (PlayerManager.Instance.currentPlayerState == PlayerState.notification)
        {
            Debug.Log("Player notification active");
            return;
        }

        // Select a random channel from the list
        int randomIndex = UnityEngine.Random.Range(0, semanticChannelList.Count);
        string randomSemanticChannel = semanticChannelList[randomIndex];
        Debug.Log($"Selected random channel: {randomSemanticChannel}");

        // Search for corrupt spirits in the selected channel
        foreach (SO_AreaData currentArea in AreaManager.Instance.currentAreas)
        {
            foreach (SO_EssenceMaterialType essenceMaterialType in currentArea.corruptedEssenceMaterialTypes)
            {
                if (randomSemanticChannel.Equals(essenceMaterialType.essenceMaterialSemanticChannelName))
                {
                    // Found a corrupt spirit
                    SO_CorruptEntity corruptEntity = CorruptEntityManager.Instance.GetEntityByEssenceAndScarcity(essenceMaterialType, AreaManager.Instance.currentAreas);
                    if (corruptEntity != null)
                    {
                        FoundCorruptEntity(corruptEntity);
                        return; // Stop after finding the first corrupt entity
                    }
                }
            }
        }
    }


    private void FoundCorruptEntity(SO_CorruptEntity corruptEntity)
    {
        Debug.Log("Found Corrupt entity " + corruptEntity.corruptEntityName);
        lensState = ELensState.inactive;
        OnPlayerEncounterCorruptEntity?.Invoke(corruptEntity);
    }


    public void ReceiveEssenceMaterial(List<string> semanticChannelList)
    {
        if (playerHasManaLens && PlayerManager.Instance.currentPlayerState != PlayerState.notification)
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
}



