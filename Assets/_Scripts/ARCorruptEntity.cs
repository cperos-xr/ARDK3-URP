using System.Collections.Generic;
public class ARCorruptEntity : AREntity
{
    public SO_CorruptEntity corruptEntity;
    public List<ObjectToSetActiveAndActivationStatus> setObjectsActiveStatusUponPurification = new List<ObjectToSetActiveAndActivationStatus>();
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
                foreach (ObjectToSetActiveAndActivationStatus obj in setObjectsActiveStatusUponPurification)
                {
                    obj.objectToSet.SetActive(obj.setStatus);
                }
            }
        }
    }
}