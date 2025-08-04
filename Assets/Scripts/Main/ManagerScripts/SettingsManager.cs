using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

struct BundleOptionSettings
{
    public GameObject optionButton;
    public GameObject[] optionFieldTab;
}

public class SettingsManager : MonoBehaviour
{
    private readonly string[] sceneTransitionList = { "Menu", "Controls", "Credits" };
    private readonly string[] sceneSubTransitionList = { "ServerGateway", "ProfileView" };

    private List<BundleOptionSettings> optionSettings;
    private int currentOptionCaterogy;

    void Start()
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
    }
    #endregion

    #region MAIN
    public void ToggleSettingsUsingDropdown(Dropdown option, string key, bool isInitial = false, int defaultValue = 0)
    {
        if (option != null)
        {
            int valueChosen = isInitial ? PlayerPrefs.GetInt(key, defaultValue) : option.value;
            GetDropdownSettings_ModifyData(option, key, valueChosen);
            Debug.Log("Dropdown as [ " + option.name + " ] chosen value to ( " + valueChosen + " )");
        }
    }

    public void ToggleSettingsUsingCheckbox(GameObject option, string key, bool isInitial = false, bool defaultValue = false)
    {
        if (option != null)
        {
            int valveSelector = isInitial ? PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) : 
                PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 0 ? 1 : 0;

            GetCheckBoxesSettings_ModifyValue(option, key, valveSelector);
            Debug.Log("Checkbox as [ " + option.name + " ] is currently ( " + (valveSelector == 1 ? "Active" : "Not Active") + " )");
        }
    }

    public void ToggleSettingsUsingSlider(Slider option, string key, bool isInitial = false, float defaultValue = 1)
    {
        if (option != null)
        {
            float toggledValue = isInitial ? PlayerPrefs.GetFloat(key, defaultValue) : option.value;
            GetSliderSettings_ModifyValue(option, key, toggledValue);
            Debug.Log("Slider as [ " + option.name + " ] is been toggled to ( " + toggledValue + " )");
        }
    }

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

    public void SubTransitionExecutable(int transition_id)
    {
        // Transition to other scene when interact with object
        SceneManager.LoadScene(sceneSubTransitionList[transition_id]);
    }

    public void ResetLoginIdentify()
    {
        PlayerPrefs.DeleteKey("UserID_Login");
    }
    #endregion

    #region COMPONENT 
    private void GetDropdownSettings_ModifyData(Dropdown config_object, string dataKey, int value)
    {
        // Modify value of dropdown list
        PlayerPrefs.SetInt(dataKey, value);
        GetDropdownSettings_UpdateData(config_object, dataKey);
    }

    private void GetDropdownSettings_UpdateData(Dropdown config_object, string dataKey)
    {
        // Update value of dropdown list
        config_object.value = PlayerPrefs.GetInt(dataKey);
    }

    private void GetCheckBoxesSettings_ModifyValue(GameObject contentData, string dataKey, int value)
    {
        // Modify value of checkbox
        PlayerPrefs.SetInt(dataKey, value);
        GetCheckBoxesSettings_UpdateData(contentData, dataKey);
    }

    private void GetCheckBoxesSettings_UpdateData(GameObject contentData, string dataKey)
    {
        // Update value of checkbox
        contentData.transform.GetChild(0).GetComponent<RawImage>().enabled = PlayerPrefs.GetInt(dataKey) == 1 ? true : false;
    }

    private void GetSliderSettings_ModifyValue(Slider target, string dataKey, float value)
    {
        // Modify value of slider
        PlayerPrefs.SetFloat(dataKey, value);
        GetSliderSettings_UpdateData(target, dataKey);
    }

    private void GetSliderSettings_UpdateData(Slider target, string dataKey)
    {
        // Update value of slider
        target.value = PlayerPrefs.GetFloat(dataKey);
    }
    #endregion

    #region MISC
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
                        MeloMelo_Local.LocalSave_DataManagement saveItem = new 
                            MeloMelo_Local.LocalSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
                            "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

                        saveItem.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);
                        saveItem.SaveVirtualItemFromPlayer(key.packageName, 1, MeloMelo_ExtensionContent_Settings.GetItemIsStackable(key.packageName));

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

        try
        {
            foreach (DataPackStructure key in allData.data)
            {
                if (key.unqiueCode == dataKey)
                {
                    isVerify = true;
                    for (int countOfVersion = 0; countOfVersion < versionControl.versions.Length; countOfVersion++)
                    {
                        if (versionControl.versions[countOfVersion] == StartMenu_Script.thisMenu.version)
                            currentVersion = countOfVersion;

                        if (versionControl.versions[countOfVersion] == key.version)
                            lengthOfVersion = countOfVersion;
                    }
                }
            }
        }
        catch { }

        //Debug.Log("Current Version: " + currentVersion + " | Latest Version: " + lengthOfVersion);
        return isVerify && lengthOfVersion != -1 && currentVersion >= lengthOfVersion;
    }

    public void ConfirmCode(GameObject componentId)
    {
        PlayerPrefs.DeleteKey("AlertPop_Message");

        // Code reedem status: Display
        componentId.transform.GetChild(3).gameObject.SetActive(componentId.transform.GetChild(2).GetComponent<InputField>().text != string.Empty
            ? true : false);

        // Code reedem status: Message
        componentId.transform.GetChild(3).GetComponent<Text>().text =
            MeloMelo_ExtensionContent_Settings.GetCodeNotReedemable(componentId.transform.GetChild(2).GetComponent<InputField>().text) ?
            "Exchange Completed" : VerifyCodeConfirmation(componentId.transform.GetChild(2).GetComponent<InputField>().text)
                ? "Exchange Successful" : "Invalid Code";

        // Code reedem status: Color
        componentId.transform.GetChild(3).GetComponent<Text>().color =
            VerifyCodeConfirmation(componentId.transform.GetChild(2).GetComponent<InputField>().text)
                ? Color.green : Color.red;

        // Prcoess to exchange if possible
        if (!MeloMelo_ExtensionContent_Settings.GetCodeNotReedemable(componentId.transform.GetChild(2).GetComponent<InputField>().text) &&
            VerifyCodeConfirmation(componentId.transform.GetChild(2).GetComponent<InputField>().text))
        {
            ExtractPackageBundle(componentId, componentId.transform.GetChild(2).GetComponent<InputField>().text);
            componentId.transform.GetChild(2).GetComponent<InputField>().text = string.Empty;
        }
    }
    #endregion
}
