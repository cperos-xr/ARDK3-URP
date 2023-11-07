using System.Collections.Generic;
using UnityEngine;

public class BaseEntityData : ScriptableObject
{
    public string entityName;
    public int startingInteractionIndex;
    public List<SO_Interaction> interactions = new List<SO_Interaction>();

}
