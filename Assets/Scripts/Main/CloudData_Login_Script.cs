using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_Network;

public class CloudData_Login_Script : MonoBehaviour
{
    [SerializeField] private GameObject Icon;
    private bool isServerOk;

    private delegate void AcquireEntryTempPass();
    private AcquireEntryTempPass acquireEntryPass;

    // Start is called before the first frame update
    void Start()
    {
        isServerOk = false;
    }

    #region MAIN
    public void LoadPlayer()
    {
        if (!isServerOk)
        {
            isServerOk = true;
            string url = MeloMelo_PlayerSettings.GetWebServerUrl();

            if (LoginPage_Script.thisPage.GetUserPortOutput() != string.Empty)
            {
                Debug.Log("[CloudServices] Login as: " + LoginPage_Script.thisPage.GetUserPortOutput());
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetString("TempPass_PlayerId", LoginPage_Script.thisPage.GetUserPortOutput());
                MeloMelo_PlayerSettings.UpdateWebServerUrl(url);
                MeloMelo_ExtensionContent_Settings.UpdateCharacterProfile();

                StartCoroutine(LoadCloudData());
                acquireEntryPass += GetMenuPlayThrough;
            }
            else
                Invoke("AwaitForReload", 2);
        }
    }
    #endregion

    #region COMPONENT 
    private IEnumerator LoadCloudData()
    {
        CloudLoad_DataManagement cloudData = new CloudLoad_DataManagement(
            LoginPage_Script.thisPage.GetUserPortOutput(), MeloMelo_PlayerSettings.GetWebServerUrl());

        for (int save = 0; save < 3; save++)
            StartCoroutine(cloudData.LoadProgressTrack(save + 1));

        StartCoroutine(cloudData.LoadSettingCofiguration());
        StartCoroutine(cloudData.LoadProgressProfile());
        StartCoroutine(cloudData.LoadPlayerSettings());
        StartCoroutine(cloudData.LoadSelectionLastVisited());
        StartCoroutine(cloudData.LoadBattleFormationData());
        StartCoroutine(cloudData.LoadCharacterStatusData());
        StartCoroutine(cloudData.LoadTrackDistributionChart());
        StartCoroutine(cloudData.LoadItemFromServer());
        StartCoroutine(cloudData.LoadMarathonContentListing());

        yield return new WaitUntil(() => cloudData.cloudLogging.ToArray().Length == cloudData.get_counter);
        acquireEntryPass();
    }
    #endregion

    #region MISC
    public void UpdateMessageIcon(string message)
    {
        Icon.SetActive(true);
        Icon.transform.GetComponentInChildren<Text>().text = "[Game Network]\n" + message;
    }

    private void GetMenuPlayThrough()
    {
        isServerOk = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    private void AwaitForReload()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoginPage2");
    }
    #endregion
}
