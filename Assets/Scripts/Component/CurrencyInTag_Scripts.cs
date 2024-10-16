using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyInTag_Scripts : MonoBehaviour
{
    [SerializeField] private string nameOfCurrency;

    public void ToggleCurrencyTag(bool display)
    {
        transform.GetChild(3).gameObject.SetActive(display);
        transform.GetChild(3).GetChild(0).GetComponent<Text>().text = nameOfCurrency;
    }

    public void UpdateCurrencyValue()
    {
        transform.GetChild(2).GetComponent<Text>().text = 
            PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_" + nameOfCurrency, 0).ToString();
    }
}
