using System;
using UnityEngine;
using Unity.Services.RemoteConfig;
using UnityEngine.UI;
using Unity.Services.Core;

public class PlayEvent_ServicesConfig : MonoBehaviour
{
    public struct UserAttributes { }
    public struct AppAttributes { }

    public static bool IsEventActive { get; private set; }

    private DateTime eventStartUtc;
    private DateTime eventEndUtc;

    [SerializeField] private Text TimeStamp_display;
    [SerializeField] private GameObject EventInformationBoard;

    // Start is called before the first frame update
    async void Start()
    {
        RemoteConfigService.Instance.FetchCompleted += PlayEventReceivedInfo;
        await RemoteConfigService.Instance.FetchConfigsAsync(new UserAttributes(), new AppAttributes());
    }

    // Update is called once per frame
    void OnDestroy()
    {
        RemoteConfigService.Instance.FetchCompleted -= PlayEventReceivedInfo;
    }

    #region SETUP
    private void GetTimeStamp()
    {
        TimeSpan remaining = eventEndUtc - GetSafeUtcNow();

        if (remaining.TotalSeconds > 0)
        {
            TimeStamp_display.text =
                $"{remaining.Days}days {remaining.Hours}hours {remaining.Minutes}minutes";
        }
    }
    #endregion

    #region MAIN
    private void PlayEventReceivedInfo(ConfigResponse receive)
    {
        //try
        //{
        //    bool eventEnabled =
        //        RemoteConfigService.Instance.appConfig.GetBool("event_enabled", false);

        //    if (!eventEnabled)
        //    {
        //        IsEventActive = false;
        //        Debug.Log("No event are active");
        //        return;
        //    }

        //    string startString =
        //        RemoteConfigService.Instance.appConfig.GetString("event_start_utc", "");

        //    string endString =
        //        RemoteConfigService.Instance.appConfig.GetString("event_end_utc", "");

        //    if (!TryParseUtc(startString, out eventStartUtc) ||
        //        !TryParseUtc(endString, out eventEndUtc))
        //    {
        //        Debug.LogWarning("Invalid event date config");
        //        IsEventActive = false;
        //        return;
        //    }

        //    DateTime nowUtc = GetSafeUtcNow();

        //    IsEventActive = nowUtc >= eventStartUtc && nowUtc < eventEndUtc;
        //    Debug.Log("Start: " + startString + " | End: " + endString);
        //    GetTimeStamp();
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError($"Remote Config event check failed: {e}");
        //    IsEventActive = false;
        //}

        JsonConvertPlayEvent();
    }

    private void JsonConvertPlayEvent()
    {
        // Refresh: Every time event is started or ended
        switch (ServerGateway_Script.thisServer.get_loginType)
        {
            case (int)MeloMelo_PlayerSettings.LoginType.GuestLogin:
                MeloMelo_ExtensionContent_Settings.LoadPlayEventSetup(RemoteConfigService.Instance.appConfig.GetJson("MeloMelo_PlayEvent_Setup"));
                MeloMelo_ExtensionContent_Settings.LoadPlayEventRewards(RemoteConfigService.Instance.appConfig.GetJson("MeloMelo_PlayEvent_Reward_2"));
                if (MeloMelo_ExtensionContent_Settings.GetEventRewardArray() != null) EventInformationBoard.SetActive(true);
                break;

            default:
                //Debug.Log("Play Event: " + (PlayerPrefs.GetString("Network_Config_PlayEvent", string.Empty) != string.Empty ? "OPEN!" : "CLOSED!"));
                //Cloud_Config_PlayEvent play_event_data = JsonUtility.FromJson<Cloud_Config_PlayEvent>(PlayerPrefs.GetString("Network_Config_PlayEvent", string.Empty));
                //Debug.Log("Event Reward Count: " + (play_event_data != null ? play_event_data.config.Length : -1));
                if (MeloMelo_ExtensionContent_Settings.GetEventRewardArray() != null) EventInformationBoard.SetActive(true);
                break;
        }
    }
    #endregion

    #region MISC
    private bool TryParseUtc(string value, out DateTime utcTime)
    {
        return DateTime.TryParse(
            value,
            null,
            System.Globalization.DateTimeStyles.AdjustToUniversal |
            System.Globalization.DateTimeStyles.AssumeUniversal,
            out utcTime
        );
    }

    private DateTime GetSafeUtcNow()
    {
        return DateTime.UtcNow;
    }
    #endregion
}
