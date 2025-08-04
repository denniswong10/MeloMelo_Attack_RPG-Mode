// -----------------------------------------------------------------------------
//
// This sample example C# file can be used to quickly utilise usage of Remote Config APIs
// For more comprehensive code integration, visit https://docs.unity3d.com/Packages/com.unity.remote-config@latest
//
// -----------------------------------------------------------------------------

using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class RemoteConfigSettings : MonoBehaviour
{
    public struct userAttributes {}
    public struct appAttributes {}

    [Header("Extra Context")]
    [SerializeField] private GameObject AccountID_Text;

    #region SETUP
    async Task InitializeRemoteConfigAsync()
    {
        // initialize handlers for unity game services
        await UnityServices.InitializeAsync();

        // remote config requires authentication for managing environment information
        if (MeloMelo_PlayerSettings.GetLocalUserAccount() && !AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync
                    (
                        PlayerPrefs.GetString("AccountSync_PlayerID"),
                        PlayerPrefs.GetString("AccountSync_UniqueID")
                    );

            AccountID_Text.GetComponentInChildren<Text>().text = AuthenticationService.Instance.PlayerId;
        }
    }

    async Task Start()
    {
        AccountID_Text.SetActive(true);

        // initialize Unity's authentication and core services, however check for internet connection
        // in order to fail gracefully without throwing exception if connection does not exist

        if (Utilities.CheckForInternetConnection()) await InitializeRemoteConfigAsync();
        else AccountID_Text.GetComponentInChildren<Text>().text = "Not Connected";

        if (MeloMelo_PlayerSettings.GetLocalUserAccount())
        {
            MeloMelo_PlayerSettings.DisableWebServerCache();
            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;
            await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
        }
        else
            AccountID_Text.GetComponentInChildren<Text>().text = "Not Signed In";
    }

    void ApplyRemoteConfig(ConfigResponse configResponse)
    {
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session and no local cache file exists; using default values.");

                ResetGameApplicationValue();
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly.");
                Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config.ToString());

                GetGameApplicationValue();
                break;
        }
    }
    #endregion

    #region COMPONENT
    private void GetGameApplicationValue()
    {
        // Game Overview: Configuration
        PlayerPrefs.SetString("GameLatest_Update", RemoteConfigService.Instance.appConfig.GetString("Latest_Version"));
        PlayerPrefs.SetString("GameUpdate_URL", RemoteConfigService.Instance.appConfig.GetString("GameApplication_URL"));
        PlayerPrefs.SetString("MeloMelo_NewsReport_Daily", RemoteConfigService.Instance.appConfig.GetJson("MeloMelo_GameUpdates"));

        // Auto Patcher: Configuration
        PlayerPrefs.SetString("Application_Direct_Link", RemoteConfigService.Instance.appConfig.GetString("AutoPatcher_DirectLink_URL"));
        PlayerPrefs.SetString("Application_VersionControl_Log", RemoteConfigService.Instance.appConfig.GetString("AutoPatcher_VersionControl_URL"));

        // Game BackEnd: Event Mode
        PlayerPrefs.SetString("VersionControl_PlayEvent", RemoteConfigService.Instance.appConfig.GetJson("CloudSave_Support"));
        PlayerPrefs.SetString("JSON_Custom_Marathon_Challenge", RemoteConfigService.Instance.appConfig.GetJson("MeloMelo_Marathon_CustomPlay"));
        PlayerPrefs.SetString("JSON_Custom_Marathon_Exchange", RemoteConfigService.Instance.appConfig.GetJson("MeloMelo_Marathon_Exchange"));
    }

    private void ResetGameApplicationValue()
    {
        PlayerPrefs.DeleteKey("GameLatest_Update");
        MeloMelo_PlayerSettings.DisableWebServerCache();
        PlayerPrefs.DeleteKey("MeloMelo_NewsReport_Daily");
    }
    #endregion
}