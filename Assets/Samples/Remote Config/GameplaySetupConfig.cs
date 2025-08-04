using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using UnityEngine;


public class GameplaySetupConfig : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    async void Start()
    {
        if (ServerGateway_Script.thisServer.get_loginType == (int)MeloMelo_PlayerSettings.LoginType.GuestLogin && AuthenticationService.Instance.IsSignedIn)
        {
            await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());

            PlayerPrefs.SetInt("NoteSpeed_Legacy", RemoteConfigService.Instance.appConfig.GetInt("NoteSpeed_Legacy_speedValue"));
            PlayerPrefs.SetInt("NoteSpeed_Legacy_v2", RemoteConfigService.Instance.appConfig.GetInt("NoteSpeed_Legacy_speedValue_v2"));
        }
    }
}
