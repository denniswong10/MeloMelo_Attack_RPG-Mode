using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarathonEntry : MonoBehaviour
{
    public Text Title;
    // 0 - Open, 1 - Close, 2 - End, 3 - Free
    public GameObject[] Text_Status = new GameObject[4];
    public GameObject Overall;

    private string userInput = "GUEST";

    void Start()
    {
        try { userInput = LoginPage_Script.thisPage.get_user; } catch { }

        if (userInput != "GUEST") 
        {
            GetComponent<Button>().interactable = true;
            //Invoke("Setup", 1); 
        }
        else { GetComponent<Button>().interactable = true; }
    }

    private void Setup()
    {
        int server = PlayerPrefs.GetInt("serverEnable");

        Overall.SetActive(true);
        GetComponent<Button>().interactable = (true);

        foreach(GameObject i in Text_Status) { i.SetActive(false); }

        switch (server)
        {
            case 1: // Ready
                Text_Status[0].SetActive(true);
                //Text_Status[0].GetComponent<Text>().text = "End in 1 April 2023";
                break;

            case 2: // Error
                Text_Status[3].SetActive(true);
                break;

            default: // None
                Text_Status[3].SetActive(true);
                break;
        }
    }
}
