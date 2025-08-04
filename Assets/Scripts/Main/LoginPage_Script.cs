using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MeloMelo_Local;
using System.Threading.Tasks;
using System;

public class LoginPage_Script : MonoBehaviour
{
    public static LoginPage_Script thisPage;

    private string User = "GUEST";
    public string get_user { get { return User; } }

    public Text CurrentGameVersion;
    public GameObject Icon;
    private GameObject[] BGM;

    private Coroutine rebootApplication;
    [HideInInspector] public int portNumber = 0;

    // Program: Login Scene
    void Start()
    {
        thisPage = this;
        rebootApplication = null;

        // System Component: Intit
        BGM_Loader();

        // Interface: Intit
        CurrentGameVersion.text = "Installed Version: " + CheckingForVersionIndex();
    }

    void Update()
    {
        // Return To Start Menu: Function
        if (Input.GetKeyDown(KeyCode.Escape) && rebootApplication == null) { rebootApplication = StartCoroutine(GetRebootProcessing()); }
    }

    #region SETUP
    string CheckingForVersionIndex()
    {
        try { return StartMenu_Script.thisMenu.version; } catch { return "---"; }
    }

    IEnumerator GetRebootProcessing()
    {
        Destroy(BGM[0]);
        AsyncOperation process = SceneManager.LoadSceneAsync("LoadScene");
        yield return new WaitWhile(() => !process.isDone);
    }
    #endregion

    #region MAIN
    public void LinkSite() { Application.OpenURL(MeloMelo_PlayerSettings.GetWebServerUrl() + "/database/transcripts/site7"); }

    public void BackButton() { SceneManager.LoadScene("ServerGateway"); }
    #endregion

    #region COMPONENT
    // Component System: Start-up
    private void BGM_Loader()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }
    #endregion

    #region MISC
    public void UpdateUserProfileName(string name) { User = name; }

    public string GetUserPortOutput()
    {
        switch (portNumber)
        {
            case 0:
                return GuestLogin_Script.thisScript.get_entry;

            default:
                return User;
        }
    }
    #endregion
}
