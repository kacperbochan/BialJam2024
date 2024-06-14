using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        Castor.ResetStaticData();
        NextLevelTrigger.ResetStaticData();
        Removable.ResetStaticData();
    }
}
