using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class EssenceMaterialTypeContainer : MonoBehaviour
{
    public SO_EssenceMaterialType essenceMaterialType;
    public void SelectEssenceMaterialType()
    {
        PurificationManager.Instance.SelectEssenceMaterial(essenceMaterialType);
    }
}
