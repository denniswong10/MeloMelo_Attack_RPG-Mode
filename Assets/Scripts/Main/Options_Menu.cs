using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MeloMelo_Local;

struct BundleOptionSettings
{
    public GameObject optionButton;
    public GameObject[] optionFieldTab;
}

[System.Serializable]
struct DataPackStructure
{
    public string packageName;
    public string dataSeriesKey;
    public string unqiueCode;
    public string version;
}

[System.Serializable]
struct DataArrayBundle
{
    public DataPackStructure[] data;

    public DataArrayBundle GetBundle(string jsonData)
    {
        Debug.Log(jsonData);
        return JsonUtility.FromJson<DataArrayBundle>(jsonData);
    }
}

[System.Serializable]
struct VersionControlArray
{
    public string[] versions;

    public VersionControlArray GetAllData(string jsonData)
    {
        Debug.Log(jsonData);
        return JsonUtility.FromJson<VersionControlArray>(jsonData);
    }
}

public class Options_Menu : MonoBehaviour
{
    private GameObject[] BGM;
    private SettingsScripts mainOptions;
    private string currentOptionScripted;

    [SerializeField] private Slider[] AuidoDataArray;
    [SerializeField] private GameObject[] AuidoMasterDataArray;
    [SerializeField] private GameObject[] GraphicsCheckBoxArray;
    [SerializeField] private Dropdown[] AutoSaveConfigArray;

    [SerializeField] private GameObject AlertPop;
    [SerializeField] private GameObject ChangePanel;
    [SerializeField] private Text MessageChangePanel;
    [SerializeField] private GameObject Icon;

    void Start()
    {
        LoadBGM();
        BuildOptionScripts();

        LoadCurrentChangeForReference();
        RefreshOptionSettingsData();
    }

    #region SETUP
    #region MISC:
    private void LoadBGM()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
        PlayerPrefs.SetInt("ReviewOption", 1);
    }

    private void GetModifyOfBGMAudioData(string bgm_key)
    {
        AudioSource audio = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        if (audio) audio.volume = PlayerPrefs.GetFloat(bgm_key, 0.5f);
    }

    private void UpdatePlayerChangesIndicator(bool isSaved)
    {
        MessageChangePanel.text = isSaved ? "Pending Changes" : "All Changes Saved";
    }
    #endregion

    #region INTILLIZATE SETTINGS SCENE:
    private void BuildOptionScripts()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Options":
                mainOptions = new SettingsScripts();
                break;

            default:
                break;
        }
    }

    private void RefreshOptionSettingsData()
    {
        // Update direct to mainOption
        if (mainOptions != null)
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

            string[] autoSaveKeyArray =
            {
                MeloMelo_PlayerSettings.GetAutoSaveProgress_ValueKey,
                MeloMelo_PlayerSettings.GetAutoSaveGameSettings_ValueKey,
                MeloMelo_PlayerSettings.GetAutoSavePlaySettings_ValueKey
            };

            for (int audio_index = 0; audio_index < AuidoDataArray.Length; audio_index++)
                mainOptions.AdjustHandlerOfAudio(AuidoDataArray[audio_index], audioKeyArray[audio_index]);

            for (int audio_index = 0; audio_index < AuidoMasterDataArray.Length; audio_index++)
                mainOptions.ChangeOfOption_AudioMasterSettings(AuidoMasterDataArray[audio_index], 
                    audioMasterSettingsArray[audio_index], GetAudioValueSetted(audio_index));

            for (int graphics_index = 0; graphics_index < GraphicsCheckBoxArray.Length; graphics_index++)
                mainOptions.ChangeOfOptions_GameplayContent(GraphicsCheckBoxArray[graphics_index],
                    graphicsKeyArray[graphics_index], GetGraphicsValueSetted(graphics_index));

            for (int autoSave_index = 0; autoSave_index < AutoSaveConfigArray.Length; autoSave_index++)
            {
                AutoSaveConfigArray[autoSave_index].value = GetAutoSaveValueSetted(autoSave_index) ? 1 : 0;
                mainOptions.ChangeOfOptions_AccountConfiguration(AutoSaveConfigArray[autoSave_index], autoSaveKeyArray[autoSave_index]);
            }

            UpdatePlayerChangesIndicator(IsSavedOptionChangeWanted());
        }
    }

    private bool GetGraphicsValueSetted(int data_id)
    {
        PlayerSettingsDatabase settingsData = JsonUtility.FromJson<PlayerSettingsDatabase>(currentOptionScripted);
        switch (data_id)
        {
            case 0:
                return settingsData.allowIntefaceAnimation;

            case 1:
                return settingsData.allowCharacterAnimation;

            case 2:
                return settingsData.allowEnemyAnimation;

            case 3:
                return settingsData.allowDamageIndicatorOnAlly;

            case 4:
                return settingsData.allowDamageIndicatorOnEnemy;

            default:
                return false;
        }
    }

    private bool GetAudioValueSetted(int data_id)
    {
        PlayerSettingsDatabase settingsData = JsonUtility.FromJson<PlayerSettingsDatabase>(currentOptionScripted);
        switch (data_id)
        {
            case 0:
                return settingsData.audio_mute_data;

            case 1:
                return settingsData.audio_voice_data;

            default:
                return false;
        }
    }

    private bool GetAutoSaveValueSetted(int data_id)
    {
        PlayerSettingsDatabase settingsData = JsonUtility.FromJson<PlayerSettingsDatabase>(currentOptionScripted);
        switch (data_id)
        {
            case 0:
                return settingsData.autoSaveGameProgress;

            case 1:
                return settingsData.autoSaveGameSettings;

            case 2:
                return settingsData.autoSavePlaySettings;

            default:
                return false;
        }
    }
    #endregion
    #endregion

    #region MAIN
    public void GlobalScene_OnTransition(string id) { SceneManager.LoadScene(id); }
    #endregion

    #region COMPONENT (SettingsScripts)
    #region MAIN: BUTTON
    public void TopPanelMainButton(int id) { if (mainOptions != null) mainOptions.SelectOptionCaterogy(id); }
    public void BottomPanelMainButton(int id)
    {
        if (mainOptions != null)
        {
            bool isPendingChanges = IsSavedOptionChangeWanted();
            ChangePanel.SetActive(isPendingChanges);
            UpdatePlayerChangesIndicator(isPendingChanges);
            if (!ChangePanel.activeInHierarchy) mainOptions.TransitionMainExecutable(id);
        }
    }
    #endregion

    #region SUB: CHANGE PANEL
    private void SaveCurrentChangeForReference()
    {
        // Saving of new changes
        LocalSave_DataManagement savedChanges =
            new LocalSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
            "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

        // Save change through files
        savedChanges.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileAccountSettings);
        savedChanges.SaveAccountSettings();
    }

    private void LoadCurrentChangeForReference()
    {
        // Load of current changes
        LocalLoad_DataManagement loadChanges = 
            new LocalLoad_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
            "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

        // Load changes for reference
        currentOptionScripted = loadChanges.GetLocalJsonFile(MeloMelo_GameSettings.GetLocalFileAccountSettings, true);
    }

    private void ForceSavedOptionChange(string data)
    {
        // Get load options
        CloudDatabase_Local_DataManagement optionToSave =
            new CloudDatabase_Local_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
            "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

        // Saved current changes from data
        optionToSave.SaveJsonFormatToLocalData(MeloMelo_GameSettings.GetLocalFileAccountSettings, data);
    }
    
    private bool IsSavedOptionChangeWanted()
    {
        // Save all changes and keep the original data saved
        PlayerPrefs.SetString("CurrentOriginalChanges", currentOptionScripted);
        SaveCurrentChangeForReference();

        // Overwritten the currentOptionSripted data and check if the data is equal to the original data
        LoadCurrentChangeForReference();
        return PlayerPrefs.GetString("CurrentOriginalChanges", string.Empty) != currentOptionScripted;
    }

    public void SubPanelSaveChanges()
    {
        // Do nothings if changes are okay
        UpdatePlayerChangesIndicator(IsSavedOptionChangeWanted());
        ChangePanel.SetActive(false);
    }
    public void SubPanelDiscardChanges()
    {
        // Revert the changes by replacing the new data to the original one
        ForceSavedOptionChange(PlayerPrefs.GetString("CurrentOriginalChanges", string.Empty));
        UpdatePlayerChangesIndicator(IsSavedOptionChangeWanted());
        ChangePanel.SetActive(false);
    }
    #endregion

    #region Audio:
    public void InternalPanelBGMHandler(GameObject id) 
    { 
        // Slider: For BGM
        if (mainOptions != null) mainOptions.AdjustHandlerOfAudio(id.GetComponent<Slider>(), MeloMelo_PlayerSettings.GetBGM_ValueKey);
        GetModifyOfBGMAudioData(id.name);
    }
    public void InternalPanelSEHandler(GameObject id) 
    { 
        // Slider: For SE
        if (mainOptions != null) mainOptions.AdjustHandlerOfAudio(id.GetComponent<Slider>(), MeloMelo_PlayerSettings.GetSE_ValueKey); 
    }
    public void InternalPanelAudioMuteHandler(GameObject id)
    {
        if (mainOptions != null)
        {
            // CheckBox: For Audio Mute
            bool isChecked = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAudioMute_ValueKey, 1) == 1 ? true : false;
            mainOptions.ChangeOfOptions_GameplayContent(id, MeloMelo_PlayerSettings.GetAudioMute_ValueKey, !isChecked ? true : false);
        }
    }
    public void InternalPanelAudioVoiceHandler(GameObject id)
    {
        if (mainOptions != null)
        {
            // CheckBox: For Audio Voice
            bool isChecked = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAudioVoice_ValueKey, 1) == 1 ? true : false;
            mainOptions.ChangeOfOptions_GameplayContent(id, MeloMelo_PlayerSettings.GetAudioVoice_ValueKey, !isChecked ? true : false);
        }
    }
    #endregion

    #region Graphics:
    public void InternalPanelCharacterAnimationHanlder(GameObject id)
    {
        if (mainOptions != null)
        {
            // CheckBox: For Character Animation
            bool isChecked = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetCharacterAnimation_ValueKey, 1) == 1 ? true : false;
            mainOptions.ChangeOfOptions_GameplayContent(id, MeloMelo_PlayerSettings.GetCharacterAnimation_ValueKey, !isChecked ? true : false);
        }
    }
    public void InternalPanelEnemyAnimationHanlder(GameObject id)
    {
        if (mainOptions != null)
        {
            // CheckBox: For Enemy Animation
            bool isChecked = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetEnemyAnimation_ValueKey, 1) == 1 ? true : false;
            mainOptions.ChangeOfOptions_GameplayContent(id, MeloMelo_PlayerSettings.GetEnemyAnimation_ValueKey, !isChecked ? true : false);
        }
    }
    public void InternalPanelInterfaceAnimationHanlder(GameObject id)
    {
        if (mainOptions != null)
        {
            // CheckBox: For Interface Animation
            bool isChecked = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey, 1) == 1 ? true : false;
            mainOptions.ChangeOfOptions_GameplayContent(id, MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey, !isChecked ? true : false);
        }
    }
    public void InternalPanelDamageAHanlder(GameObject id)
    {
        if (mainOptions != null)
        {
            // CheckBox: For Damage Indicator (Ally)
            bool isChecked = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetDamageIndicatorA_ValueKey, 1) == 1 ? true : false;
            mainOptions.ChangeOfOptions_GameplayContent(id, MeloMelo_PlayerSettings.GetDamageIndicatorA_ValueKey, !isChecked ? true : false);
        }
    }
    public void InternalPanelDamageBHanlder(GameObject id)
    {
        if (mainOptions != null)
        {
            // CheckBox: For Damage Indicator (Enemy)
            bool isChecked = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetDamageIndicatorB_ValueKey, 1) == 1 ? true : false;
            mainOptions.ChangeOfOptions_GameplayContent(id, MeloMelo_PlayerSettings.GetDamageIndicatorB_ValueKey, !isChecked ? true : false);
        }
    }

    public void InternalPanelAirGuideChoiceHandler(GameObject id)
    { 

    }
    public void InternalPanelJudgeTimingChoiceHandler(GameObject id)
    { 

    }
    #endregion

    #region Account:
    public void InternalPanelAutoSaveProgressHandler(GameObject id)
    {
        if (mainOptions != null)
        {
            // Dropdown: For Auto Save Database (Progress)
            mainOptions.ChangeOfOptions_AccountConfiguration(id.GetComponent<Dropdown>(), MeloMelo_PlayerSettings.GetAutoSaveProgress_ValueKey);
        }
    }
    public void InternalPanelAutoSaveThroughPlaySettings(GameObject id)
    {
        if (mainOptions != null)
        {
            // Dropdown: For Auto Save Database (PlaySettings)
            mainOptions.ChangeOfOptions_AccountConfiguration(id.GetComponent<Dropdown>(), MeloMelo_PlayerSettings.GetAutoSavePlaySettings_ValueKey);
        }
    }
    public void InternalPanelAutoSaveThroughOptionSettings(GameObject id)
    {
        if (mainOptions != null)
        {
            // Dropdown: For Auto Save Database (GameSettings)
            mainOptions.ChangeOfOptions_AccountConfiguration(id.GetComponent<Dropdown>(), MeloMelo_PlayerSettings.GetAutoSaveGameSettings_ValueKey);
        }
    }
    public void InternalPanelLogoutHanlder() 
    {
        if (mainOptions != null)
        {
            mainOptions.ResetLoginIdentify();
            mainOptions.SubTransitionExecutable(0);
        }    
    }
    public void InternalPanelProfileHandler() { if (mainOptions != null) mainOptions.SubTransitionExecutable(1); }
    #endregion

    #region Data:
    public void InternalPanelCodeReedem(GameObject id)
    {
        if (mainOptions != null)
        {
            // Button: For code redeeming
            StartCoroutine(AwaitForCodeTransfering(id));
        }
    }

    public void InternalPanelFilledCode(GameObject id)
    {
        id.transform.GetChild(1).GetComponent<Button>().interactable = id.transform.GetChild(2).GetComponent<InputField>().text != string.Empty;
        id.transform.GetChild(2).GetComponent<InputField>().text = id.transform.GetChild(2).GetComponent<InputField>().text.ToUpper();
    }

    private IEnumerator AwaitForCodeTransfering(GameObject id)
    {
        id.transform.GetChild(1).GetComponent<Button>().interactable = false;
        yield return new WaitUntil(() => PlayerPrefs.HasKey("ExchangeLoader_Ready"));
        mainOptions.ConfirmCode(id);


        if (!AlertPop.activeInHierarchy && PlayerPrefs.HasKey("AlertPop_Message")) StartCoroutine(DisplayAlertPop());
        else AlertPop.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("AlertPop_Message", string.Empty);
    }

    private IEnumerator DisplayAlertPop()
    {
        AlertPop.SetActive(true);
        AlertPop.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("AlertPop_Message", string.Empty);
        yield return new WaitForSeconds(5);
        AlertPop.SetActive(false);
    }
    #endregion
    #endregion

    #region MISC
    public bool IsProcessStillPending() { return Icon.activeInHierarchy; }

    public IEnumerator GetOptionMessage(string message)
    {
        Icon.SetActive(true);
        Icon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\n" + message;
        yield return new WaitForSeconds(3);
        Icon.SetActive(false);
    }
    #endregion
}

public class SettingsScripts
{
    private readonly string[] sceneTransitionList = { "Menu", "Controls", "Credits" };
    private readonly string[] sceneSubTransitionList = { "ProfileView", "ServerGateway" };

    private List<BundleOptionSettings> optionSettings;
    private int currentOptionCaterogy;

    public SettingsScripts()
    {
        optionSettings = new List<BundleOptionSettings>();
        GerenateSettingsData(GameObject.FindGameObjectsWithTag("Option_TopPanel_Button"));
        optionSettings.Sort((optionA, optionB) => optionA.optionButton.name.CompareTo(optionB.optionButton.name));
        SelectOptionCaterogy(0);
    }

    #region SETUP
    private void GerenateSettingsData(GameObject[] mainOptions)
    {
        if (optionSettings != null)
        {
            foreach (GameObject option in mainOptions)
            {
                List<GameObject> optionGroup = new List<GameObject>();
                int toggleOverIndex = 1;

                while (GameObject.Find(option.name + "_" + toggleOverIndex))
                {
                    optionGroup.Add(GameObject.Find(option.name + "_" + toggleOverIndex));
                    toggleOverIndex++;
                }

                //Debug.Log("Current search: " + option.name + " found");
                AddSettingsData(option, optionGroup.ToArray());
            }
        }
    }

    private void AddSettingsData(GameObject target, GameObject[] token)
    {
        BundleOptionSettings setOption = new BundleOptionSettings();
        setOption.optionButton = target;
        setOption.optionFieldTab = token;
        optionSettings.Add(setOption);
        //Debug.Log("Option_Data: " + token.Length + " added successfully");
    }
    #endregion

    #region MAIN
    public void SelectOptionCaterogy(int index)
    {
        // Current options toggle
        currentOptionCaterogy = index;

        // Toggle only selected options
        for (int currentSelect = 0; currentSelect < optionSettings.ToArray().Length; currentSelect++)
        {
            // Change the color border
            optionSettings[currentSelect].optionButton.GetComponent<RawImage>().color = currentSelect == currentOptionCaterogy ?
                Color.green : Color.white;

            // Set visible on selected options
            foreach (GameObject optionTab in optionSettings[currentSelect].optionFieldTab)
                optionTab.SetActive(currentSelect == currentOptionCaterogy);
        }
    }

    public void TransitionMainExecutable(int transition_id)
    {
        // Transition to other scene when interact with object
        SceneManager.LoadScene(sceneTransitionList[transition_id]);
    }
    #endregion

    #region COMPONENT
    private void GetSliderSettings_UpdateData(Slider target, string dataKey, float defaultValue = 1)
    {
        // Update value of slider
        target.value = PlayerPrefs.GetFloat(dataKey, defaultValue);
    }

    private void GetSliderSettings_ModifyValue(Slider target, string dataKey, float value)
    {
        // Modify value of slider
        PlayerPrefs.SetFloat(dataKey, value);
        GetSliderSettings_UpdateData(target, dataKey, 0.5f);
    }

    private void GetCheckBoxesSettings_UpdateData(GameObject contentData, string dataKey)
    {
        // Update value of checkbox
        contentData.transform.GetChild(0).GetComponent<RawImage>().enabled = PlayerPrefs.GetInt(dataKey, 1) == 1 ? true : false;
    }

    private void GetCheckBoxesSettings_ModifyValue(GameObject contentData, string dataKey, int value)
    {
        // Modify value of checkbox
        PlayerPrefs.SetInt(dataKey, value);
        GetCheckBoxesSettings_UpdateData(contentData, dataKey);
    }

    private void GetDropdownSettings_UpdateData(Dropdown config_object, string dataKey)
    {
        // Update value of dropdown list
        config_object.value = PlayerPrefs.GetInt(dataKey, 1);
    }

    private void GetDropdownSettings_ModifyData(Dropdown config_object, string dataKey, int value)
    {
        // Modify value of dropdown list
        PlayerPrefs.SetInt(dataKey, value);
        GetDropdownSettings_UpdateData(config_object, dataKey);
    }
    #endregion

    #region MAIN (Audio Settings)
    public void AdjustHandlerOfAudio(Slider target, string key)
    {
        if (target)
        {
            GetSliderSettings_ModifyValue(target, key, target.value);
            Debug.Log("Audio: " + target.name + " | Volume: " + target.value);
        }
    }

    public void ChangeOfOption_AudioMasterSettings(GameObject target, string key, bool isChecked)
    {
        if (target)
        {
            GetCheckBoxesSettings_ModifyValue(target, key, isChecked ? 1 : 0);
            Debug.Log("Master Control: " + target.name + " | Switch: " + (isChecked ? "ON" : "OFF"));
        }
    }
    #endregion

    #region MAIN (Graphics Settings)
    public void ChangeOfOptions_GameplayContent(GameObject target, string key, bool isChecked)
    {
        if (target)
        {
            GetCheckBoxesSettings_ModifyValue(target, key, isChecked ? 1 : 0);
            Debug.Log("Control: " + target.name + " | Switch: " + (isChecked ? "ON" : "OFF"));
        }
    }
    #endregion

    #region MAIN (Account Settings)
    public void ChangeOfOptions_AccountConfiguration(Dropdown target, string key)
    {
        if (target)
        {
            GetDropdownSettings_ModifyData(target, key, target.value);
            Debug.Log("Control: " + target.name + " | Switch: " + target.options[target.value].text);
        }
    }

    public void ResetLoginIdentify()
    {
        PlayerPrefs.DeleteKey("UserID_Login");
    }

    public void SubTransitionExecutable(int transition_id)
    {
        // Transition to other scene when interact with object
        SceneManager.LoadScene(sceneSubTransitionList[transition_id]);
    }
    #endregion

    #region SETUP (Data Settings)
    private void ExtractPackageBundle(GameObject id, string dataKey)
    {
        DataArrayBundle allData = new DataArrayBundle().GetBundle(PlayerPrefs.GetString("ExchangeCode_TempData", string.Empty));
        foreach (DataPackStructure key in allData.data)
        {
            if (key.unqiueCode == dataKey)
            {
                switch (key.dataSeriesKey)
                {
                    case "Gift":
                        PlayerPrefs.SetString("AlertPop_Message", "Added " + key.packageName + " into your storage bag");
                        LocalSave_DataManagement saveItem = new LocalSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
                            "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

                        saveItem.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);
                        saveItem.SaveVirtualItemFromPlayer(key.packageName, 1);

                        saveItem.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileExchangeHistory);
                        saveItem.SaveExchangeTranscationHistory(JsonUtility.ToJson(key));
                        break;

                    default:
                        PlayerPrefs.SetString("AlertPop_Message", key.packageName);
                        break;
                }
                break;
            }
        }
    }

    private bool VerifyCodeConfirmation(string dataKey)
    {        
        VersionControlArray versionControl = new VersionControlArray().GetAllData(
            PlayerPrefs.GetString("ExchangeCode_TempData_Version", string.Empty));

        DataArrayBundle allData = new DataArrayBundle().GetBundle
            (PlayerPrefs.GetString("ExchangeCode_TempData", string.Empty));

        int lengthOfVersion = 0, currentVersion = -1;
        bool isVerify = false;

        foreach (DataPackStructure key in allData.data)
        {
            if (key.unqiueCode == dataKey)
            {
                isVerify = true;
                for (int countOfVersion = 0; countOfVersion < versionControl.versions.Length; countOfVersion++)
                {
                    if (versionControl.versions[countOfVersion] == StartMenu_Script.thisMenu.get_version)
                        currentVersion = countOfVersion;

                    if (versionControl.versions[countOfVersion] == key.version)
                        lengthOfVersion = countOfVersion;
                }
            }
        }

        //Debug.Log("Current Version: " + currentVersion + " | Latest Version: " + lengthOfVersion);
        return isVerify && lengthOfVersion != -1 && currentVersion >= lengthOfVersion;
    }
    #endregion

    #region MAIN (Data Settings)
    public void ConfirmCode(GameObject componentId)
    {
        PlayerPrefs.DeleteKey("AlertPop_Message");

        // Code reedem status: Display
        componentId.transform.GetChild(3).gameObject.SetActive(componentId.transform.GetChild(2).GetComponent<InputField>().text != string.Empty 
            ? true : false);

        // Code reedem status: Message
        componentId.transform.GetChild(3).GetComponent<Text>().text =
            MeloMelo_GameSettings.GetCodeNotReedemable(componentId.transform.GetChild(2).GetComponent<InputField>().text) ?
            "Exchange Completed" : VerifyCodeConfirmation(componentId.transform.GetChild(2).GetComponent<InputField>().text)
                ? "Exchange Successful" : "Invalid Code";

        // Code reedem status: Color
        componentId.transform.GetChild(3).GetComponent<Text>().color =
            VerifyCodeConfirmation(componentId.transform.GetChild(2).GetComponent<InputField>().text)
                ? Color.green : Color.red;

        // Prcoess to exchange if possible
        if (!MeloMelo_GameSettings.GetCodeNotReedemable(componentId.transform.GetChild(2).GetComponent<InputField>().text) &&
            VerifyCodeConfirmation(componentId.transform.GetChild(2).GetComponent<InputField>().text))
        {
            ExtractPackageBundle(componentId, componentId.transform.GetChild(2).GetComponent<InputField>().text);
            componentId.transform.GetChild(2).GetComponent<InputField>().text = string.Empty;
        }
    }
    #endregion
}

public class CreditScripts
{
    
}

public class ControlScripts
{
    
}