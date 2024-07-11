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
        if (!AuthenticationService.Instance.IsSignedIn)
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
        if (PlayerPrefs.HasKey("AccountSync"))
        {
            if (Utilities.CheckForInternetConnection()) await InitializeRemoteConfigAsync();
            else AccountID_Text.GetComponentInChildren<Text>().text = "Not Signed In";

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
        PlayerPrefs.SetString("GameLatest_Update", RemoteConfigService.Instance.appConfig.GetString("Latest_Version"));
        PlayerPrefs.SetString("GameUpdate_URL", RemoteConfigService.Instance.appConfig.GetString("GameApplication_URL"));
        PlayerPrefs.SetString("GameWeb_URL", RemoteConfigService.Instance.appConfig.GetString("GameWebpage_URL"));
    }

    private void ResetGameApplicationValue()
    {
        PlayerPrefs.DeleteKey("GameLatest_Update");
        PlayerPrefs.DeleteKey("GameUpdate_URL");
        PlayerPrefs.DeleteKey("GameWeb_URL");
    }
    #endregion
}