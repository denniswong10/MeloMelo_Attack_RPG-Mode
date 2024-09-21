using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterNominatorList
{
    public GameObject Characters_Ref;
    public string Char_Name;
};

public class CharacterToggle : MonoBehaviour
{
    public RawImage CharIcon_Ref;
    public GameObject NotAssign_Ref;
    public CharacterNominatorList[] CharacterListing;

    private int mainSet = 0;
    private string CharName_Ref = "NotAssign";

    // Start is called before the first frame update
    void Start()
    {
        SearchMainCharacter();
        AssignCharacter();
    }

    void SearchMainCharacter()
    {
        for (int i = 1; i < 4; i++)
            if (PlayerPrefs.GetString("Slot" + i + "_mainSet", "F") == "T")
                mainSet = i;
    }

    void AssignCharacter()
    {
        //CharName_Ref = PlayerPrefs.GetString("Slot" + mainSet + "_charName", "Warrior");
        CharName_Ref = PlayerPrefs.GetString("CharacterFront", "None");

        if (CharName_Ref == "None") { NotAssign_Ref.SetActive(true); }

        foreach (CharacterNominatorList i in CharacterListing)
        {
            if (i.Char_Name == CharName_Ref)
            {
                i.Characters_Ref.SetActive(true);
                CharIcon_Ref.texture = Resources.Load<Texture>("Character_Data/" + i.Char_Name);
            }
        }
    }
}
