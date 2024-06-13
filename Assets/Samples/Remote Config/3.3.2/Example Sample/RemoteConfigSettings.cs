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

public class RemoteConfigSettings : MonoBehaviour
{
    public struct userAttributes {}
    public struct appAttributes {}

    #region SETUP
    async Task InitializeRemoteConfigAsync()
    {
        // initialize handlers for unity game services
        await UnityServices.InitializeAsync();

        // remote config requires authentication for managing environment information
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("PlayID Quick Startup: " + AuthenticationService.Instance.PlayerId);
        }
    }

    async Task Start()
    {
        // initialize Unity's authentication and core services, however check for internet connection
        // in order to fail gracefully without throwing exception if connection does not exist
        if (Utilities.CheckForInternetConnection()) await InitializeRemoteConfigAsync();

        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;
        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
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