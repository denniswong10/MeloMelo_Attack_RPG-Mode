using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyInTag_Scripts : MonoBehaviour
{
    [SerializeField] private string nameOfCurrency;
    [SerializeField] private MeloMelo_Economy.CurrencyType currencyType;

    public void ToggleCurrencyTag(bool display)
    {
        transform.GetChild(3).gameObject.SetActive(display);
        transform.GetChild(3).GetChild(0).GetComponent<Text>().text = nameOfCurrency;
    }

    public void UpdateCurrencyValue()
    {
        transform.GetChild(2).GetComponent<Text>().text = GetCurrencyValue();
    }

    #region MISC
    private string GetCurrencyValue()
    {
        switch (currencyType)
        {
            case MeloMelo_Economy.CurrencyType.HonorCoin:
                return MeloMelo_Economy.GetHonorCoinCurrency(LoginPage_Script.thisPage.GetUserPortOutput()).ToString();

            case MeloMelo_Economy.CurrencyType.Credits:
                return MeloMelo_Economy.GetCreditCurrency(LoginPage_Script.thisPage.GetUserPortOutput()).ToString();

            case MeloMelo_Economy.CurrencyType.Storage_Space:
                return MeloMelo_Economy.GetResourceSpace(LoginPage_Script.thisPage.GetUserPortOutput()).ToString();

            default:
                return string.Empty;
        }
    }
    #endregion
}
