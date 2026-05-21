using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarathonRegistration_Script : MonoBehaviour
{
    [SerializeField] private Button add_button;

    #region MAIN
    public void Add()
    {

    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region MISC
    public void UpdateEventCodeInput(InputField input)
    {
        add_button.interactable = input.text != string.Empty;
    }
    #endregion
}
