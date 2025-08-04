using System.Collections;
using System.Collections.Generic;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using UnityEngine;
using Newtonsoft.Json;

public class ExchangeZoneSetup : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    private string packageDataEncoder;
    private string versionArray;

    #region SETUP
    private void GetExchangeZoneData()
    {
        // Preset the data zone before allowing access to code
        packageDataEncoder = RemoteConfigService.Instance.appConfig.GetJson("MeloMelo_Exchange_Management");
        versionArray = RemoteConfigService.Instance.appConfig.GetJson("CloudSave_Support");
        PlayerPrefs.SetString("ExchangeCode_TempData", packageDataEncoder);
        PlayerPrefs.SetString("ExchangeCode_TempData_Version", versionArray);

        // Ready for review
        PlayerPrefs.SetInt("ExchangeLoader_Ready", 1);
    }

    private async void GetSetupReady()
    {
        // Only active to guest loign
        if (ServerGateway_Script.thisServer.get_loginType == (int)MeloMelo_PlayerSettings.LoginType.GuestLogin)
        {
            // Refresh the latest cloud data to local data
            await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
            GetExchangeZoneData();
        }
    }
    #endregion

    #region MAIN
    public void RefereshExchangeZone()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            PlayerPrefs.DeleteKey("ExchangeLoader_Ready");
            GetSetupReady();
        }
    }
    #endregion
}
