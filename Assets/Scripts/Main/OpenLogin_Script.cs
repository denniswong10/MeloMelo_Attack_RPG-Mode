using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_Network;

public class OpenLogin_Script : MonoBehaviour
{
    [SerializeField] private InputField userName;
    [SerializeField] private InputField passWord;
    [SerializeField] private Button loginBtn;
    [SerializeField] private Text serverID_Tag;
    
    private Authenticate_DataManagement services;
    private CloudData_Login_Script cloudFunction;

    void Start()
    {
        // Get login credentials information and logic checker
        services = new Authenticate_DataManagement(MeloMelo_PlayerSettings.GetWebServerUrl());
        cloudFunction = GetComponent<CloudData_Login_Script>();

        // Display server name through text
        GetServerID(PlayerPrefs.GetString("ServerTag", string.Empty));
    }

    void Update()
    {
        // Hotkey for quick login and input toggling
        if (cloudFunction != null && !services.get_success)
        {
            if (Input.GetKeyDown(KeyCode.Return)) Login();
            if (Input.GetKeyDown(KeyCode.Tab)) ToggleInput();
        }
    }

    #region SETUP
    private void GetServerID(string id_address)
    {
        string vaildServerId = id_address != string.Empty ? id_address : "???";
        serverID_Tag.text = "Server ID: " + vaildServerId;
    }
    #endregion

    #region MAIN
    public void Login()
    {
        if (cloudFunction != null)
        {
            LoginPage_Script.thisPage.portNumber = 1;
            LockedLoginFiller();

            StartCoroutine(services.AuthenticateUser(userName.text, passWord.text));
            StartCoroutine(VerifyUser());
        }
    }

    public void Register()
    {
        Application.OpenURL(MeloMelo_PlayerSettings.GetWebServerUrl() + "/database/transcripts/site7/database/sitemap/MeloMelo Site (GameHub)/signup.php");
    }
    #endregion

    #region COMPONENT
    private IEnumerator VerifyUser()
    {
        float loginTimeOut = 5f;
        float timer = 0f;

        cloudFunction.UpdateMessageIcon("Logging in...");

        yield return new WaitUntil(() => {
            timer += Time.deltaTime;
            return services.get_success || timer >= loginTimeOut;
        });

        if (!services.get_success) LoginTimeOut();
        else LoginSuccessful();
    }
    #endregion

    #region MISC
    public void CheckInformationFilter()
    {
        bool inputAreFilled = !IsInputFieldEmpty(userName) && !IsInputFieldEmpty(passWord);
        loginBtn.interactable = inputAreFilled;
    }

    private bool IsInputFieldEmpty(InputField target)
    {
        return target == null || string.IsNullOrWhiteSpace(target.text);
    }

    private void LoginTimeOut()
    {
        if (cloudFunction != null) cloudFunction.UpdateMessageIcon("Login Failed!");
    }


    private void LoginSuccessful()
    {
        cloudFunction.UpdateMessageIcon("Login Successful!");
        LoginPage_Script.thisPage.UpdateUserProfileName(services.GetUserPlayerName());
        cloudFunction.LoadPlayer();
    }

    private void LockedLoginFiller()
    {
        userName.interactable = false;
        passWord.interactable = false;
        loginBtn.interactable = false;
    }

    private void ToggleInput()
    {
        if (userName.isFocused) passWord.Select();
        else userName.Select();
    }
    #endregion
}
