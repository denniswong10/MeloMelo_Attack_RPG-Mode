using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_RPGEditor;

[SerializeField]
public class OpenReadableChoice
{
    public string title;
    public string choice;
}

public class OpenChoiceLocal
{
    public string title;
    public List<string> choices;

    public OpenChoiceLocal(string title)
    {
        this.title = title;
        choices = new List<string>();
    }
}

public class OpenChoiceSetupScript : MonoBehaviour
{
    [SerializeField] private GameObject itemContent;
    private GameObject promptMessage;

    private string itemName;
    private string chosenCharacter;

    private List<int> choiceOptionListing = null;
    private int usageOfProgram;
    private List<OpenChoiceLocal> createChoicePath = null;
    private List<OpenReadableChoice> createReadablePath = null;

    #region SETUP
    private void CreateAccess()
    {
        choiceOptionListing = new List<int>();
        createChoicePath = new List<OpenChoiceLocal>();
        createReadablePath = new List<OpenReadableChoice>();
        Debug.Log("Begin to used choice setup...");
    }

    public void Setup(int startingCall, UsageOfItemDetail reference, string characterRef)
    {
        usageOfProgram = startingCall;
        itemName = reference.itemName;
        chosenCharacter = characterRef;
        CreateAccess();

        if (createReadablePath != null)
        {
            string[] data = reference.dataArray.Split("@");
            foreach (string info in data)
                try { createReadablePath.Add(JsonUtility.FromJson<OpenReadableChoice>(info)); Debug.Log(info); } catch { break; }
        }

        if (createChoicePath != null)
        {
            foreach (OpenReadableChoice gatheredInfo in createReadablePath)
            {
                OpenChoiceLocal readyToAssgin = new OpenChoiceLocal(gatheredInfo.title);
                string[] createChoicePick = gatheredInfo.choice.Split("*");
                foreach (string assignChoice in createChoicePick) readyToAssgin.choices.Add(assignChoice);
                createChoicePath.Add(readyToAssgin);
            }
        }

        Invoke("UpdatePromptInput", 0.1f);
    }

    public void SetupPromptMessage(GameObject messageTab)
    {
        promptMessage = messageTab;
    }

    public void UpdatePromptInput()
    {
        int currentTarget = choiceOptionListing != null ? choiceOptionListing.ToArray().Length : 0;

        if (currentTarget != createChoicePath.ToArray().Length)
        {
            int currentPickIndex = 0;
            PlayerPrefs.SetInt("ConfirmPick_OpenChoiceSetup", 1);
            transform.GetChild(3).gameObject.SetActive(currentTarget + 1 != createChoicePath.ToArray().Length);

            transform.GetChild(0).GetComponent<Text>().text = "[ " + GetCharacterIdentify(chosenCharacter).characterName + " ] " 
                + createChoicePath[currentTarget].title;

            foreach (string choiceForPick in createChoicePath[currentTarget].choices)
            {
                currentPickIndex++;
                GameObject choiceInstance = Instantiate(itemContent.transform.GetChild(0).gameObject, itemContent.transform);
                choiceInstance.name = currentPickIndex.ToString();
                choiceInstance.transform.GetChild(0).GetComponent<Text>().text = choiceForPick;
                choiceInstance.SetActive(true);
            }
        }
        else
            GetProgramCaller(usageOfProgram);
    }

    private void ConfirmItemUsage()
    {
        MeloMelo_ItemUsage_Settings.SetItemUsed(itemName);
    }
    #endregion

    #region MAIN
    public void ClosePanel()
    {
        Destroy(gameObject);
    }

    public void ConfirmPick(GameObject template)
    {
        if (PlayerPrefs.HasKey("ConfirmPick_OpenChoiceSetup"))
        {
            PlayerPrefs.DeleteKey("ConfirmPick_OpenChoiceSetup");
            if (choiceOptionListing != null) choiceOptionListing.Add(int.Parse(template.name));

            for (int instance = 1; instance < itemContent.transform.childCount; instance++)
                Destroy(itemContent.transform.GetChild(instance).gameObject);
        }

        Invoke("UpdatePromptInput", 0.1f);
    }
    #endregion

    #region COMPONENT
    private void GetProgramCaller(int usageOfProgram)
    {
        switch (usageOfProgram)
        {
            case 1:
                CharacterOnStatsReset();
                break;

            case 2:
                CharacterSkillFunction(usageOfProgram, 0);
                break;

            case 3:
                CharacterSkillFunction(usageOfProgram, choiceOptionListing[1]);
                break;

            case 4:
                CharacterManualTraining(5);
                break;

            case 5:
                CharacterManualTraining(30);
                break;

            case 6:
                CharacterManualTraining(70);
                break;

            case 7:
                CharacterLevelingTraining(1);
                break;

            case 8:
                CharacterInstantUpgrade("STR,MAG,VIT", 5);
                break;

            case 9:
                CharacterInstantUpgrade("MAG", 5);
                break;

            case 10:
                CharacterInstantUpgrade("VIT", 5);
                break;

            case 11:
                CharacterInstantUpgrade("STR", 5);
                break;

            default:
                StartCoroutine(MessagePopUp("Item is not available at this moment"));
                Invoke("ClosePanel", 3.1f);
                break;
        }
    }

    private void CharacterSkillFunction(int tierGroup, int skill_index)
    {
        if (choiceOptionListing[0] == 1)
        {
            if (MeloMelo_CharacterInfo_Settings.GetCharacterStatus(chosenCharacter))
            {
                switch (tierGroup)
                {
                    case 2:
                        if (MeloMelo_SkillData_Settings.CheckSkillStatus(
                            Resources.Load<SkillContainer>("Database_Skills/" + chosenCharacter + "_Priamry_Skill").skillName))
                            MeloMelo_SkillData_Settings.LearnSkill(Resources.Load<SkillContainer>("Database_Skills/" + chosenCharacter + "_Priamry_Skill").skillName);
                        else
                            MeloMelo_SkillData_Settings.UpgradeSkill(Resources.Load<SkillContainer>("Database_Skills/" + chosenCharacter + "_Priamry_Skill").skillName);
                        break;

                    case 3:
                        if (MeloMelo_SkillData_Settings.CheckSkillStatus(
                            Resources.Load<SkillContainer>("Database_Skills/" + chosenCharacter + "_Secondary_Skill_" + skill_index).skillName))
                            MeloMelo_SkillData_Settings.LearnSkill(Resources.Load<SkillContainer>("Database_Skills/" + chosenCharacter + "_Secondary_Skill_" + skill_index).skillName);
                        else
                            MeloMelo_SkillData_Settings.UpgradeSkill(Resources.Load<SkillContainer>("Database_Skills/" + chosenCharacter + "_Secondary_Skill_" + skill_index).skillName);
                        break;

                    default:
                        Invoke("ClosePanel", 3.1f);
                        break;
                }
            }
            else
                StartCoroutine(MessagePopUp("Character need to be unlock before using any consumable"));

            ConfirmItemUsage();
            try { StartCoroutine(MessagePopUp("Character just learned " + Resources.Load<SkillContainer>("Database_Skills/" + chosenCharacter + "_Priamry_Skill").skillName)); }
            catch { StartCoroutine(MessagePopUp("Character just learned " + Resources.Load<SkillContainer>("Database_Skills/" + chosenCharacter + "_Secondary_Skill_" + skill_index).skillName)); }
        }
        else
            StartCoroutine(MessagePopUp("Ticket hasn't been used after rejecting"));
    }

    private void CharacterManualTraining(int percentage)
    {
        if (choiceOptionListing[0] == 1)
        {
            StatsManage_Database getInfo = new StatsManage_Database(chosenCharacter);
            GetCharacterIdentify(chosenCharacter).UpdateCurrentStats(false);
            int maxExperience = getInfo.GetCharacterStatus(GetCharacterIdentify(chosenCharacter).level).GetExperience;
            float amountPerUnit = (maxExperience < 0 ? 0 : maxExperience) * 0.01f;

            if (maxExperience > 0)
            {
                GetCharacterIdentify(chosenCharacter).experience += (int)(amountPerUnit * percentage);
                GetCharacterIdentify(chosenCharacter).UpdateCurrentStats(true);

                ConfirmItemUsage();
                StartCoroutine(MessagePopUp("Character gained " + (amountPerUnit * percentage) + " experience during training"));
            }
            else
                StartCoroutine(MessagePopUp("Character level have reached its limit during training"));
        }
        else
            StartCoroutine(MessagePopUp("Ticket hasn't been used after rejecting"));

        Invoke("ClosePanel", 3.1f);
    }

    private void CharacterLevelingTraining(int amount)
    {
        if (choiceOptionListing[0] == 1)
        {
            StatsManage_Database getInfo = new StatsManage_Database(chosenCharacter);
            GetCharacterIdentify(chosenCharacter).UpdateCurrentStats(false);
            int currentLevel = GetCharacterIdentify(chosenCharacter).level;

            if (currentLevel < getInfo.GetCharacterStatus(GetCharacterIdentify(chosenCharacter).level + 1).GetLevel)
            {
                GetCharacterIdentify(chosenCharacter).level += amount;
                GetCharacterIdentify(chosenCharacter).UpdateCurrentStats(true);

                ConfirmItemUsage();
                StartCoroutine(MessagePopUp("Character gained " + amount + " level during training"));
            }
            else
                StartCoroutine(MessagePopUp("Character level have reached its limit during training"));
        }
        else
            StartCoroutine(MessagePopUp("Ticket hasn't been used after rejecting"));

        Invoke("ClosePanel", 3.1f);
    }

    private void CharacterOnStatsReset()
    {
        /*
         1st: Stats to be reset
         2nd: Stats to be re-assign
         3rd: Confirmation use
         */
        if (choiceOptionListing[2] == 1)
        {
            if (choiceOptionListing[0] == choiceOptionListing[1])
                StartCoroutine(MessagePopUp("Stats modified repeated"));

            else if (ModifyCharacterStats(GetStatsFromIndex(choiceOptionListing[0]), true, 1))
            {
                ModifyCharacterStats(GetStatsFromIndex(choiceOptionListing[1]), false, 1);
                ConfirmItemUsage();
                StartCoroutine(MessagePopUp("Stats has been updated"));
            }
            else
                StartCoroutine(MessagePopUp("Minimum of 1 stats point require to reset"));
        }
        else
            StartCoroutine(MessagePopUp("Item hasn't been used after rejecting"));

        Invoke("ClosePanel", 3.1f);
    }

    private void CharacterInstantUpgrade(string allStatsArray, int amount)
    {
        if (choiceOptionListing[2] == 1)
        {
            string[] assignStats = allStatsArray.Split(",");
            foreach (string stats in assignStats)
            {
                switch (stats)
                {
                    case "STR":
                        ModifyCharacterStats("STR", false, amount);
                        break;

                    case "VIT":
                        ModifyCharacterStats("VIT", false, amount);
                        break;

                    case "MAG":
                        ModifyCharacterStats("MAG", false, amount);
                        break;

                    default:
                        break;
                }
            }

            if (assignStats.Length > 0) { StartCoroutine(MessagePopUp("Succesful of used book")); }
            else { StartCoroutine(MessagePopUp("Book attribute empty")); }
        }
        else
            StartCoroutine(MessagePopUp("Cancel used of book"));
    }

    private bool ModifyCharacterStats(string typeOfStats, bool reset, int amount)
    {
        switch (typeOfStats)
        {
            case "STR":
                if (MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(chosenCharacter) <= 0 && reset) return false;
                MeloMelo_ExtraStats_Settings.IncreaseStrengthStats(chosenCharacter, reset ? -amount : amount);
                break;

            case "VIT":
                if (MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(chosenCharacter) <= 0 && reset) return false;
                MeloMelo_ExtraStats_Settings.IncreaseVitalityStats(chosenCharacter, reset ? -amount : amount);
                break;

            case "MAG":
                if (MeloMelo_ExtraStats_Settings.GetExtraMagicStats(chosenCharacter) <= 0 && reset) return false;
                MeloMelo_ExtraStats_Settings.IncreaseMagicStats(chosenCharacter, reset ? -amount : amount);
                break;
        }

        return true;
    }

    private string GetStatsFromIndex(int index)
    {
        switch (index)
        {
            case 1:
                return "STR";

            case 2:
                return "VIT";

            case 3:
                return "MAG";

            default:
                return string.Empty;
        }
    }

    private ClassBase GetCharacterIdentify(string className)
    {
        return Resources.Load<ClassBase>("Character_Data/" + className);
    }
    #endregion

    #region MISC
    private IEnumerator MessagePopUp(string customText)
    {
        if (promptMessage)
        {
            promptMessage.SetActive(true);
            promptMessage.transform.GetChild(0).GetComponent<Text>().text = customText;
            yield return new WaitForSeconds(2);
            promptMessage.SetActive(false);
        }
    }
    #endregion
}
