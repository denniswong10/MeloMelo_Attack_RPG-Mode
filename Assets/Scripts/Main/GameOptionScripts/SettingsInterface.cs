using UnityEngine;
using UnityEngine.UI;

interface ISettings
{
    void InternalPanelBGMHandler(Slider id);
    void InternalPanelSEHandler(Slider id);
    void InternalPanelAudioMuteHandler(GameObject id);
    void InternalPanelAudioVoiceHandler(GameObject id);

    void InternalPanelFrameLimitHandler(Dropdown id);
    void InternalPanelPerformanceOptimization(Dropdown id);
    void InternalPanelCharacterAnimationHandler(GameObject id);
    void InternalPanelEnemyAnimationHandler(GameObject id);
    void InternalPanelInterfaceAnimationHandler(GameObject id);
    void InternalPanelDamageAHandler(GameObject id);
    void InternalPanelDamageBHandler(GameObject id);

    void InternalPanelCharacterHealthDisplay(Dropdown id);
    void InternalPanelEnemyHealthDisplay(Dropdown id);

    void InternalPanelSpeedMeterHandler(Dropdown id);
    void InternalPanelAirGuideChoiceHandler(Dropdown id);
    void InternalPanelJudgeTimingChoiceHandler(Dropdown id);
    void InternalPanelFancyMovement(Dropdown id);

    void InternalPanelAutoSaveProgressHandler(Dropdown id);
    void InternalPanelAutoSaveThroughPlaySettings(Dropdown id);
    void InternalPanelAutoSaveThroughOptionSettings(Dropdown id);

    void InternalPanelCodeReedem(GameObject id);
    void InternalPanelFilledCode(GameObject id);
}

public class SettingsInterface : MonoBehaviour, ISettings
{
    [SerializeField] private Slider[] AudioDataArray;
    [SerializeField] private GameObject[] AudioMasterDataArray;
    [SerializeField] private GameObject[] GraphicsCheckBoxArray;
    [SerializeField] private Dropdown[] AutoSaveConfigArray;
    [SerializeField] private Dropdown[] UnitDisplayInterfaceArray;
    [SerializeField] private Dropdown[] OtherVisualDataArray;

    private SettingsManager option;
    private Options_Menu main;

    void Start()
    {
        option = GetComponent<SettingsManager>() != null ? GetComponent<SettingsManager>() : null;
        main = GetComponent<Options_Menu>() != null ? GetComponent<Options_Menu>() : null;

        RefreshSettings();
    }

    #region SETUP
    private void RefreshSettings()
    {
        string[] audioKeyArray =
        {
             MeloMelo_PlayerSettings.GetBGM_ValueKey,
             MeloMelo_PlayerSettings.GetSE_ValueKey
        };

        string[] audioMasterSettingsArray =
        {
             MeloMelo_PlayerSettings.GetAudioMute_ValueKey,
             MeloMelo_PlayerSettings.GetAudioVoice_ValueKey
        };

        string[] graphicsKeyArray =
        {
             MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey,
             MeloMelo_PlayerSettings.GetCharacterAnimation_ValueKey,
             MeloMelo_PlayerSettings.GetEnemyAnimation_ValueKey,
             MeloMelo_PlayerSettings.GetDamageIndicatorA_ValueKey,
             MeloMelo_PlayerSettings.GetDamageIndicatorB_ValueKey
        };

        string[] unitDisplayKeyArray =
        {
             MeloMelo_PlayerSettings.GetUnitHealthOnCharacter_ValueKey,
             MeloMelo_PlayerSettings.GetUnitHealthOnEnemy_ValueKey
        };

        string[] otherVisualKeyArray =
        {
             MeloMelo_PlayerSettings.GetFrameRateLimit_ValueKey,
            MeloMelo_PlayerSettings.GetPeformanceOptimize_ValueKey,
             MeloMelo_PlayerSettings.GetAirGuide_ValueKey,
             MeloMelo_PlayerSettings.GetSpeedMeter_ValueKey,
             MeloMelo_PlayerSettings.GetFacnyMovement_ValueKey
        };

        string[] autoSaveKeyArray =
        {
            MeloMelo_PlayerSettings.GetAutoSaveProgress_ValueKey,
            MeloMelo_PlayerSettings.GetAutoSaveGameSettings_ValueKey,
            MeloMelo_PlayerSettings.GetAutoSavePlaySettings_ValueKey
        };

        if (option != null)
        {
            // Audio Toggle
            for (int audio_index = 0; audio_index < AudioDataArray.Length; audio_index++)
                option.ToggleSettingsUsingSlider(AudioDataArray[audio_index], audioKeyArray[audio_index], true, 0.5f);

            // Audio Master
            for (int audio_index = 0; audio_index < AudioMasterDataArray.Length; audio_index++)
                option.ToggleSettingsUsingCheckbox(AudioMasterDataArray[audio_index], audioMasterSettingsArray[audio_index], true);

            // Graphics Checklist
            for (int graphics_index = 0; graphics_index < GraphicsCheckBoxArray.Length; graphics_index++)
                option.ToggleSettingsUsingCheckbox(GraphicsCheckBoxArray[graphics_index], graphicsKeyArray[graphics_index], true, true);

            // Unit HUD Options
            for (int uiDisplay = 0; uiDisplay < UnitDisplayInterfaceArray.Length; uiDisplay++)
                option.ToggleSettingsUsingDropdown(UnitDisplayInterfaceArray[uiDisplay], unitDisplayKeyArray[uiDisplay], true);

            // Visual Behaviours
            for (int others = 0; others < OtherVisualDataArray.Length; others++)
                option.ToggleSettingsUsingDropdown(OtherVisualDataArray[others], otherVisualKeyArray[others], true);

            // Auto Save Checklist
            for (int autoSave_index = 0; autoSave_index < AutoSaveConfigArray.Length; autoSave_index++)
                option.ToggleSettingsUsingDropdown(AutoSaveConfigArray[autoSave_index], autoSaveKeyArray[autoSave_index], true, 1);
        }
    }
    #endregion

    #region MAIN
    public void TopPanelMainButton(int id) { if (option != null) option.SelectOptionCaterogy(id); }
    public void BottomPanelMainButton(int id) { if (option != null) option.TransitionMainExecutable(id); }
    #endregion

    #region MAIN (Audio Handler)
    public void InternalPanelBGMHandler(Slider id)
    {
        // Adjust panel settings for BGM
        if (option != null) option.ToggleSettingsUsingSlider(id, MeloMelo_PlayerSettings.GetBGM_ValueKey);
        if (main != null) main.GetModifyOfBGMAudioData(MeloMelo_PlayerSettings.GetBGM_ValueKey);
    }

    public void InternalPanelSEHandler(Slider id)
    {
        // Adjust panel settings for SE
        if (option != null) option.ToggleSettingsUsingSlider(id, MeloMelo_PlayerSettings.GetSE_ValueKey);
    }

    public void InternalPanelAudioMuteHandler(GameObject id)
    {
        // Adjust panel settings for audio to mute
        if (option != null) option.ToggleSettingsUsingCheckbox(id, MeloMelo_PlayerSettings.GetAudioMute_ValueKey);
    }

    public void InternalPanelAudioVoiceHandler(GameObject id)
    {
        // Adjust panel settings for voice audio
        if (option != null) option.ToggleSettingsUsingCheckbox(id, MeloMelo_PlayerSettings.GetAudioVoice_ValueKey);
    }
    #endregion

    #region MAIN (Graphics Handler)
    public void InternalPanelFrameLimitHandler(Dropdown id)
    {
        // Adjust of selected frame rate value
        if (option != null) option.ToggleSettingsUsingDropdown(id, MeloMelo_PlayerSettings.GetFrameRateLimit_ValueKey);
    }

    public void InternalPanelPerformanceOptimization(Dropdown id)
    {
        // Adjust panel for better game performance experience
        if (option != null) option.ToggleSettingsUsingDropdown(id, MeloMelo_PlayerSettings.GetPeformanceOptimize_ValueKey);
    }

    public void InternalPanelCharacterAnimationHandler(GameObject id)
    {
        // Adjust panel settings for character animation
        if (option != null) option.ToggleSettingsUsingCheckbox(id, MeloMelo_PlayerSettings.GetCharacterAnimation_ValueKey);
    }

    public void InternalPanelEnemyAnimationHandler(GameObject id)
    {
        // Adjust panel settings for enemy animation
        if (option != null) option.ToggleSettingsUsingCheckbox(id, MeloMelo_PlayerSettings.GetEnemyAnimation_ValueKey);
    }

    public void InternalPanelInterfaceAnimationHandler(GameObject id)
    {
        // Adjust panel settings for interface animation
        if (option != null) option.ToggleSettingsUsingCheckbox(id, MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey);
    }

    public void InternalPanelDamageAHandler(GameObject id)
    {
        // Adjust panel settings for character damage indicator
        if (option != null) option.ToggleSettingsUsingCheckbox(id, MeloMelo_PlayerSettings.GetDamageIndicatorA_ValueKey);
    }

    public void InternalPanelDamageBHandler(GameObject id)
    {
        // Adjust panel settings for enemy damage indicator
        if (option != null) option.ToggleSettingsUsingCheckbox(id, MeloMelo_PlayerSettings.GetDamageIndicatorB_ValueKey);
    }
    #endregion

    #region MAIN (Unit HUD Handler)
    public void InternalPanelCharacterHealthDisplay(Dropdown id)
    {
        // Adjust of selected preference health display for character
        if (option != null) option.ToggleSettingsUsingDropdown(id, MeloMelo_PlayerSettings.GetUnitHealthOnCharacter_ValueKey);
    }

    public void InternalPanelEnemyHealthDisplay(Dropdown id)
    {
        // Adjust of selected preference health display for enemy
        if (option != null) option.ToggleSettingsUsingDropdown(id, MeloMelo_PlayerSettings.GetUnitHealthOnEnemy_ValueKey);
    }
    #endregion

    #region MAIN (Visual Handler)
    public void InternalPanelSpeedMeterHandler(Dropdown id)
    {
        // Adjust of selected preference in ticks through audio cue for every beat
        if (option != null) option.ToggleSettingsUsingDropdown(id, MeloMelo_PlayerSettings.GetSpeedMeter_ValueKey);
    }

    public void InternalPanelAirGuideChoiceHandler(Dropdown id)
    {
        // Adjust of selected preference in notation guidline through gameplay
        if (option != null) option.ToggleSettingsUsingDropdown(id, MeloMelo_PlayerSettings.GetAirGuide_ValueKey);
    }

    public void InternalPanelJudgeTimingChoiceHandler(Dropdown id)
    {
        // Adjust of selected preference in judgement cue-offset through gameplay
        if (option != null) option.ToggleSettingsUsingDropdown(id, MeloMelo_PlayerSettings.GetJudgeTimingOffset_ValueKey);
    }

    public void InternalPanelFancyMovement(Dropdown id)
    {
        // Adjust of selected preference mode of notation motion behaviour
        if (option != null) option.ToggleSettingsUsingDropdown(id, MeloMelo_PlayerSettings.GetFacnyMovement_ValueKey);
    }
    #endregion

    #region MAIN (AutoSave Handler)
    public void InternalPanelAutoSaveProgressHandler(Dropdown id)
    {
        // Manage auto save progress in game
        if (option != null) option.ToggleSettingsUsingDropdown(id, MeloMelo_PlayerSettings.GetAutoSaveProgress_ValueKey);
    }

    public void InternalPanelAutoSaveThroughPlaySettings(Dropdown id)
    {
        // Manage auto save in player settings everytime its save
        if (option != null) option.ToggleSettingsUsingDropdown(id, MeloMelo_PlayerSettings.GetAutoSavePlaySettings_ValueKey);
    }

    public void InternalPanelAutoSaveThroughOptionSettings(Dropdown id)
    {
        // Manage auto save in game settings everytime its save
        if (option != null) option.ToggleSettingsUsingDropdown(id, MeloMelo_PlayerSettings.GetAutoSaveGameSettings_ValueKey);
    }
    #endregion

    #region MAIN (Account Handler)
    public void InternalPanelLogoutHanlder()
    {
        if (option != null)
        {
            option.ResetLoginIdentify();
            option.SubTransitionExecutable(0);
        }
    }

    public void InternalPanelProfileHandler() 
    { 
        if (option != null) option.SubTransitionExecutable(1);
    }
    #endregion

    #region MAIN (Data Handler)
    public void InternalPanelCodeReedem(GameObject id)
    {
        if (main != null) StartCoroutine(main.AwaitForCodeTransfering(id));
    }

    public void InternalPanelFilledCode(GameObject id)
    {
        id.transform.GetChild(1).GetComponent<Button>().interactable = id.transform.GetChild(2).GetComponent<InputField>().text != string.Empty;
        id.transform.GetChild(2).GetComponent<InputField>().text = id.transform.GetChild(2).GetComponent<InputField>().text.ToUpper();
    }
    #endregion
}
