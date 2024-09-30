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
        // Get all characters which are store in files
        ClassBase[] characters = Resources.LoadAll<ClassBase>("Character_Data");

        // Perform check on characters status (LOCKED, UNLOCKED)
        for (int character = 0; character < characters.Length; character++)
        {
            // Character doesn't perform non-class type then process
            if (characters[character] != DefaultClass)
            {
                // Create slot for character and update the content
                RawImage characterProfile = Instantiate(Resources.Load<RawImage>("Character_Data/SlotInstance/CharInstance"), characterList.transform);
                characterProfile.transform.GetChild(0).GetComponent<RawImage>().texture = Resources.Load<Texture>("Character_Data/" + characters[character].name);
                characterProfile.GetComponent<CharacterInfo_Script>().Char = characters[character];

                // Check for character status is not available to use
                CreateCharacterConditionUponPick(characters[character].name);

                characterProfile.transform.GetChild(0).GetComponent<RawImage>().color = MeloMelo_CharacterInfo_Settings.GetCharacterChosenSelection() ?
                        new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.55f);
                characterProfile.transform.GetChild(1).gameObject.SetActive(!MeloMelo_CharacterInfo_Settings.GetCharacterChosenSelection());
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
    public void UpdateCheckForPrimarySkill()
    {

    }

    public void UpdateCheckForSecondarySkill(int id)
    {
        // Get container on update a change of secondary skill
        SkillContainer checkForSkill = Resources.Load<SkillContainer>("Database_Skills/" +
            PlayerPrefs.GetString("recentSelection", string.Empty) + "_Secondary_Skill_" + id);

        // Process to find skill is able to be chosen
        if (checkForSkill && (MeloMelo_SkillData_Settings.CheckSkillStatus(checkForSkill.skillName) || checkForSkill.isUnlockReady))
        {
            // Confirm of secondary skill change
            MeloMelo_CharacterInfo_Settings.SetUsageOfSecondarySkill(PlayerPrefs.GetString("recentSelection", string.Empty), id);

            // Update the mark display shown that the player have already chosen this change
            for (int index = 1; index < 4; index++) GameObject.Find("Skill_B" + index).transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("Skill_B" + id).transform.GetChild(0).gameObject.SetActive(true);
        }
    }

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

        // Update character condition selection and get character information
        CreateCharacterConditionUponPick(PlayerPrefs.GetString("recentSelection", string.Empty));
        GetCharacterUnlockInfo(MeloMelo_CharacterInfo_Settings.GetCharacterChosenSelection(), selectionId);

        // Focus select: Light only character are currently on pick
        foreach (GameObject character in GameObject.FindGameObjectsWithTag("Slot"))
        {
            if (selectionId == character.GetComponent<CharacterInfo_Script>().Char.name
                && MeloMelo_CharacterInfo_Settings.GetCharacterChosenSelection())
                character.transform.GetChild(0).GetComponent<RawImage>().color = new Color(1, 1, 1, 1);

            else if (!MeloMelo_CharacterInfo_Settings.GetCharacterChosenSelection())
                character.transform.GetChild(0).GetComponent<RawImage>().color = new Color(1, 1, 1, 0.55f);

            else
                character.transform.GetChild(0).GetComponent<RawImage>().color = new Color(1, 1, 1, 0.55f);
        }

        // Get information about character (Main, Skills)
        GetCharacterInfoPlate(MeloMelo_CharacterInfo_Settings.GetCharacterChosenSelection() &&
            PlayerPrefs.GetInt("CharacterSelection_ToggleTab", 0) == 0 ? true : false, selectionId);

        GetCharacterSkillInfoPlate(MeloMelo_CharacterInfo_Settings.GetCharacterChosenSelection() &&
            PlayerPrefs.GetInt("CharacterSelection_ToggleTab", 0) == 1 ? true : false, selectionId);
    }

    public void ConfrimCharacterPick(bool isPickedSuccess)
    {
        // Cannot process when character is not available
        if (MeloMelo_CharacterInfo_Settings.GetCharacterChosenSelection())
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
        // Find character restricted information
        CharacterRestriction info = Resources.Load<CharacterRestriction>("Character_Data/" + character + "_Restrict");

        // Check for information about unlock state if available
        if (info) CharacterInformationBoard.transform.GetChild(2).GetComponent<Text>().text = "Unlock Hint: \n\n" + info.description;
        CharacterInformationBoard.transform.GetChild(2).gameObject.SetActive(!active);
        GetCharacterTabSelection(active);
    }

    private void GetCharacterInfoPlate(bool active, string character)
    {
        // Title: Update character name and class type
        CharacterInformationBoard.transform.GetChild(0).gameObject.SetActive(active);
        CharacterInformationBoard.transform.GetChild(0).GetComponent<Text>().text =
            Resources.Load<ClassBase>("Character_Data/" + character).characterName + " (" + character + ")";

        // Script: Calculate unit power
        StatsDistribution characterStats = new StatsDistribution();
        characterStats.load_Stats();

        // Body: Update character power and assigned power
        CharacterInformationBoard.transform.GetChild(1).gameObject.SetActive(active);
        CharacterInformationBoard.transform.GetChild(1).GetComponent<Text>().text =
            "Current Unit Power: " + characterStats.get_UnitPower(character) + "\n" +
            "Assigned Character Power: " + (characterStats.get_UnitPower() - characterStats.get_UnitPower(character)) + 
            "(+" + characterStats.get_UnitPower(character) + ")\n" +
            "Total Unit Power: " + characterStats.get_UnitPower();

        // Features: Status and Skills
        AdditionalSelectionTab[PlayerPrefs.GetInt("CharacterSelection_ToggleTab", 0)].GetComponent<RawImage>().color = Color.green;
    }

    private void GetCharacterSkillInfoPlate(bool active, string character)
    {
        // Begin from the area where character information skill contain
        const int startOfPlate = 5;

        // Get all skill information onto display
        for (int togglePlate = startOfPlate; togglePlate < CharacterInformationBoard.transform.childCount; togglePlate++)
            CharacterInformationBoard.transform.GetChild(togglePlate).gameObject.SetActive(active);

        // Single Skill: Only 1 slot
        SkillContainer primarySkill = Resources.Load<SkillContainer>("Database_Skills/" + character + "_Primary_skill");
        if (primarySkill && primarySkill.isUnlockReady)
            CharacterInformationBoard.transform.GetChild(startOfPlate + 1).GetComponent<RawImage>().texture = primarySkill.skillIcon;
        else
            CharacterInformationBoard.transform.GetChild(startOfPlate + 1).GetComponent<RawImage>().texture = NoneOfAbove;

        // Multiple Skill: Up to 3 slots
        List<SkillContainer> secondarySkills = new List<SkillContainer>();
        for (int skill_id = 1; skill_id < 4; skill_id++)
        {
            SkillContainer checkForSkill = Resources.Load<SkillContainer>("Database_Skills/" + character + "_Secondary_Skill_" + skill_id);
            if (checkForSkill) secondarySkills.Add(checkForSkill);
        }

        // Multiple Skill Process: For inspection where skill are available for use
        for (int ulimateSkill = 0; ulimateSkill < secondarySkills.ToArray().Length; ulimateSkill++)
        {
            // Skill are available for use
            if (MeloMelo_SkillData_Settings.CheckSkillStatus(secondarySkills[ulimateSkill].skillName) || secondarySkills[ulimateSkill].isUnlockReady)
            {
                CharacterInformationBoard.transform.GetChild(startOfPlate + 2 + ulimateSkill).GetComponent<RawImage>().texture =
                        secondarySkills[ulimateSkill].skillIcon;

                if (ulimateSkill + 1 == MeloMelo_CharacterInfo_Settings.GetUsageOfSecondarySkill(character))
                    CharacterInformationBoard.transform.GetChild(startOfPlate + 2 + ulimateSkill).transform.GetChild(0).gameObject.SetActive(true);
            }

            // Skill are not available and remain unknown
            else
            {
                CharacterInformationBoard.transform.GetChild(startOfPlate + 2 + ulimateSkill).GetComponent<RawImage>().texture =
                    NoneOfAbove;

                if (CharacterInformationBoard.transform.GetChild(startOfPlate + 2 + ulimateSkill).transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    MeloMelo_CharacterInfo_Settings.SetUsageOfSecondarySkill(character, 0);
                    CharacterInformationBoard.transform.GetChild(startOfPlate + 2 + ulimateSkill).transform.GetChild(0).gameObject.SetActive(false);
                }
            }
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

    #region MISC
    private void CreateCharacterConditionUponPick(string className)
    {
        // Character contain non-restrict assets or unlock code
        bool isCharacterAvailableByDefault = Resources.Load<CharacterRestriction>("Character_Data/" + className + "_Restrict") == null
            || MeloMelo_CharacterInfo_Settings.GetCharacterStatus(className);

        // Character met condition for selection
        bool isCharacrterUnlockedByCondition = Resources.Load<CharacterRestriction>("Character_Data/" + className + "_Restrict") != null &&
            GetCharacterUnlockPhase(Resources.Load<CharacterRestriction>("Character_Data/" + className + "_Restrict"));

        // Update chosen selection
        MeloMelo_CharacterInfo_Settings.SetCharacterChosenSelection(isCharacterAvailableByDefault || isCharacrterUnlockedByCondition);
    }
    #endregion
}
