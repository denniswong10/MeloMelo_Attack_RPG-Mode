using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryProfileSlot : MonoBehaviour
{
    #region MAIN
    public void SelectSlot()
    {
        PlayerPrefs.SetString("SelectedGuestEntry", transform.GetChild(0).GetComponent<Text>().text);
        GuestLogin_Script.thisScript.UpdateGuestEntryName(PlayerPrefs.GetString("SelectedGuestEntry"));
    }
    #endregion
}
