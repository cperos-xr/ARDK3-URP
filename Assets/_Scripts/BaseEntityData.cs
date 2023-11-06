using System.Collections.Generic;
using UnityEngine;

public class BaseEntityData : ScriptableObject
{
    public string entityName;
    public int startingInteractionIndex;
    public List<Interaction> interactions = new List<Interaction>();

}
