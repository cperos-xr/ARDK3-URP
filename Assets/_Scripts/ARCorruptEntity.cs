using System.Collections.Generic;
public class ARCorruptEntity : AREntity
{
    public SO_CorruptEntity corruptEntity;
    public List<ObjectToSetActive> setObjectsActiveStatusUponPurification = new List<ObjectToSetActive>();
    private void OnEnable()
    {
        PurificationManager.OnPlayerPurifiesEntity += ARPurification;
    }
    private void OnDisable()
    {
        PurificationManager.OnPlayerPurifiesEntity -= ARPurification;
    }
    private void ARPurification(PurificationEntity purificationEntity)
    {
        if (purificationEntity.corruptedEntity.Equals(corruptEntity))
        {
            if (setObjectsActiveStatusUponPurification.Count > 0)
            {
                foreach (ObjectToSetActive obj in setObjectsActiveStatusUponPurification)
                {
                    obj.objectToSet.SetActive(obj.setStatus);
                }
            }
        }
    }
}