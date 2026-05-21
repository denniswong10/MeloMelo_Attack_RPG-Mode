using System.Collections;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using UnityEngine;

public class TrackTagSetup : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    private TrackTagSupport releaseTag;
    private TrackTagSupport2 areaBonusTag;

    struct TrackTagSupport
    {
        public string[] newReleaseTrack;

        public TrackTagSupport SetReleaseTag(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<TrackTagSupport>(format);
        }
    }

    struct TrackTagSupport2
    {
        public string[] areaBonusTrack;

        public TrackTagSupport2 SetBonusTag(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<TrackTagSupport2>(format);
        }
    }

    async void Start()
    {
        try
        {
            switch (ServerGateway_Script.thisServer.get_loginType)
            {
                case (int)MeloMelo_PlayerSettings.LoginType.GuestLogin:
                    if (AuthenticationService.Instance.IsSignedIn)
                    {
                        RemoteConfigService.Instance.FetchCompleted += CompleteConfig;
                        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
                    }
                    break;

                default:
                    StartCoroutine(LoadingNetworkPlayEvent());
                    break;
            }
        }
        catch
        {
            Debug.Log("Selection Tag: Disable");
        }
    }

    void OnDestroy()
    {
        RemoteConfigService.Instance.FetchCompleted -= CompleteConfig;
    }

    #region SETUP
    private void CompleteConfig(ConfigResponse configResponse)
    {
        JsonConvertTrackAssign();
    }

    private void JsonConvertTrackAssign()
    {
        try
        {
            releaseTag = new TrackTagSupport().SetReleaseTag(RemoteConfigService.Instance.appConfig.GetJson("TrackTag_Support"));
            areaBonusTag = new TrackTagSupport2().SetBonusTag(RemoteConfigService.Instance.appConfig.GetJson("TrackTag_Support"));

            foreach (string track in releaseTag.newReleaseTrack)
                PlayerPrefs.SetInt(track + "_newReleaseTrack", 1);

            foreach (string track in areaBonusTag.areaBonusTrack)
                PlayerPrefs.SetInt(track + "_areaBonusTrack", 1);
        }
        catch { Debug.Log("Unable to connect network..."); }
    }
    #endregion

    #region EXTRA
    private IEnumerator LoadingNetworkPlayEvent()
    {
        MeloMelo_Network_RemoteConfig.ConfigurationBase playEventSetup = new MeloMelo_Network_RemoteConfig.ConfigurationSetup_PlayEvent();
        yield return StartCoroutine(playEventSetup.VerifyConfig());

        MeloMelo_Network_RemoteConfig.ConfigurationBase check_config = new MeloMelo_Network_RemoteConfig.ConfigruationSetup_VersionControl();
        yield return StartCoroutine(check_config.VerifyConfig());

        MeloMelo_ExtensionContent_Settings.LoadPlayEventRewards(PlayerPrefs.GetString("Network_Config_PlayEvent"));
    }
    #endregion
}
