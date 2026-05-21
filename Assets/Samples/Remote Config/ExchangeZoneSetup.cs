using System.Collections;
using System.Collections.Generic;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using UnityEngine;
using MeloMelo_Network_RemoteConfig;

public class ExchangeZoneSetup : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    private string packageDataEncoder;
    private string versionArray;

    #region SETUP
    private void GetLocalConfigData()
    {
        // Preset the data zone before allowing access to code
        packageDataEncoder = RemoteConfigService.Instance.appConfig.GetJson("MeloMelo_Exchange_Management");
        versionArray = RemoteConfigService.Instance.appConfig.GetJson("CloudSave_Support");
        PlayerPrefs.SetString("ExchangeCode_TempData", packageDataEncoder);
        PlayerPrefs.SetString("ExchangeCode_TempData_Version", versionArray);

        // Ready for review
        PlayerPrefs.SetInt("ExchangeLoader_Ready", 1);
    }

    private IEnumerator GetNetworkConfigData()
    {
        ConfigurationBase setup_config = new ConfigurationSetup_ExchangePoint();
        yield return StartCoroutine(setup_config.VerifyConfig());

        ConfigurationBase check_config = new ConfigruationSetup_VersionControl();
        yield return StartCoroutine(check_config.VerifyConfig());

        // Ready for review
        PlayerPrefs.SetInt("ExchangeLoader_Ready", 1);
    }

    private async void GetSetupReady()
    {
        switch (ServerGateway_Script.thisServer.get_loginType)
        {
            case (int)MeloMelo_PlayerSettings.LoginType.GuestLogin:
                // Refresh the latest cloud data to local data
                await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
                GetLocalConfigData();
                break;

            default:
                StartCoroutine(GetNetworkConfigData());
                break;
        }
    }
    #endregion

    #region MAIN
    public void RefereshExchangeZone()
    {
        PlayerPrefs.DeleteKey("ExchangeLoader_Ready");
        GetSetupReady();

        //if (AuthenticationService.Instance.IsSignedIn)
        //{
        //    PlayerPrefs.DeleteKey("ExchangeLoader_Ready");
        //    GetSetupReady();
        //}
    }
    #endregion
}
