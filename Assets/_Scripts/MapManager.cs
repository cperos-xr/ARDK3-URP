using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private SO_ItemData menehuneMap;
    [SerializeField] private bool mapDataHasBeenPlaced = false;

    public bool playerHasMenehuneMap = false;


    private void OnEnable()
    {
        InteractionManager.OnPlayerReceiveItem += CheckForMenehuneMap;
    }

    private void OnDisable()
    {
        InteractionManager.OnPlayerReceiveItem -= CheckForMenehuneMap;
    }
    
    private void CheckForMenehuneMap(SO_ItemData itemData, BaseEntityData entityData)
    {
        if (itemData.Equals(menehuneMap))
        {
            playerHasMenehuneMap = true;
            if(!mapDataHasBeenPlaced) 
            {
                mapDataHasBeenPlaced = true;
                LevelManager.Instance.PlaceAllMarkers();
            }

        }

    }
}
