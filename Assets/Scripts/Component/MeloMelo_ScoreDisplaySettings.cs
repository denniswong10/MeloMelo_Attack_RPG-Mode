using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeloMelo_ScoreDisplaySettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DisplayScore();
        DisplayScore2();
    }

    private void DisplayScore()
    {
        switch(PlayerPrefs.GetInt("ScoreDisplay", 0))
        {
            case 1:
                transform.GetChild(1).gameObject.SetActive(true);
                break;

            case 2:
                transform.GetChild(3).gameObject.SetActive(true);
                break;

            case 3:
                transform.GetChild(2).gameObject.SetActive(true);
                break;

            case 4:
                transform.GetChild(4).gameObject.SetActive(true);
                break;

            case 5:
                transform.GetChild(5).gameObject.SetActive(true);
                break;

            default:
                transform.GetChild(0).gameObject.SetActive(true);
                break;
        }
    }

    private void DisplayScore2()
    {
        switch(PlayerPrefs.GetInt("ScoreDisplay2", 0))
        {
            case 0:
                break;

            default:
                transform.GetChild(6).gameObject.SetActive(true);
                break;
        }
    }
}
