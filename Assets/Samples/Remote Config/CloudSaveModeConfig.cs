using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using UnityEngine;

public class CloudSaveModeConfig : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    struct LatestVersionArray
    {
        public string[] versions;
        public LatestVersionArray GetLatestVersion(string format) { return JsonUtility.FromJson<LatestVersionArray>(format); }
    }

    [SerializeField] private GameObject[] CloudFeatures;

    #region SETUP
    private void GetCloudSetupData()
    {
        CloudControlPanel(RemoteConfigService.Instance.appConfig.GetBool("CloudSave_Mode") && GetSupportCloudService());
        GetComponent<CloudSaveConfig>().GetContentAccountID();
        PlayerPrefs.SetInt("CloudSaveLoader_Ready", 1);
    }

    private async void GetSetupReady()
    {
        if (ServerGateway_Script.thisServer.get_loginType == (int)MeloMelo_PlayerSettings.LoginType.GuestLogin)
        {
            // Refresh the latest cloud data to local data
            await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
            GetCloudSetupData();
        }
    }
    #endregion

    #region MAIN
    public void RefreshCloudServices()
    {
        PlayerPrefs.DeleteKey("CloudSaveLoader_Ready");
        if (AuthenticationService.Instance.IsSignedIn) GetSetupReady();
        else
        {
            CloudControlPanel(false);
            GetComponent<CloudSaveConfig>().GetContentIDForNotUsingServices(0);
        }
    }
    #endregion

    #region COMPONENT
    private bool GetSupportCloudService()
    {
        string current = StartMenu_Script.thisMenu.version;

        LatestVersionArray versionList = new LatestVersionArray().
            GetLatestVersion(RemoteConfigService.Instance.appConfig.GetJson("CloudSave_Support"));

        foreach (string versionArray in versionList.versions)
            if (versionArray == current) return true;

        return false;
    }

    private void CloudControlPanel(bool active)
    {
        foreach (GameObject feature in CloudFeatures) feature.SetActive(active);
    }
    #endregion
}
