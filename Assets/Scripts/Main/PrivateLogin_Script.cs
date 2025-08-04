using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_Network;

public class PrivateLogin_Script : MonoBehaviour
{
    [SerializeField] private Text playerID;
    [SerializeField] private InputField secureKey;
    [SerializeField] private Button loginBtn;

    [SerializeField] private GameObject entryCreator;
    [SerializeField] private GameObject[] alertNoticeS;

    // Start is called before the first frame update
    void Start()
    {
        UpdateContentLoginButton(PlayerPrefs.GetString("TempPass_PlayerId", string.Empty) != string.Empty);

        // Get server data services (Player ID)
        GetServer_CheckID();
    }

    #region SETUP
    private void LoadTempDetail()
    {
        // Display playerId if existing account is been created
        string playerId = PlayerPrefs.GetString("TempPass_PlayerId", string.Empty);
        playerID.text = "Player ID: " + (playerId == string.Empty ? "--" : playerId);
    }

    private void UpdateContentLoginButton(bool isSigned)
    {
        // Update button content on create if player account not found. Otherwise button to login
        loginBtn.gameObject.transform.GetChild(0).GetComponent<Text>().text = isSigned ? "LOGIN" : "CREATE";
    }
    #endregion

    #region MAIN (Network Handler)
    public void Login()
    {
        loginBtn.interactable = false;
        LoginPage_Script.thisPage.portNumber = 1;
    }

    public void LoginConfirm()
    {
        
    }
    #endregion

    #region MISC
    private void GetServer_CheckID()
    {
        CloudServices_ControlPanel services = new CloudServices_ControlPanel(MeloMelo_PlayerSettings.GetWebServerUrl());
        StartCoroutine(services.CheckNetwork_IDInspection(PlayerPrefs.GetString("TempPass_PlayerId", string.Empty)));
    }
    #endregion
}
