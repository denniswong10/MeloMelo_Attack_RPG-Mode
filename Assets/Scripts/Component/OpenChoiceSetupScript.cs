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
                CharacterInstantUpgrade("STR,MAG,VIT", 50);
                break;

            case 10:
                CharacterInstantUpgrade("VIT", 5);
                break;

            case 11:
                CharacterInstantUpgrade("STR", 5);
                break;

            case 12:
                int selectedChoice = choiceOptionListing[0];
                RestoreAdventureProgressPlay(createChoicePath[0].choices[selectedChoice - 1]);
                break;

            case 13:
                int choiceConfirmation = choiceOptionListing[2];

                CharacterInstantUpgrade(
                    createChoicePath[0].choices[choiceOptionListing[0] - 1], 
                    int.Parse(createChoicePath[1].choices[choiceOptionListing[1] - 1]),
                    choiceConfirmation
                    );
                break;

            default:
                AddMessageToPopUp("Item is not available at this moment");
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
                        SkillContainer primary_Skill = Resources.Load<SkillContainer>("Database_Skills/" + chosenCharacter + "_Primary_Skill");

                        if (primary_Skill != null)
                        {
                            if (!MeloMelo_SkillData_Settings.CheckSkillStatus(primary_Skill.skillName))
                            {
                                MeloMelo_SkillData_Settings.UnlockSkill(primary_Skill.skillName);
                                MeloMelo_SkillData_Settings.LearnSkill(primary_Skill.skillName);
                                AddMessageToPopUp("Character just learned " + primary_Skill.skillName);
                            }
                            else
                            {
                                MeloMelo_SkillData_Settings.UpgradeSkill(primary_Skill.skillName);
                                AddMessageToPopUp("Character has raise skill grade on " + primary_Skill.skillName);
                            }

                            ConfirmItemUsage();
                        }
                        else
                            AddMessageToPopUp("Character skill isn't available at the moment");
                        break;

                    case 3:
                        SkillContainer secondary_Skill = Resources.Load<SkillContainer>("Database_Skills/" + chosenCharacter + "_Secondary_Skill_" + skill_index);

                        if (secondary_Skill != null)
                        {
                            if (!MeloMelo_SkillData_Settings.CheckSkillStatus(secondary_Skill.skillName))
                            {
                                MeloMelo_SkillData_Settings.UnlockSkill(secondary_Skill.skillName);
                                MeloMelo_SkillData_Settings.LearnSkill(secondary_Skill.skillName);
                                AddMessageToPopUp("Character just learned " + secondary_Skill.skillName);
                            }
                            else
                            {
                                MeloMelo_SkillData_Settings.UpgradeSkill(secondary_Skill.skillName);
                                AddMessageToPopUp("Character has raise skill grade on " + secondary_Skill.skillName);
                            }

                            ConfirmItemUsage();
                        }
                        else
                            AddMessageToPopUp("Character skill isn't available at the moment");
                        break;

                    default:
                        Invoke("ClosePanel", 3.1f);
                        break;
                }
            }
            else
                AddMessageToPopUp("Character need to be unlock before using any consumable");
        }
        else
            AddMessageToPopUp("Ticket hasn't been used after rejecting");

        Invoke("ClosePanel", 3.1f);
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
                AddMessageToPopUp("Character gained " + (amountPerUnit * percentage) + " experience during training");
            }
            else
                AddMessageToPopUp("Character level have reached its limit during training");
        }
        else
            AddMessageToPopUp("Ticket hasn't been used after rejecting");

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
                AddMessageToPopUp("Character gained " + amount + " level during training");
            }
            else
                AddMessageToPopUp("Character level have reached its limit during training");
        }
        else
            AddMessageToPopUp("Ticket hasn't been used after rejecting");

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
                AddMessageToPopUp("Stats modified repeated");

            else if (ModifyCharacterStats(GetStatsFromIndex(choiceOptionListing[0]), true, 1))
            {
                ModifyCharacterStats(GetStatsFromIndex(choiceOptionListing[1]), false, 1);
                ConfirmItemUsage();
                AddMessageToPopUp("Stats has been updated");
            }
            else
                AddMessageToPopUp("Minimum of 1 stats point require to reset");
        }
        else
            AddMessageToPopUp("Item hasn't been used after rejecting");

        Invoke("ClosePanel", 3.1f);
    }

    private void CharacterInstantUpgrade(string allStatsArray, int amount, int confirmationManual = -1)
    {
        if ((confirmationManual == -1 && choiceOptionListing[0] == 1) || confirmationManual == 1)
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

            if (assignStats.Length > 0) { AddMessageToPopUp("Succesful of used book"); }
            else { AddMessageToPopUp("Book attribute empty"); }
        }
        else
            AddMessageToPopUp("Cancel used of book");

        Invoke("ClosePanel", 3.1f);
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

    private void RestoreAdventureProgressPlay(string area)
    {
        if (choiceOptionListing[1] == 1)
        {
            foreach (StoryProgressData progress in MeloMelo_Adventure.allAdventureRouteData)
            {
                if (progress.title == area)
                {
                    foreach (int routeId in progress.routeId_listing)
                        MeloMelo_Adventure.MarkRouteCleared(progress.adventure_type, routeId);

                    break;
                }
            }

            MeloMelo_Local.LocalSave_DataManagement forceSave = new MeloMelo_Local.LocalSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
                "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

            forceSave.SelectFileForActionWithUserTag(MeloMelo_GameSettings.CloudSaveSetting_AdventureMode);
            forceSave.SaveAdventureRoutePlay();

            AddMessageToPopUp("Progress Data ( " + area + " ) - Restore Successful!");
        }

        Invoke("ClosePanel", 3.1f);
    }
    #endregion

    #region MISC
    private Queue<string> messageInLine = new Queue<string>();
    private bool messageInProgress = false;

    private IEnumerator MessagePopUp()
    {
        messageInProgress = true;

        if (promptMessage)
        {
            while (messageInLine.Count > 0)
            {
                string messageOnQueue = messageInLine.Dequeue();
                promptMessage.SetActive(true);

                promptMessage.transform.GetChild(0).GetComponent<Text>().text = messageOnQueue;
                yield return new WaitForSeconds(2);
            }
        }

        promptMessage.SetActive(false);
        messageInProgress = false;
    }

    private void AddMessageToPopUp(string customText)
    {
        if (messageInLine != null)
        {
            messageInLine.Enqueue(customText);
            if (!messageInProgress) StartCoroutine(MessagePopUp());
        }
    }
    #endregion
}
