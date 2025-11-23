using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorText_Script : MonoBehaviour
{
    private Color32 colorIndicate;
    public string index;
    public bool earlyNLate;

    void Start()
    {
        //if (index != "Miss" && !earlyNLate) { GetComponent<TextMesh>().text = index + " x" + GameManager.thisManager.getJudgeWindow.getCombo; }
        //else { GetComponent<TextMesh>().text = index; }
    }

    void Update()
    {
        //if (transform.position.y > 1)
        //{
        //    Destroy(gameObject);
        //}
        //else { transform.Translate(Vector3.up * Time.deltaTime, Space.World); }

        RunIndicator();
    }

    #region SETUP
    public void UpdateJudgementStatus(string judge_index)
    {
        switch (judge_index)
        {
            case "Perfect_2":
                index = "Critical" + " x" + GameManager.thisManager.getJudgeWindow.getCombo;
                colorIndicate = new Color32(248, 235, 11, 255);
                break;

            case "Perfect":
                index = judge_index + " x" + GameManager.thisManager.getJudgeWindow.getCombo;
                colorIndicate = new Color32(15, 130, 9, 255);
                break;

            case "Bad":
                index = judge_index + " x" + GameManager.thisManager.getJudgeWindow.getCombo;
                colorIndicate = new Color32(113, 111, 111, 255);
                break;

            default:
                index = judge_index;
                colorIndicate = Color.red;
                break;
        }

        GetComponent<TextMesh>().fontSize = 10;
        GetComponent<TextMesh>().text = index;
        GetComponent<TextMesh>().color = colorIndicate;
    }

    public void UpdateMarginErrorStatus(int margin_index)
    {
        switch (margin_index)
        {
            case 0:
                index = "Early";
                colorIndicate = new Color32(11, 30, 248, 255);
                break;

            case 1:
                index = "Perfect";
                colorIndicate = new Color32(248, 255, 11, 255);
                break;

            default:
                index = "Late";
                colorIndicate = new Color32(248, 11, 16, 255);
                break;
        }

        GetComponent<TextMesh>().fontSize = 8;
        GetComponent<TextMesh>().text = index;
        GetComponent<TextMesh>().color = colorIndicate;
    }

    public void RunIndicator()
    {
        if (gameObject.activeInHierarchy)
        {
            if (transform.position.y > 1) GameManager.thisManager.getInGameObjectWindow.judgeIndicator.ReturnPopUpText(gameObject);
            else transform.Translate(Vector3.up * Time.deltaTime, Space.World);
        }
    }
    #endregion
}
