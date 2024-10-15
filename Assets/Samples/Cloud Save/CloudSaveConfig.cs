using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Threading.Tasks;
using Unity.Services.CloudSave;

using MeloMelo_Local;

public class CloudSaveConfig : MonoBehaviour
{
    [SerializeField] private Button UploadBtn;
    [SerializeField] private Button TransferBtn;
    [SerializeField] private Text AccountID;

    [SerializeField] private GameObject AlertBox_Upload;
    [SerializeField] private GameObject AlertBox_Transfer;

    private string[] pathLoader =
    {
        MeloMelo_GameSettings.CloudSaveSetting_MainProgress,
        MeloMelo_GameSettings.CloudSaveSetting_BattleProgress,
        MeloMelo_GameSettings.CloudSaveSetting_AcccountSettings,
        MeloMelo_GameSettings.CloudSaveSetting_GameplaySettings,
        MeloMelo_GameSettings.CloudSaveSetting_CharacterSettings,
        MeloMelo_GameSettings.CloudSaveSetting_PointsData,
        MeloMelo_GameSettings.CloudSaveSetting_ProfileData,
        MeloMelo_GameSettings.CloudSaveSetting_SelectionData,
        MeloMelo_GameSettings.CloudSaveSetting_CharacterStats,
        MeloMelo_GameSettings.CloudSaveSetting_SkillDatabase,
        MeloMelo_GameSettings.CloudSaveSetting_ItemDatabase,
        MeloMelo_GameSettings.CloudSaveSetting_ExchangeHistory
    };

    private string[] pathLoader2 =
    {
        MeloMelo_GameSettings.GetLocalFileChartLegacy,
        MeloMelo_GameSettings.GetLocalFileChartOld,
        MeloMelo_GameSettings.GetLocalFileChartNew
    };

    #region MAIN
    public void UploadProgressToCloud()
    {
        // Get prompt warning for processing this method
        AlertBox_Upload.SetActive(PlayerPrefs.HasKey("CloudSaveLoader_Ready") && !GetComponent<Options_Menu>().IsProcessStillPending());
    }

    public void GetProgressFromCloud()
    {
        // Get prompt warning for processing this method
        AlertBox_Transfer.SetActive(PlayerPrefs.HasKey("CloudSaveLoader_Ready") && !GetComponent<Options_Menu>().IsProcessStillPending());
    }

    public void GetCancelToAnyService(int index)
    {
        switch (index)
        {
            case 1:
                AlertBox_Upload.SetActive(false);
                break;

            case 2:
                AlertBox_Transfer.SetActive(false);
                break;
        }
    }
    #endregion

    #region COMPONENT
    public void ProcessToUpload()
    {
        if (GetComponent<Auto_Authenticate_Config>().IsDone)
        {
            UploadBtn.interactable = false;

            // Save Progress
            foreach (string path in pathLoader) TransferDataThroughLocalToCloud(path,
                "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress", ".txt");

            // Save Chart Listing
            foreach (string path in pathLoader2) TransferDataThroughLocalToCloud(path,
                "StreamingAssets/LocalData/MeloMelo_LocalSave_ChartList", ".json");

            // isDone
            StartCoroutine(GetComponent<Options_Menu>().GetOptionMessage("Upload Completed!"));
        }
    }

    public void ProcessForTransfer()
    {
        if (GetComponent<Auto_Authenticate_Config>().IsDone)
        {
            TransferBtn.interactable = false;

            // Load progress
            foreach (string path in pathLoader) TransferDataThroughCloudToLocal(path,
                "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress", ".txt");

            // Load Chart Listing
            foreach (string path in pathLoader2) TransferDataThroughCloudToLocal(path,
                "StreamingAssets/LocalData/MeloMelo_LocalSave_ChartList", ".json");

            // isDone
            StartCoroutine(GetComponent<Options_Menu>().GetOptionMessage("Transfer Completed!"));
        }
    }

    private async void UploadCloudData(string key, object value)
    {
        var data = new Dictionary<string, object> { { key, value } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }

    private async Task GetCloudData(string key, string cloudKey)
    {
        var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { cloudKey });

        if (data.TryGetValue(cloudKey, out var getToken))
        {
            Debug.Log("Cloud Data: " + getToken.Value.GetAs<string>());

            if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
            PlayerPrefs.SetString(key, getToken.Value.GetAs<string>());
            PlayerPrefs.SetInt("ReviewTransfer", 1);
        }
    }
    #endregion

    #region COMPONENT (LOCAL)
    private void TransferDataThroughLocalToCloud(string sourceFile, string destination, string fileType)
    {
        CloudDatabase_Local_DataManagement local = new CloudDatabase_Local_DataManagement
            (
                GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByPlayerId(),
                destination
            );

        UploadCloudData(sourceFile, local.GetJsonFormatFromLocalData(sourceFile + fileType));
        Debug.Log("Game Network: Local->Cloud " + sourceFile + " completed!");
    }

    private async void TransferDataThroughCloudToLocal(string sourceFile, string destination, string fileType)
    {
        CloudDatabase_Local_DataManagement local = new CloudDatabase_Local_DataManagement
            (
                GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByPlayerId(),
                destination
            );

        await GetCloudData(sourceFile + "_Container", sourceFile);
        local.SaveJsonFormatToLocalData(sourceFile + fileType, PlayerPrefs.GetString(sourceFile + "_Container"));
    }
    #endregion

    #region MISC
    public void GetContentAccountID()
    {
        string id = GetComponent<Auto_Authenticate_Config>().GetAccountID();

        if (id == string.Empty) AccountID.text = "Account ID: ---";
        else AccountID.text = "Account ID: " + id;
    } 

    public void GetContentIDForNotUsingServices(int index)
    {
        AccountID.text = index == 1 ? "- Usage of database remotely through server -" : "- Cloud Save (Not Available) -";
    }
    #endregion
}
