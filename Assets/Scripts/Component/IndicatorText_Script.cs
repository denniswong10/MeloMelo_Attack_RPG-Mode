using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorText_Script : MonoBehaviour
{
    public string index;
    public bool earlyNLate;

    void Start()
    {
        if (index != "Miss" && !earlyNLate) { GetComponent<TextMesh>().text = index + " x" + GameManager.thisManager.getJudgeWindow.getCombo; }
        else { GetComponent<TextMesh>().text = index; }
    }

    void Update()
    {
        if (transform.position.y > 1)
        {
            Destroy(gameObject);
        }
        else { transform.Translate(Vector3.up * Time.deltaTime, Space.World); }
    }
}
