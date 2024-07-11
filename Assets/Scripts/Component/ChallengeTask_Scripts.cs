using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeTask_Scripts : MonoBehaviour
{
    private bool isPicked = false;
    public bool is_Picked { get { return isPicked; } }
    public GameObject[] CoverImage_List;

    #region MAIN
    public void SelectChallenge(bool select)
    {
        isPicked = select;
        GetComponent<RawImage>().color = isPicked ? Color.green : Color.white;
    }

    public void DeSelectOther()
    {
        GameObject.Find("Selection_List").GetComponent<MarathonTask_Script>().DisagreeToAction(string.Empty);
    }
    #endregion
}
