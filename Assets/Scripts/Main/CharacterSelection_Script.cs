using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MeloMelo_RPGEditor;
using UnityEngine.UI;

public class CharacterSelection_Script : MonoBehaviour
{
    public static CharacterSelection_Script thisSelect;
    UnitFormation_Management unit = new UnitFormation_Management();

    [SerializeField] private GameObject characterList;
    [SerializeField] private ClassBase DefaultClass;
    [SerializeField] private GameObject CharacterInformationBoard;

    // Start is called before the first frame update
    void Start()
    {
        thisSelect = this;
        GetCharactersInstance();
    }

    #region SETUP
    private void GetCharactersInstance()
    {
        ClassBase[] characters = Resources.LoadAll<ClassBase>("Character_Data");

        for (int character = 0; character < characters.Length; character++)
        {
            if (characters[character] != DefaultClass)
            {
                RawImage characterProfile = Instantiate(Resources.Load<RawImage>("Character_Data/SlotInstance/CharInstance"), characterList.transform);
                characterProfile.transform.GetChild(0).GetComponent<RawImage>().texture = Resources.Load<Texture>("Character_Data/" + characters[character].name);
                characterProfile.GetComponent<CharacterInfo_Script>().Char = characters[character];

                if (Resources.Load<CharacterRestriction>("Character_Data/" + characters[character].name + "_Restrict") != null)
                {
                    characterProfile.transform.GetChild(0).GetComponent<RawImage>().color = new Color(1, 1, 1, 0.55f);
                    characterProfile.transform.GetChild(1).gameObject.SetActive(true);
                    characterProfile.GetComponent<Button>().interactable = false;
                }
            }
        }
    }
    #endregion

    #region MAIN
    public void UnitSlot_Update(string selectionId)
    {
        PlayerPrefs.SetString("recentSelection", selectionId);

        foreach (GameObject character in GameObject.FindGameObjectsWithTag("Slot"))
        {
            if (selectionId != character.GetComponent<CharacterInfo_Script>().Char.characterName)
                character.transform.GetChild(0).GetComponent<RawImage>().color = new Color(1, 1, 1, character.GetComponent<RawImage>().color.a);
        }

        // Set main force character
        unit.SetMainForce(selectionId);

        // Set new assigned character
        if (!GetSlotForCharacterPlacement(selectionId))
        {
            //PlayerPrefs.SetString("Slot" + PlayerPrefs.GetInt("SlotSelect_setup", 1) + "_mainSet", "T");
            PlayerPrefs.SetString("Slot" + PlayerPrefs.GetInt("SlotSelect_setup", 1) + "_charName", selectionId);
        }

        // Set character power level
        PlayerPrefs.SetInt("Slot" + PlayerPrefs.GetInt("SlotSelect_setup", 1) + "_power", 0);

        // Get information about character status
        GetCharacterInfoPlate(selectionId);
    }

    public void ConfrimCharacterPick()
    {
        // Close Scene
        GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Close");
        Invoke("GoBackSetup", 2);
    }
    #endregion

    // Unit Formation: Single Update
    public void UnitSlot_Update(int SlotIndex, string charName, bool mainSet, int power)
    {
        PlayerPrefs.SetString("recentSelection", charName);

        bool check = false;
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetString("Slot" + (i + 1) + "_charName", "None") != PlayerPrefs.GetString("recentSelection", "None")) { check = true; }
            else { check = false; break; }
        }

        if (check)
        {
            PlayerPrefs.SetString("Slot" + SlotIndex + "_charName", charName);

            if (mainSet)
            {
                PlayerPrefs.SetString("Slot" + SlotIndex + "_mainSet", "T");
                unit.SetMainForce(charName);
            }
            else { PlayerPrefs.SetString("Slot" + SlotIndex + "_mainSet", "F"); }

            PlayerPrefs.SetInt("Slot" + SlotIndex + "_power", power);
        }

        // Close Scene
        GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Close");
        Invoke("GoBackSetup", 2);
    }

    #region COMPONENT
    private bool GetSlotForCharacterPlacement(string id)
    {
        bool check = false;

        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetString("Slot" + (i + 1) + "_charName", "None") == id) { check = true; break; } 
            else { check = false; }
        }

        return check;
    }

    private void GoBackSetup() { SceneManager.LoadScene(PlayerPrefs.GetString("SlotSelect_lastSelect", string.Empty)); }

    private void GetCharacterInfoPlate(string character)
    {
        CharacterInformationBoard.transform.GetChild(0).gameObject.SetActive(true);
        CharacterInformationBoard.transform.GetChild(0).GetComponent<Text>().text = character;
        CharacterInformationBoard.transform.GetChild(1).gameObject.SetActive(true);
        CharacterInformationBoard.transform.GetChild(2).gameObject.SetActive(false);
    }
    #endregion
}
