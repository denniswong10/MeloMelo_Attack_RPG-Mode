using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MeloMelo_Network_RemoteConfig
{
    public abstract class ConfigurationBase
    {
        protected bool isConfigOK;
        protected string directory = MeloMelo_PlayerSettings.GetWebServerUrl() + "/database/transcripts/site5/melomelo_backend/gameHub_backend/MeloMelo/";

        protected abstract string API_Address_Point { get; }
        protected abstract void OnConfigLoaded(string data);

        public IEnumerator VerifyConfig()
        {
            isConfigOK = false;
            Debug.Log("Connection: " + directory + API_Address_Point);

            UnityWebRequest request = UnityWebRequest.Get(directory + API_Address_Point);
            yield return request.SendWebRequest();

            if (request.downloadHandler.text.Trim('{', '}') != string.Empty)
            {
                OnConfigLoaded(request.downloadHandler.text);
            }

            isConfigOK = true;
        }

        public bool GetConfigComplete() { return isConfigOK; }
    }

    public class ConfigruationSetup_VersionControl : ConfigurationBase
    {
        protected override string API_Address_Point =>
            MeloMelo_SQL_Services_GameConfiguration.MeloMelo_Config_VersionControl;

        protected override void OnConfigLoaded(string data)
        {
            PlayerPrefs.SetString("ExchangeCode_TempData_Version", data);
            PlayerPrefs.SetString("VersionControl_PlayEvent", data);
        }
    }

    public class ConfigurationSetup_ExchangePoint : ConfigurationBase
    {
        protected override string API_Address_Point =>
            MeloMelo_SQL_Services_GameConfiguration.MeloMelo_Config_ExchangePoint;

        protected override void OnConfigLoaded(string data)
        {
            PlayerPrefs.SetString("ExchangeCode_TempData", data);
        }
    }

    public class ConfigurationSetup_PlayEvent : ConfigurationBase
    {
        protected override string API_Address_Point =>
            MeloMelo_SQL_Services_GameConfiguration.MeloMelo_Config_PlayEvent;

        protected override void OnConfigLoaded(string data)
        {
            PlayerPrefs.SetString("Network_Config_PlayEvent", data);
        }
    }

    public class ConfigurationSetup_MarathonExchange : ConfigurationBase
    {
        protected override string API_Address_Point =>
            MeloMelo_SQL_Services_GameConfiguration.MeloMelo_Config_MarathonExchange;

        protected override void OnConfigLoaded(string data)
        {
            PlayerPrefs.SetString("JSON_Custom_Marathon_Exchange", data);
        }
    }

    public class ConfigurationSetup_MarathonContent : ConfigurationBase
    {
        protected override string API_Address_Point =>
            MeloMelo_SQL_Services_GameConfiguration.MeloMelo_Config_MarathonContent;

        protected override void OnConfigLoaded(string data)
        {
            PlayerPrefs.SetString("JSON_Custom_Marathon_Challenge", data);
        }
    }
}
