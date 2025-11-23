using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator_Script : MonoBehaviour
{
    private float currentTimeline = 0;

    #region SETUP
    public void Setup(int setTimeOut)
    {
        currentTimeline = Time.time + setTimeOut;
    }
    #endregion

    void Update()
    {
        if (gameObject.activeInHierarchy && Time.time >= currentTimeline) 
            GameManager.thisManager.getInGameObjectWindow.damageIndicator.ReturnPopUpText(gameObject);
    }
}
