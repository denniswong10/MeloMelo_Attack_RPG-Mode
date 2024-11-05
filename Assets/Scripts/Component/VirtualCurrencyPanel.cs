using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualCurrencyPanel : MonoBehaviour
{
    [SerializeField] private Text PlayerName;
    [SerializeField] private CurrencyInTag_Scripts[] currecnyList;

    void Start()
    {
        PlayerName.text = LoginPage_Script.thisPage.GetUserPortOutput();
        for (int instance = 0; instance < currecnyList.Length; instance++) currecnyList[instance].UpdateCurrencyValue();
    }

    #region MAIN
    public void ClosePanel()
    {
        Destroy(gameObject);
    }
    #endregion
}
