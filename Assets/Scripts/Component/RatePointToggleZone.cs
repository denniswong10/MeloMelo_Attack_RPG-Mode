using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatePointToggleZone : MonoBehaviour
{
    [SerializeField] private GameObject ratePointToggle;

    private float NextTimeToggle = 0;
    private int[] counter = { 9999, 999, 99, 9, 1 };
    private int current = 0;
    private int number = 0;
    private bool isDoneCount = false;

    #region SETUP
    #endregion

    #region MAIN
    public void GetZoneFunctionProgram()
    {
        if (ratePointToggle.activeInHierarchy)
        {
            if (Time.time >= NextTimeToggle && number < current)
            {
                NextTimeToggle = Time.time + 0.1f;

                for (int i = 0; i < counter.Length; i++)
                    if (number + counter[i] <= current) number += counter[i];

                ratePointToggle.transform.GetChild(0).GetComponent<Text>().text = number.ToString();
            }

            if (number >= current && !isDoneCount)
            {
                isDoneCount = true;
                PlayerPrefs.SetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "UserRatePointToggle", number);
                Invoke("CloseRatePointToogle", 2);
            }
        }
    }
    #endregion

    #region COMPONENT
    private void CloseRatePointToogle()
    {
        ratePointToggle.SetActive(false);
        ResultMenu_Script.thisRes.CloseRatePointToogle(1);
    }
    #endregion

    #region MISC
    public void ProcessToRateZone(string user, string storageId)
    {
        isDoneCount = false;
        NextTimeToggle = 0;
        current = PlayerPrefs.GetInt(user + storageId, 0);
        number = 0;
        ratePointToggle.SetActive(true);
    }
    #endregion
}
