using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Options_Menu : MonoBehaviour
{
    private GameObject[] BGM;
    public Dropdown Res;
    public GameObject Icon;

    #region SETUP
    void CallUpOptionTask()
    {
        // Audio
        Update_OptionControl("BGM_Options", "BGM_VolumeGET", true);
        Update_OptionControl("SE_Options", "SE_VolumeGET", true);

        // Gameplay Features
        Update_OptionControl("SpeedMeter", "SpeedMeter_valve", false);
        Update_OptionControl("AirGuide", "AirGuide_valve", false);
        Update_OptionControl("RateSystem", "RateSystem_valve", false);

        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CallUpOptionTask();
    }

    // Button Interaction - Options

    #region MAIN
    public void Option_InteractANDTransition(string scene)
    {
        // Option - Back (Menu), Credit (Credits)
        // Credit - Review (Review)

        if (scene == "LoginPage") PlayerPrefs.DeleteKey("UserID_Login");
        SceneManager.LoadScene(scene);
    }

    public void ProfileViewOption()
    {
        //Application.OpenURL(PlayerPrefs.GetString("GameWeb_URL", string.Empty) + "database/transcripts/site7/database/games/MeloMelo_Listing/eNets.php");
        SceneManager.LoadScene("ProfileView");
    }

    public void Option_SettingsControl(string option)
    {
        // SpeedMeter_Button - SpeedMeter_Display       AirGuide_Display - AirGuide_Display
        // RateSystem_Button - RateMeter_Controller
        PlayerPrefs.SetInt(option + "_valve", GameObject.Find(option).GetComponent<Dropdown>().value);
    }

    public void Option_AudioControls(int index)
    {
        if (index == 1)
        {
            PlayerPrefs.SetFloat("BGM_VolumeGET", GameObject.Find("BGM_Options").GetComponent<Slider>().value);
            Update_OptionControl("BGM_Options", "BGM_VolumeGET", true, true);
        }
        else
        {
            PlayerPrefs.SetFloat("SE_VolumeGET", GameObject.Find("SE_Options").GetComponent<Slider>().value);
            Update_OptionControl("SE_Options", "SE_VolumeGET", true);
        }
    }

    public void JudgeTimingWindowOffset(Dropdown material)
    {
        MeloMelo_GameSettings.GetInputDelaySelection = material.value;
    }
    #endregion

    void GetBGMUpdateAudio()
    {
        try { GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().volume = GameObject.Find("BGM_Options").GetComponent<Slider>().value; }
        catch { }
    }

    // Menu - Update Content
    private void Update_OptionControl(string obj, string option, bool isFloat, bool BGM = false)
    {
        // (float)          BGM_Options - BGM_VolumeGET          SE_Options - SE_VolumeGET
        // (int)            SpeedMeter - SpeedMeter_Display      AirGuide - AirGuide_Display
        if (isFloat)
        {
            GameObject.Find(obj).GetComponent<Slider>().value = PlayerPrefs.GetFloat(option, 0.5f);
            GameObject.Find(obj).GetComponent<Slider>().value = PlayerPrefs.GetFloat(option, 0.5f);
        }
        else
        {
            GameObject.Find(obj).GetComponent<Dropdown>().value = PlayerPrefs.GetInt(option, 0);
            GameObject.Find(obj).GetComponent<Dropdown>().value = PlayerPrefs.GetInt(option, 0);
        }

        if (BGM) GetBGMUpdateAudio();
    }
}
