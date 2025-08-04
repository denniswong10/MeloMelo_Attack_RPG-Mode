using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_Local;
using MeloMelo_RPGEditor;

class GameDataProcessing
{
    public int currentProcess { get; private set; }
    public int totalProcessed { get; private set; }

    public void BeginProcess() => totalProcessed += 1;
    public void CompletedProcess() => currentProcess += 1;
    public bool GetProcessingComplete() { return currentProcess == totalProcessed; }
}

public class Local_Save_Script : MonoBehaviour
{
    private LocalSave_DataManagement localfile;
    private ResultMenu_Script mainScript;
    private GameDataProcessing postProcessing;

    // Start is called before the first frame update
    void Start()
    {
        postProcessing = new GameDataProcessing();

        if (LoginPage_Script.thisPage.portNumber == (int)MeloMelo_PlayerSettings.LoginType.GuestLogin)
        {
            mainScript = GetComponent<ResultMenu_Script>();
            localfile = new LocalSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");
        }
        else
            Destroy(GetComponent<Local_Save_Script>());
    }

    #region MAIN
    public void StartSavingProcess()
    {
        if (localfile != null)
        {
            mainScript.SaveIcon.SetActive(true);
            StartCoroutine(AllSave("[Game Local]"));

            // Prompt message to user
            mainScript.SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "[Game Local]\nChecking Progress...";
        }
    }
    #endregion

    #region COMPONENT 
    private IEnumerator AllSave(string serverTitle)
    {
        bool isInvaild = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted() == GameManager.thisManager.getJudgeWindow.getOverallCombo;

        if (isInvaild)
        {
            LocalSave_DataManagement data = new LocalSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
                "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

            // Save: Achievement
            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileMainProgress);
            data.SaveProgress(BeatConductor.thisBeat.Music_Database.Title,
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                GameManager.thisManager.get_score1.get_score,
                MeloMelo_GameSettings.GetScoreRankStructure(GameManager.thisManager.get_score1.get_score.ToString()).rank);

            // Save: Points
            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFilePointData);
            data.SavePointProgress(BeatConductor.thisBeat.Music_Database.Title,
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                (int)GameManager.thisManager.get_point.get_score,
                PlayerPrefs.GetInt("OverallCombo", 0) * 3);

            // Save: Battle
            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileBattleProgress);
            data.SaveBattleProgress(
                BeatConductor.thisBeat.Music_Database.Title,
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                PlayerPrefs.GetInt("BattleDifficulty_Mode", 1),
                PlayerPrefs.GetString(BeatConductor.thisBeat.Music_Database.Title + "_SuccessBattle_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1) + PlayerPrefs.GetInt("BattleDifficulty_Mode", 1), "F") == "T" ? true : false,
                (int)GameManager.thisManager.get_score2.get_score);


            // Save: Adventure Mode
            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileAdventureMode);
            data.SaveAdventureRoutePlay();

            // Save: Profile
            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileProfileData);
            data.SaveProfileState();

            // Save: Account
            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileAccountSettings);
            data.SaveAccountSettings();

            // Save: Last Selection Point
            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileSelectionData);
            if (PlayerPrefs.HasKey("MarathonPermit"))
                mainScript.CheckMarathonContent();
            else if (!PlayerPrefs.HasKey("Mission_Played"))
                data.SaveLatestSelectionPoint(PreSelection_Script.thisPre.get_AreaData.AreaName, PlayerPrefs.GetInt("LastSelection", 1));

            if (!PlayerPrefs.HasKey("MarathonPermit"))
            {
                StatsDistribution allStats = new StatsDistribution();
                allStats.load_Stats();

                foreach (ClassBase character in allStats.slot_Stats)
                {
                    if (character.characterName != "None")
                    {
                        // Save: Character Progress
                        data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileCharacterStats);
                        data.SaveCharacterStatsProgress(character.name, character.level, character.experience);

                        // Clear effect buff: After used
                        if (MeloMelo_ItemUsage_Settings.GetExpBoost(character.name) > 0)
                            PlayerPrefs.SetInt(character.name + "_EXP_BOOST", 0);
                        if (MeloMelo_ItemUsage_Settings.GetExpBoostByMultiply(character.name) > 0)
                            PlayerPrefs.SetInt(character.name + "_EXP_BOOST_2", 0);
                        if (MeloMelo_ItemUsage_Settings.GetPowerBoost(character.name) > 0)
                            PlayerPrefs.SetInt(character.name + "_POWER_BOOST", 0);
                        if (MeloMelo_ItemUsage_Settings.GetPowerBoostByMultiply(character.name) > 0)
                            PlayerPrefs.SetInt(character.name + "_POWER_BOOST_2", 0);
                    }
                }

                // BonusPlay: Reset
                if (PlayerPrefs.GetInt("BonusTrackPlay", 0) > 0)
                {
                    int currentUsed = PlayerPrefs.GetInt("BonusTrackPlay", 0);
                    PlayerPrefs.SetInt("BonusTrackPlay", currentUsed - 1);
                }

                // Save: Skills
                data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileSkillDatabase);
                data.SaveAllSkillsType();

                // Save: Used Items
                data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);
                string[] listOfUsedItem = MeloMelo_ItemUsage_Settings.GetAllItemUsed();

                if (listOfUsedItem != null)
                {
                    foreach (string itemName in listOfUsedItem)
                    {
                        mainScript.PromptMessage.SetActive(true);
                        mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Successful Used: " +
                            itemName + " ( x" + MeloMelo_ItemUsage_Settings.GetItemUsed(itemName) + " )";

                        data.SaveVirtualItemFromPlayer(itemName, -MeloMelo_ItemUsage_Settings.GetItemUsed(itemName), true);
                        PlayerPrefs.DeleteKey(itemName + "_VirtualItem_Unsaved_Used");
                        yield return new WaitForSeconds(2);
                        mainScript.PromptMessage.SetActive(false);
                    }
                }

                StartCoroutine(PlayEventControl());
            }
            else 
                StartCoroutine(PlayMarathonReward());

            LoadAllItemToLocal();
        }

        yield return new WaitForSeconds(!isInvaild ? 5 : 1);
        yield return new WaitUntil(() => postProcessing.GetProcessingComplete());
        mainScript.ContentSavedCompleted(serverTitle, isInvaild);

        yield return new WaitForSeconds(1);
        mainScript.SaveIcon.transform.GetChild(1).GetComponent<Text>().text = serverTitle + "\nGame Loading...";

        // Process to encode
        StartCoroutine(mainScript.Encode_DataCheck());
    }
    #endregion

    #region MISC
    private bool IsTrackListCleared()
    {
        bool isCleared = false;
        int clearedOfLength = PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty) != "CustomList" ?
            Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty)).trackList.Length :
                MeloMelo_ExtensionContent_Settings.LoadMarathonDetail(PlayerPrefs.GetInt("MarathonInstanceNumber", 0)).track_difficulty.Length;

        for (int track = 0; track < clearedOfLength; track++)
        {
            if (PlayerPrefs.HasKey("TrackListRecord_Score" + track)) isCleared = true;
            else isCleared = false;
        }

        return isCleared;
    }

    private IEnumerator PlayMarathonReward()
    {
        postProcessing.BeginProcess();

        LocalSave_DataManagement data = new LocalSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
                "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

        ItemData item = Resources.Load<ItemData>("Database_Item/#12");
        int maxAmount = IsTrackListCleared() ? 1 : 0;
        maxAmount += IsTrackListCleared() && PlayerPrefs.GetInt("Marathon_Quest_Result", 0) == 1 ? 1 : 0;

        if (item && maxAmount > 0)
        {
            mainScript.PromptMessage.SetActive(true);
            mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Item Obtained: " +
                item.itemName + " ( x" + maxAmount + " )";

            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);
            data.SaveVirtualItemFromPlayer(item.itemName, maxAmount, true);
        }

        yield return new WaitForSeconds(1);
        mainScript.PromptMessage.SetActive(false);
        postProcessing.CompletedProcess();
    }

    private IEnumerator GatherFragmentAfterBattle()
    {
        postProcessing.BeginProcess();

        LocalSave_DataManagement data = new LocalSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
                "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

        ItemData item = Resources.Load<ItemData>("Database_Item/#49");
        int itemChanceObtain = 100 - Random.Range(1, 100);
        int maxAmount = itemChanceObtain >= 55 ? 1 : 0;

        if (item && maxAmount > 0)
        {
            mainScript.PromptMessage.SetActive(true);
            mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Item Obtained: " +
                item.itemName + " ( x" + maxAmount + " )";

            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);
            data.SaveVirtualItemFromPlayer(item.itemName, maxAmount, true);
        }
        else
            Debug.Log("Map Fragment Chance: " + itemChanceObtain + " %");

        yield return new WaitForSeconds(1);
        mainScript.PromptMessage.SetActive(false);
        postProcessing.CompletedProcess();
    }

    private IEnumerator ClaimRewardAdventureSeries()
    {
        UsageOfItemDetail[] inActiveDirectory = Resources.LoadAll<UsageOfItemDetail>("Database_Item/Filtered_Items/Story_Rewarding_Bundle");
        List<VirtualItemDatabase> unPacked_item_reward = new List<VirtualItemDatabase>();

        LocalSave_DataManagement data = new LocalSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
                "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");
        data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);

        if (inActiveDirectory != null && inActiveDirectory.Length > 0)
        {
            postProcessing.BeginProcess();

            foreach (UsageOfItemDetail inActiveArtifact in inActiveDirectory)
            {
                if (MeloMelo_ItemUsage_Settings.GetActiveItem(inActiveArtifact.itemName).itemName == inActiveArtifact.itemName)
                {
                    foreach (string decode_item in inActiveArtifact.dataArray.Split('/'))
                    {
                        if (decode_item != string.Empty)
                            unPacked_item_reward.Add(new VirtualItemDatabase().GetItemData(decode_item));
                    }

                    Debug.Log("Adventure Package: " + inActiveArtifact.itemName + "( SUCCESS! )");

                    foreach (VirtualItemDatabase item in unPacked_item_reward)
                    {
                        if (item.amount > 0)
                        {
                            data.SaveVirtualItemFromPlayer(item.itemName, item.amount, true);
                            mainScript.PromptMessage.SetActive(true);
                            mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Story Reward: " + item.itemName + " ( x" + item.amount + " )";
                            yield return new WaitForSeconds(2);
                        }

                        Debug.Log("Package Extracted: " + item.itemName + " x" + item.amount);
                    }

                    if (unPacked_item_reward.ToArray().Length == 0) Debug.Log("Package Extracted: " + inActiveArtifact.itemName + "( EMPTY! )");

                    data.SaveVirtualItemFromPlayer(inActiveArtifact.itemName, -1, false);
                    mainScript.PromptMessage.SetActive(false);
                    yield return new WaitForSeconds(1);
                }
                else
                    Debug.Log("Adventure Package: " + inActiveArtifact.itemName + "( NOT FOUND )");
            }

            postProcessing.CompletedProcess();
        }

        if (inActiveDirectory.Length == 0) Debug.Log("Adventure Program: Not Found...");
        yield return null;
    }

    private IEnumerator PlayEventControl()
    {
        LocalSave_DataManagement data = new LocalSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
                "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

        // Save: Reward Items
        data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);

        if (MeloMelo_ExtensionContent_Settings.GetEventRewardArray() != null)
        {
            postProcessing.BeginProcess();

            int eventPlayedCount = PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_playedCount_EventReward", 0);
            int countMultiplier = PlayerPrefs.HasKey(BeatConductor.thisBeat.Music_Database.Title + "_areaBonusTrack") ? 2 : 1;

            PlayerPrefs.DeleteKey(BeatConductor.thisBeat.Music_Database.Title + "_areaBonusTrack");
            PlayerPrefs.SetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_playedCount_EventReward", eventPlayedCount + countMultiplier);
            int eventPlayId = 1;

            foreach (PlayEventRewardData item in MeloMelo_ExtensionContent_Settings.GetEventRewardArray())
            {
                if (PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_playedCount_EventReward", 0) >= item.playRequirement *
                    PlayerPrefs.GetInt(eventPlayId + "_RepeatableRewarding", 1) &&

                    MeloMelo_ExtensionContent_Settings.GetVersionNumber(StartMenu_Script.thisMenu.version) >=
                    MeloMelo_ExtensionContent_Settings.GetVersionNumber(item.version)
                    )
                {
                    mainScript.PromptMessage.SetActive(true);
                    mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Item Obtained: " +
                        item.itemName + " ( x" + item.maxObtain + " )";

                    data.SaveVirtualItemFromPlayer(item.itemName, item.maxObtain, true);
                    int currentObtain = PlayerPrefs.GetInt(eventPlayId + "_RepeatableRewarding", 1);
                    PlayerPrefs.SetInt(eventPlayId + "_RepeatableRewarding", currentObtain + 1);
                }
                else
                {
                    mainScript.PromptMessage.SetActive(true);
                    mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Item Review: " +
                    item.itemName + " (" + PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_playedCount_EventReward", 0) +
                    "/" + item.playRequirement * PlayerPrefs.GetInt(eventPlayId + "_RepeatableRewarding", 1) + " )";
                }

                yield return new WaitForSeconds(1.5f);
                mainScript.PromptMessage.SetActive(false);
                eventPlayId++;
            }

            postProcessing.CompletedProcess();
        }

        if (PlayerPrefs.HasKey("GatheringMode")) StartCoroutine(GatherFragmentAfterBattle());
        StartCoroutine(ClaimRewardAdventureSeries());
    }

    private void LoadAllItemToLocal()
    {
        LocalLoad_DataManagement loadData = new LocalLoad_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
               "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");
        loadData.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);
        StartCoroutine(loadData.PostLoading_VirtualItemData());
    }
    #endregion
}
