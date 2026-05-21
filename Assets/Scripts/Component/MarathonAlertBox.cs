using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarathonAlertBox : MonoBehaviour
{
    #region MAIN
    public void ClearItemForMarathonMode()
    {
        MeloMelo_ItemStore_Management.ClearingUpUsedItem();
        gameObject.SetActive(false);
    }

    public void CancelAccessToMarathonMode()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
