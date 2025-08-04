using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingContent_Script : MonoBehaviour
{
    [SerializeField] private Text UI_Text;

    #region MAIN
    public void NowLoading(string text)
    {
        UI_Text.text = text;
        gameObject.SetActive(true);
    }

    public void DoneLoading()
    {
       gameObject.SetActive(false);
    }
    #endregion
}
