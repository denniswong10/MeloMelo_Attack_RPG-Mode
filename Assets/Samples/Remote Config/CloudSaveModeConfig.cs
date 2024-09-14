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

        public LatestVersionArray GetLatestVersion(string format)
        {
            return JsonUtility.FromJson<LatestVersionArray>(format);
        }
    }

    [SerializeField] private GameObject[] CloudFeatures;

    async void Start()
    {
        if (ServerGateway_Script.thisServer.get_loginType == (int)MeloMelo_GameSettings.LoginType.GuestLogin && AuthenticationService.Instance.IsSignedIn)
        {
            await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
            foreach (GameObject feature in CloudFeatures) feature.SetActive(GetSupportCloudService());
        }
    }

    #region MAIN
    public bool GetCloudSave()
    {
        return RemoteConfigService.Instance.appConfig.GetBool("CloudSave_Mode");
    }

    public bool GetSupportCloudService()
    {
        string current = StartMenu_Script.thisMenu.get_version;

        LatestVersionArray versionList = new LatestVersionArray().
            GetLatestVersion(RemoteConfigService.Instance.appConfig.GetJson("CloudSave_Support"));

        foreach (string versionArray in versionList.versions)
            if (versionArray == current) return true;

        return false;
    }

    public void DisableCloud()
    {
        foreach (GameObject feature in CloudFeatures) feature.SetActive(false);
    }
    #endregion
}
