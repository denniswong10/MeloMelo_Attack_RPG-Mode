using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using UnityEngine;

public class GameplaySettingConfig : MonoBehaviour
{
    private bool isLogin = false;

    [System.Serializable]
    struct NoteSpeed_Default_Settings
    {
        public NoteSpeed_Settings data;

        public void GetSettingFormat(string format)
        {
            Debug.Log(format);
            data = JsonUtility.FromJson<NoteSpeed_Settings>(format);
        }
    }

    [System.Serializable]
    struct NoteSpeed_Array_Settings
    {
        public NoteSpeed_Settings[] data;

        public NoteSpeed_Array_Settings GetSettingsFormat(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<NoteSpeed_Array_Settings>(format);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        try { isLogin = AuthenticationService.Instance.IsSignedIn; } catch { Debug.Log("Remote Config: OFFLINE"); }
        Invoke("GetConfigOnGoing", 2.1f);
    }

    #region SETUP
    private void GetConfigOnGoing()
    {
        if (BeatConductor.thisBeat.get_noteSpeed == 0)
            BeatConductor.thisBeat.UpdateNoteSpeed(
                GetRemoteConfigurationOfNoteSpeed((int)BeatConductor.thisBeat.Music_Database.BPM, PlayerPrefs.GetInt("NoteSpeed", 0))
                );
    }
    #endregion

    #region COMPONENT
    private int GetRemoteConfigurationOfNoteSpeed(int bpmState, int index)
    {
        if (isLogin)
        {
            int selectedSpeed = 0;
            string jsonForamt = RemoteConfigService.Instance.appConfig.GetJson("NoteSpeed_Settings_Configuration");
            string jsonFormat_default = RemoteConfigService.Instance.appConfig.GetJson("NoteSpeed_Default_Configuration");

            selectedSpeed = GetNoteSpeedInArrayForm(bpmState, jsonForamt);

            if (selectedSpeed == 0)
                selectedSpeed = GetNoteSpeedInSingleForm(bpmState, jsonFormat_default);

            Debug.Log("Use config through network: NoteSpeed[" + index + ", " + selectedSpeed +"]");
            return selectedSpeed + (index * 5);
        }
        else
        {
            int selectedSpeed = 0;

            MeloMelo_Local.LocalLoad_DataManagement setting = new MeloMelo_Local.LocalLoad_DataManagement
                (
                    string.Empty,
                    "StreamingAssets/PlaySettings"
                );

            selectedSpeed = GetNoteSpeedInArrayForm(bpmState, setting.GetLocalJsonFile("MeloMelo_NoteSpeed_Configuration.json"));

            if (selectedSpeed == 0)
                selectedSpeed = GetNoteSpeedInSingleForm(bpmState, setting.GetLocalJsonFile("MeloMelo_NoteSpeed_Default_Configuration.json"));

            Debug.Log("Use config through local: NoteSpeed[" + index + ", " + selectedSpeed + "]");
            return selectedSpeed + (index * 5);
        }
    }

    private int GetNoteSpeedInArrayForm(int state, string dataString)
    {
        int speed = 0;
        NoteSpeed_Array_Settings settings = new NoteSpeed_Array_Settings();
        settings = settings.GetSettingsFormat(dataString);

        for (int i = 0; i < settings.data.Length; i++)
        {
            if (state >= settings.data[i].bpm)
            {
                speed = settings.data[i].baseSpeed;
                break;
            }
        }

        return speed;
    }

    private int GetNoteSpeedInSingleForm(int state, string dataString)
    {
        NoteSpeed_Default_Settings setting = new NoteSpeed_Default_Settings();
        setting.GetSettingFormat(dataString);

        if (state <= setting.data.bpm) return setting.data.baseSpeed;
        return 0;
    }
    #endregion
}
