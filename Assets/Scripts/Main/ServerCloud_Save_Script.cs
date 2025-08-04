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
            StartCoroutine(FirstLayerSaving("[Game Network]"));

            // Prompt message to user
            mainScript.SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\nChecking Progress...";
        }
    }
    #endregion

    #region COMPONENT
    private IEnumerator FirstLayerSaving(string serverTitle)
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

        // Save Track Chart Listing
        LocalLoad_DataManagement customJsonFile = new LocalLoad_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), "StreamingAssets/LocalData/MeloMelo_LocalSave_ChartList");

        cloudDataServices.ClearCacheDistributionData(mainScript.FindTrackChartCateogry(BeatConductor.thisBeat.Music_Database.seasonNo));
        cloudDataServices.SaveChartDistributionData(mainScript.FindTrackChartCateogry(BeatConductor.thisBeat.Music_Database.seasonNo),
            customJsonFile.GetLocalJsonFile("TempPass_ChartData_" + mainScript.FindTrackChartCateogry(BeatConductor.thisBeat.Music_Database.seasonNo) + ".json", false));

        if (!PlayerPrefs.HasKey("MarathonPermit"))
        {
            // Save Character Progression (Individual Status)
            StatsDistribution allStats = new StatsDistribution();
            allStats.load_Stats();

            foreach (ClassBase character in allStats.slot_Stats)
            {
                if (character.characterName != "None")
                {
                    // Save: Character Progress
                    cloudDataServices.SaveCharacterStatusData(character.name, character.level, character.experience, 0, 0, 0, 0, 0);

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

                // Save All Item: Database
                cloudDataServices.SaveItemDataToServer(MeloMelo_ItemUsage_Settings.GetActiveItems());
            }
        }
        else
            StartCoroutine(PlayMarathonReward());

        yield return new WaitUntil(() => cloudDataServices.get_process.ToArray().Length == cloudDataServices.get_counter);
        mainScript.ContentSavedCompleted(serverTitle, GetProcessCloudSuccessful(cloudDataServices.get_process.ToArray()));

        yield return new WaitForSeconds(1.5f);

        // Checking other component settings
        StartCoroutine(FinalLayerSaving(serverTitle));
    }

    private IEnumerator FinalLayerSaving(string serverTitle)
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

        // Save Profile
        cloudDataServices.SaveProgressProfile(
            PlayerPrefs.GetInt(LoginPage_Script.thisPage.get_user + "totalRatePoint", 0),
            PlayerPrefs.GetInt(LoginPage_Script.thisPage.get_user + "PlayedCount_Data") + 1,
            PlayerPrefs.GetInt(LoginPage_Script.thisPage.get_user + "_Credits", 0)
            );

        // Save Last Visited Area
        if (!PlayerPrefs.HasKey("MarathonPermit"))
            cloudDataServices.SaveMusicSelectionLastVisited(
                PreSelection_Script.thisPre.get_AreaData.AreaName,
                PlayerPrefs.GetInt("LastSelection", 1),
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1)
                );

        // Save Configuration (First-Time Visit)
        cloudDataServices.SavePlayerData(
            PlayerPrefs.GetString("HowToPlay_Notice", "T"),
            PlayerPrefs.GetString("BattleSetup_Guide", "T"),
            PlayerPrefs.GetString("Control_notice", "T"),
            PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetBGM_ValueKey),
            PlayerPrefs.GetFloat("SE_VolumeGET", 1)
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
        StartCoroutine(misc.LoadTrackDistributionChart());

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
        CloudSave_DataManagement data = new CloudSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), MeloMelo_PlayerSettings.GetWebServerUrl());

        ItemData item = Resources.Load<ItemData>("Database_Item/#12");
        int maxAmount = IsTrackListCleared() ? 1 : 0;
        maxAmount += IsTrackListCleared() && PlayerPrefs.GetInt("Marathon_Quest_Result", 0) == 1 ? 1 : 0;

        if (item && maxAmount > 0)
        {
            mainScript.PromptMessage.SetActive(true);
            mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Item Obtained: " +
                item.itemName + " ( x" + maxAmount + " )";

            MeloMelo_ItemUsage_Settings.OverwriteActiveItem(item.itemName, maxAmount);
            data.SaveItemDataToServer(MeloMelo_ItemUsage_Settings.GetActiveItems());
        }

        yield return new WaitForSeconds(1);
        mainScript.PromptMessage.SetActive(false);
    }
    #endregion
}
