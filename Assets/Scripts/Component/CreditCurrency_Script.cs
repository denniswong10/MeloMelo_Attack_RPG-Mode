using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditCurrency_Script : MonoBehaviour
{
    private int currency;
    public Text text;
    private string user;

    void Start()
    {
        try { user = LoginPage_Script.thisPage.get_user; }
        catch { user = "GUEST"; }

        currency = PlayerPrefs.GetInt(user + "_Credits", 0);
        text.text = currency.ToString();
    }
}
