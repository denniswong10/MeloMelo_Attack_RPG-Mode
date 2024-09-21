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
    [SerializeField] private GameObject[] AdditionalSelectionTab;
    [SerializeField] private Texture NoneOfAbove;

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

                if (Resources.Load<CharacterRestriction>("Character_Data/" + characters[character].name + "_Restrict") != null
                    && !GetCharacterUnlockPhase(Resources.Load<CharacterRestriction>("Character_Data/" + characters[character].name + "_Restrict")))
                {
                    characterProfile.transform.GetChild(0).GetComponent<RawImage>().color = new Color(1, 1, 1, 0.55f);
                    characterProfile.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }

    private bool GetCharacterUnlockPhase(CharacterRestriction data_condition)
    {
        bool availableForUse = true;

        if (data_condition)
        {
            foreach (TrackRestriction_Character trackCleared in data_condition.trackCleared)
            {
                if (PlayerPrefs.GetInt(trackCleared.trackName + "_score" + trackCleared.difficulty, 0) < trackCleared.score)
                { availableForUse = false; break; }
            }
        }
        else
            return false;

        return availableForUse;
    }
    #endregion

    #region MAIN
    public void CharacterToggle_Status()
    {
        PlayerPrefs.SetInt("CharacterSelection_ToggleTab", 0);
        GetCharacterTabSelection(true);

        GetCharacterInfoPlate(true, PlayerPrefs.GetString("recentSelection", string.Empty));
        GetCharacterSkillInfoPlate(false, PlayerPrefs.GetString("recentSelection", string.Empty));
    }

    public void CharacterToggle_Skills()
    {
        PlayerPrefs.SetInt("CharacterSelection_ToggleTab", 1);
        GetCharacterTabSelection(true);

        GetCharacterInfoPlate(false, PlayerPrefs.GetString("recentSelection", string.Empty));
        GetCharacterSkillInfoPlate(true, PlayerPrefs.GetString("recentSelection", string.Empty));
    }

    public void UnitSlot_Update(string selectionId)
    {
        // Current selection of character
        PlayerPrefs.SetString("recentSelection", selectionId);

        bool isCharacterAvailable = Resources.Load<CharacterRestriction>("Character_Data/" + selectionId + "_Restrict") == null ||
            (Resources.Load<CharacterRestriction>("Character_Data/" + selectionId + "_Restrict") != null &&
            GetCharacterUnlockPhase(Resources.Load<CharacterRestriction>("Character_Data/" + selectionId + "_Restrict")));

        GetCharacterUnlockInfo(isCharacterAvailable, selectionId);

        if (isCharacterAvailable)
        {
            // Focus character slot when selected
            foreach (GameObject character in GameObject.FindGameObjectsWithTag("Slot"))
            {
                if (selectionId != character.GetComponent<CharacterInfo_Script>().Char.characterName)
                    character.transform.GetChild(0).GetComponent<RawImage>().color = new Color(1, 1, 1, character.GetComponent<RawImage>().color.a);
            }
        }

        // Get information about character
        GetCharacterInfoPlate(isCharacterAvailable && PlayerPrefs.GetInt("CharacterSelection_ToggleTab", 0) == 0 ? true : false, selectionId);
        GetCharacterSkillInfoPlate(isCharacterAvailable && PlayerPrefs.GetInt("CharacterSelection_ToggleTab", 0) == 1 ? true : false, selectionId);
    }

    public void ConfrimCharacterPick(bool isPickedSuccess)
    {
        // Cannot process when character is not available
        if (Resources.Load<CharacterRestriction>("Character_Data/" + PlayerPrefs.GetString("recentSelection", "None") + "_Restrict") != null
            && GetCharacterUnlockPhase(Resources.Load<CharacterRestriction>("Character_Data/" + PlayerPrefs.GetString("recentSelection", "None") + "_Restrict"))
            || Resources.Load<CharacterRestriction>("Character_Data/" + PlayerPrefs.GetString("recentSelection", "None") + "_Restrict") == null)
        {
            // Save changes on confirm picked character
            if (isPickedSuccess)
            {
                // Set main force character
                unit.SetMainForce(PlayerPrefs.GetString("recentSelection", "None"));

                // Set new assigned character
                if (!GetSlotForCharacterOccupied(PlayerPrefs.GetString("recentSelection", "None")))
                {
                    PlayerPrefs.SetString("Slot" + PlayerPrefs.GetInt("SlotSelect_setup", 1) + "_charName",
                        PlayerPrefs.GetString("recentSelection", "None"));
                }

                // Set character power level
                PlayerPrefs.SetInt("Slot" + PlayerPrefs.GetInt("SlotSelect_setup", 1) + "_power", 0);
            }

            // Close Scene
            GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Close");
            Invoke("GoBackSetup", 2);
        }

        else if (!isPickedSuccess)
        {
            // Close Scene
            GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Close");
            Invoke("GoBackSetup", 2);
        }
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
    private bool GetSlotForCharacterOccupied(string characterName)
    {
        bool isPickDupliate = false;

        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetString("Slot" + (i + 1) + "_charName", "None") == characterName) { isPickDupliate = true; break; } 
            else { isPickDupliate = false; }
        }

        return isPickDupliate;
    }

    private void GetCharacterUnlockInfo(bool active, string character)
    {
        CharacterRestriction info = Resources.Load<CharacterRestriction>("Character_Data/" + character + "_Restrict");
        if (info) CharacterInformationBoard.transform.GetChild(2).GetComponent<Text>().text = "Unlock Hint: \n\n" + info.description;
        CharacterInformationBoard.transform.GetChild(2).gameObject.SetActive(!active);
        GetCharacterTabSelection(active);
    }

    private void GetCharacterInfoPlate(bool active, string character)
    {
        CharacterInformationBoard.transform.GetChild(0).gameObject.SetActive(active);
        CharacterInformationBoard.transform.GetChild(0).GetComponent<Text>().text = 
            Resources.Load<ClassBase>("Character_Data/" + character).characterName + " (" + character + ")";
        CharacterInformationBoard.transform.GetChild(1).gameObject.SetActive(active);

        // Features: Status and Skills
        AdditionalSelectionTab[PlayerPrefs.GetInt("CharacterSelection_ToggleTab", 0)].GetComponent<RawImage>().color = Color.green;
    }

    private void GetCharacterSkillInfoPlate(bool active, string character)
    {
        const int startOfPlate = 5;

        for (int togglePlate = startOfPlate; togglePlate < CharacterInformationBoard.transform.childCount; togglePlate++)
            CharacterInformationBoard.transform.GetChild(togglePlate).gameObject.SetActive(active);

        // Features: Skill Checker
        SkillContainer primarySkill = Resources.Load<SkillContainer>("Database_Skills/" + character + "_Primary_skill");
        if (primarySkill) CharacterInformationBoard.transform.GetChild(startOfPlate + 1).GetComponent<RawImage>().texture = primarySkill.skillIcon;
        else CharacterInformationBoard.transform.GetChild(startOfPlate + 1).GetComponent<RawImage>().texture = NoneOfAbove;

        List<SkillContainer> secondarySkills = new List<SkillContainer>();
        for (int skill_id = 0; skill_id < 3; skill_id++)
        {
            SkillContainer checkForSkill = Resources.Load<SkillContainer>("Database_Skills/" + character + "_Secondary_Skill_" + skill_id);
            if (checkForSkill) secondarySkills.Add(checkForSkill);
        }

        for (int ulimateSkill = 0; ulimateSkill < secondarySkills.ToArray().Length; ulimateSkill++)
        {
            if (PlayerPrefs.GetInt(secondarySkills[ulimateSkill].skillName + "_Unlock_Code", 0) == 1)
                CharacterInformationBoard.transform.GetChild(startOfPlate + 2 + ulimateSkill).GetComponent<RawImage>().texture =
                    secondarySkills[ulimateSkill].skillIcon;
            else
                CharacterInformationBoard.transform.GetChild(startOfPlate + 2 + ulimateSkill).GetComponent<RawImage>().texture =
                    NoneOfAbove;
        }
    }

    private void GetCharacterTabSelection(bool active)
    {
        // Features: Status and Skills
        foreach (GameObject selectionTab in AdditionalSelectionTab)
        {
            selectionTab.SetActive(active);
            selectionTab.GetComponent<RawImage>().color = Color.white;
        }
    }

    private void GoBackSetup() { SceneManager.LoadScene(PlayerPrefs.GetString("SlotSelect_lastSelect", string.Empty)); }
    #endregion
}
