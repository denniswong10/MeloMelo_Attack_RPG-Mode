using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_Local;

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

public class Options_Menu : MonoBehaviour
{
    private GameObject[] BGM;
    private SettingsManager settings;

    [SerializeField] private GameObject AlertPop;
    [SerializeField] private GameObject ChangePanel;
    [SerializeField] private Text MessageChangePanel;
    [SerializeField] private GameObject Icon;

    void Start()
    {
        LoadBGM();
        settings = GetComponent<SettingsManager>();
    }

    #region SETUP
    private void LoadBGM()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
        PlayerPrefs.SetInt("ReviewOption", 1);
    }

    public void GetModifyOfBGMAudioData(string bgm_key)
    {
        AudioSource audio = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
        if (audio) audio.volume = PlayerPrefs.GetFloat(bgm_key);
    }

    private void UpdatePlayerChangesIndicator(bool isSaved)
    {
        MessageChangePanel.text = isSaved ? "Pending Changes" : "All Changes Saved";
    }
    #endregion

    #region MISC (API For Data):
    public IEnumerator AwaitForCodeTransfering(GameObject id)
    {
        id.transform.GetChild(1).GetComponent<Button>().interactable = false;
        yield return new WaitUntil(() => PlayerPrefs.HasKey("ExchangeLoader_Ready"));
        settings.ConfirmCode(id);


        if (!AlertPop.activeInHierarchy && PlayerPrefs.HasKey("AlertPop_Message")) StartCoroutine(DisplayAlertPop());
        else AlertPop.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("AlertPop_Message", string.Empty);
    }

    public IEnumerator DisplayAlertPop()
    {
        AlertPop.SetActive(true);
        AlertPop.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("AlertPop_Message", string.Empty);
        yield return new WaitForSeconds(5);
        AlertPop.SetActive(false);
    }
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