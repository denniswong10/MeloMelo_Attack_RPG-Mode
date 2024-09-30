using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo_Script : MonoBehaviour
{
    public ClassBase Char;

    public void SelectCharacterWithId() 
    {
        CharacterSelection_Script.thisSelect.UnitSlot_Update(Char.name);
        GameObject.Find("SelectBtn").GetComponent<Button>().interactable = true;
    }
}
