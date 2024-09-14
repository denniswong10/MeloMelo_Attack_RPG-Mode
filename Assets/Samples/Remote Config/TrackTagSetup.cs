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
        if (ServerGateway_Script.thisServer.get_loginType == (int)MeloMelo_GameSettings.LoginType.GuestLogin && AuthenticationService.Instance.IsSignedIn)
        {
            await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
            JsonConvertTrackAssign();
        }
    }

    #region SETUP
    private void JsonConvertTrackAssign()
    {
        releaseTag = new TrackTagSupport().SetReleaseTag(RemoteConfigService.Instance.appConfig.GetJson("TrackTag_Support"));
        areaBonusTag = new TrackTagSupport2().SetBonusTag(RemoteConfigService.Instance.appConfig.GetJson("TrackTag_Support"));

        foreach (string track in releaseTag.newReleaseTrack)
            PlayerPrefs.SetInt(track + "_newReleaseTrack", 1);

        foreach (string track in areaBonusTag.areaBonusTrack)
            PlayerPrefs.SetInt(track + "_areaBonusTrack", 1);
    }
    #endregion
}
