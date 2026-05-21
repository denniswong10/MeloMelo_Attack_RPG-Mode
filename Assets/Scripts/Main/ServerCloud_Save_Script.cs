using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_Network;
using MeloMelo_Local;
using MeloMelo_RPGEditor;

public class ServerCloud_Save_Script : MonoBehaviour
{
    private CloudSave_DataManagement cloudDataServices;
    private ResultMenu_Script mainScript;

    // Start is called before the first frame update
    void Start()
    {
        if (LoginPage_Script.thisPage.portNumber == (int)MeloMelo_PlayerSettings.LoginType.TempPass)
        {
            mainScript = GetComponent<ResultMenu_Script>();
            cloudDataServices = new CloudSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), MeloMelo_PlayerSettings.GetWebServerUrl());
        }
        else
            Destroy(GetComponent<ServerCloud_Save_Script>());
    }

    #region MAIN 
    public void StartSavingProcess()
    {
        if (cloudDataServices != null)
        {
            mainScript.SaveIcon.SetActive(true);
            StartCoroutine(CheckingForExtension("[Game Network]"));

            // Prompt message to user
            mainScript.SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\nChecking Progress...";
        }
    }
    #endregion

    #region COMPONENT
    private IEnumerator CheckingForExtension(string serverTitle)
    {
        if (!PlayerPrefs.HasKey("MarathonPermit"))
        {
            // Save Character Progression (Individual Status)
            StatsDistribution allStats = new StatsDistribution();
            allStats.load_Stats();

            foreach (Character_Base_Data character in allStats.slot_Stats)
            {
                if (character != null)
                {
                    // Clear effect buff: After used
                    if (MeloMelo_ItemUsage_Settings.GetExpBoost(character.className) > 0)
                        PlayerPrefs.SetInt(character.className + "_EXP_BOOST", 0);
                    if (MeloMelo_ItemUsage_Settings.GetExpBoostByMultiply(character.className) > 0)
                        PlayerPrefs.SetInt(character.className + "_EXP_BOOST_2", 0);
                    if (MeloMelo_ItemUsage_Settings.GetPowerBoost(character.className) > 0)
                        PlayerPrefs.SetInt(character.className + "_POWER_BOOST", 0);
                    if (MeloMelo_ItemUsage_Settings.GetPowerBoostByMultiply(character.className) > 0)
                        PlayerPrefs.SetInt(character.className + "_POWER_BOOST_2", 0);

                    // Reset usage of pot slot
                    PlayerPrefs.DeleteKey(character.className + "_EXP_USAGE_COUNT");
                    PlayerPrefs.DeleteKey(character.className + "_POWER_USAGE_COUNT");
                }
            }

            // Remove Used Item: Game Local
            if (MeloMelo_ItemUsage_Settings.GetActiveItems() != null)
            {
                foreach (VirtualItemDatabase item in MeloMelo_ItemUsage_Settings.GetActiveItems())
                {
                    if (MeloMelo_ItemUsage_Settings.GetItemUsed(item.itemName) > 0)
                    {
                        mainScript.PromptMessage.SetActive(true);
                        mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Successful Used: " +
                            item.itemName + " ( x" + MeloMelo_ItemUsage_Settings.GetItemUsed(item.itemName) + " )";

                        MeloMelo_ItemUsage_Settings.OverwriteActiveItem(item.itemName, -MeloMelo_ItemUsage_Settings.GetItemUsed(item.itemName));
                        PlayerPrefs.DeleteKey(item.itemName + "_VirtualItem_Unsaved_Used");
                        yield return new WaitForSeconds(2);
                        mainScript.PromptMessage.SetActive(false);
                    }
                }

                yield return StartCoroutine(PlayEventControl());
            }
        }
        else
            yield return StartCoroutine(PlayMarathonReward());

        // Checking other component settings
        StartCoroutine(SaveAllProgress(serverTitle));
    }

    private IEnumerator SaveAllProgress(string serverTitle)
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("[TempPass->SaveProgress] ID: " + LoginPage_Script.thisPage.GetUserPortOutput());

        // Save 1
        cloudDataServices.SaveProgressTrack(
                BeatConductor.thisBeat.Music_Database.Title,
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                (int)GameManager.thisManager.get_score1.get_score,
                GameManager.thisManager.getJudgeWindow.getMaxCombo
                );

        // Save 2
        cloudDataServices.SaveProgressTrackByRemark(
            BeatConductor.thisBeat.Music_Database.Title,
            PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
            PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 5)
            );

        // Save 3
        cloudDataServices.SaveProgressTrackByPoint(
                BeatConductor.thisBeat.Music_Database.Title,
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                (int)GameManager.thisManager.get_point.get_score
                );

        // Save Last Visited Area
        if (!PlayerPrefs.HasKey("MarathonPermit"))
            cloudDataServices.SaveMusicSelectionLastVisited(
                PreSelection_Script.thisPre.get_AreaData.AreaName,
                PlayerPrefs.GetInt("LastSelection", 1),
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1)
                );

        // Save Track Chart Listing
        LocalLoad_DataManagement customJsonFile = new LocalLoad_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), "StreamingAssets/LocalData/MeloMelo_LocalSave_ChartList");

        cloudDataServices.ClearCacheDistributionData(MeloMelo_GameSettings.FindTrackChartCateogry(BeatConductor.thisBeat.Music_Database.seasonNo));
        cloudDataServices.SaveChartDistributionData(MeloMelo_GameSettings.FindTrackChartCateogry(BeatConductor.thisBeat.Music_Database.seasonNo),
            customJsonFile.GetLocalJsonFile("TempPass_ChartData_" + MeloMelo_GameSettings.FindTrackChartCateogry(BeatConductor.thisBeat.Music_Database.seasonNo) + ".json", false));

        if (!PlayerPrefs.HasKey("MarathonPermit"))
        {
            // Save Character Progression (Individual Status)
            StatsDistribution allStats = new StatsDistribution();
            allStats.load_Stats();

            foreach (Character_Base_Data character in allStats.slot_Stats)
            {
                if (character != null)
                {
                    // Save: Character Progress
                    cloudDataServices.SaveCharacterStatusData(character.className, character.level, character.experience,
                        character.additionalProfile != null ? character.additionalProfile.masteryPoint : 0,
                        character.additionalProfile != null ? character.additionalProfile.rebirth_count : 0,
                        character.additionalProfile != null ? character.additionalStats.strength : 0,
                        character.additionalProfile != null ? character.additionalStats.vitalilty : 0,
                        character.additionalProfile != null ? character.additionalStats.magic : 0
                        );
                }
            }
        }

        // Save All Item: Database
        cloudDataServices.SaveItemDataToServer(MeloMelo_ItemUsage_Settings.GetActiveItems());

        yield return new WaitUntil(() => cloudDataServices.get_process.ToArray().Length == cloudDataServices.get_counter);
        mainScript.ContentSavedCompleted(serverTitle, GetProcessCloudSuccessful(cloudDataServices.get_process.ToArray()));

        yield return new WaitForSeconds(1.5f);

        // Checking other component settings
        StartCoroutine(FinalizePlayerSettings(serverTitle));
    }

    private IEnumerator FinalizePlayerSettings(string serverTitle)
    {
        if (!PlayerPrefs.HasKey("MarathonPermit")) mainScript.CheckTechScore();
        mainScript.SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\nChecking Data...";
        yield return new WaitForSeconds(1);

        // Save Configuartion (Gameplay Setup)
        cloudDataServices.SaveSettingConfiguration(
                PlayerPrefs.GetString("MVOption", "T"),
                PlayerPrefs.GetInt("NoteSpeed", 20),
                PlayerPrefs.GetInt("AutoRetreat", 0),
                PlayerPrefs.GetInt("ScoreDisplay", 0),
                PlayerPrefs.GetInt("ScoreDisplay2", 0),
                PlayerPrefs.GetInt("JudgeMeter_Setup", 0)
                );

        // Save Configuartion (Player Settings)
        cloudDataServices.SaveSystemSettingConfiguration();

        // Save Profile
        cloudDataServices.SaveProgressProfile(
            PlayerPrefs.GetInt(LoginPage_Script.thisPage.get_user + "totalRatePoint", 0),
            PlayerPrefs.GetInt(LoginPage_Script.thisPage.get_user + "PlayedCount_Data") + 1,
            PlayerPrefs.GetInt(LoginPage_Script.thisPage.get_user + "_Credits", 0)
            );

        // Save Configuration (First-Time Visit)
        cloudDataServices.SavePlayerData(
            PlayerPrefs.GetString("HowToPlay_Notice", "T"),
            PlayerPrefs.GetString("BattleSetup_Guide", "T"),
            PlayerPrefs.GetString("Control_notice", "T"),
            PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetBGM_ValueKey),
            PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetSE_ValueKey)
            );

        // Save Configuration (Character Formation)
        cloudDataServices.SaveBattleFormation(
            PlayerPrefs.GetString("Slot1_charName", "None"),
            PlayerPrefs.GetString("Slot2_charName", "None"),
            PlayerPrefs.GetString("Slot3_charName", "None"),
            PlayerPrefs.GetString("CharacterFront", "None")
            );

        // Marathon Content
        mainScript.CheckMarathonContent();

        // BonusPlay: Reset
        if (PlayerPrefs.GetInt("BonusTrackPlay", 0) > 0)
        {
            int currentUsed = PlayerPrefs.GetInt("BonusTrackPlay", 0);
            PlayerPrefs.SetInt("BonusTrackPlay", currentUsed - 1);
        }

        // Load Track Chart Listing
        CloudLoad_DataManagement misc = new CloudLoad_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), MeloMelo_PlayerSettings.GetWebServerUrl());
        StartCoroutine(misc.VerifyTrackDistributionList());

        yield return new WaitUntil(() => cloudDataServices.get_process.ToArray().Length == cloudDataServices.get_counter &&
            misc.cloudLogging.ToArray().Length == misc.get_counter);

        ContentCheckingData(serverTitle, GetProcessCloudSuccessful(cloudDataServices.get_process.ToArray()));
        ContentCheckingData(serverTitle, GetProcessCloudSuccessful(misc.cloudLogging.ToArray()));

        // Process to encode
        StartCoroutine(mainScript.Encode_DataCheck());
    }
    #endregion

    #region MISC
    private bool GetProcessCloudSuccessful(bool[] condition)
    {
        foreach (bool check in condition)
            if (!check) return false;

        return true;
    }

    private void ContentCheckingData(string title, bool isComplete)
    {
        if (isComplete)
            mainScript.SaveIcon.transform.GetChild(1).GetComponent<Text>().text = title + "\nServer OK!";
        else
            mainScript.SaveIcon.transform.GetChild(1).GetComponent<Text>().text = title + "\nServer Error!";
    }

    public void ConnectionEstablish(WWWForm info, string url)
    {
        mainScript.SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\nSaving Data...";
        StartCoroutine(cloudDataServices.GetServerToSave(url, info));
    }

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
        ItemData item = Resources.Load<ItemData>("Database_Item/#12");
        int maxAmount = IsTrackListCleared() ? 1 : 0;
        maxAmount += IsTrackListCleared() && PlayerPrefs.GetInt("Marathon_Quest_Result", 0) == 1 ? 1 : 0;

        if (item && maxAmount > 0)
        {
            mainScript.PromptMessage.SetActive(true);
            mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Item Obtained: " +
                item.itemName + " ( x" + maxAmount + " )";

            MeloMelo_ItemUsage_Settings.OverwriteActiveItem(item.itemName, maxAmount);
        }

        yield return new WaitForSeconds(1);
        mainScript.PromptMessage.SetActive(false);
    }

    private IEnumerator PlayEventControl()
    {
        if (PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetPlayEventSettings_RewardKey) == 1 &&
            MeloMelo_ExtensionContent_Settings.GetEventRewardArray() != null)
        {
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
                    MeloMelo_ExtensionContent_Settings.GetVersionNumber(item.upToDate)
                    )
                {
                    mainScript.PromptMessage.SetActive(true);
                    mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Item Obtained: " +
                        item.itemName + " ( x" + item.maxObtain + " )";

                    MeloMelo_ItemUsage_Settings.OverwriteActiveItem(item.itemName, item.maxObtain);
                    int currentObtain = PlayerPrefs.GetInt(eventPlayId + "_RepeatableRewarding", 1);
                    PlayerPrefs.SetInt(eventPlayId + "_RepeatableRewarding", currentObtain + 1);

                    yield return new WaitForSeconds(1.5f);
                }
                else if (PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetPlayEventSettings_SkipKey) == 0)
                {
                    mainScript.PromptMessage.SetActive(true);
                    mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Play Event: " +
                    item.itemName + " (" + PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_playedCount_EventReward", 0) +
                    "/" + item.playRequirement * PlayerPrefs.GetInt(eventPlayId + "_RepeatableRewarding", 1) + " )";

                    yield return new WaitForSeconds(1.5f);
                }

                mainScript.PromptMessage.SetActive(false);
                eventPlayId++;
            }
        }

       if (PlayerPrefs.HasKey("GatheringMode")) yield return StartCoroutine(GatherFragmentAfterBattle());
       // StartCoroutine(ZoneControlReward());
    }

    private IEnumerator GatherFragmentAfterBattle()
    {
        if (!PlayerPrefs.HasKey("CharacterKnockOutSuccessful"))
        {
            ItemData item = Resources.Load<ItemData>("Database_Item/#49");
            int itemChanceObtain = Random.Range(1, 100);
            int maxAmount = (PlayerPrefs.HasKey("EnemyKnockOutSuccessful") ? 1 : 0) + (itemChanceObtain >= 80 ? 1 : 0);
            mainScript.PromptMessage.SetActive(true);

            if (item != null)
            {
                if (maxAmount > 0)
                {
                    mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Item Obtained: " +
                    item.itemName + " ( x" + maxAmount + " )";

                    MeloMelo_ItemUsage_Settings.OverwriteActiveItem(item.itemName, maxAmount);
                }
                else
                {
                    mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Gathering Mode: " +
                    item.itemName + " [ No drop obtained ]";

                    Debug.Log("Map Fragment Chance: " + itemChanceObtain + " %");
                }
            }

            yield return new WaitForSeconds(1);
            mainScript.PromptMessage.SetActive(false);
        }
    }
    #endregion
}
