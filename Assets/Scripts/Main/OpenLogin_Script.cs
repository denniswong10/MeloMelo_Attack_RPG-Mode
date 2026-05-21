using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_Network;

public class OpenLogin_Script : MonoBehaviour
{
    // Server ID - Display UI
    [SerializeField] private Text serverID_placeholder;

    // Login Intel - Input Response
    [SerializeField] private InputField userName;
    [SerializeField] private InputField passWord;
    [SerializeField] private Button loginBtn;
    
    // Services - Connection between cloud data
    private Authenticate_DataManagement services;
    private CloudData_Login_Script cloudFunction;

    void Start()
    {
        // Get login credentials information and logic checker
        services = new Authenticate_DataManagement(MeloMelo_PlayerSettings.GetWebServerUrl());
        cloudFunction = GetComponent<CloudData_Login_Script>();

        // Display server name through text
        Output_ServerID_Info();
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
    private void Output_ServerID_Info()
    {
        string serverName = PlayerPrefs.GetString("ServerTag", string.Empty);
        string getNameEmpty =  !string.IsNullOrEmpty(serverName) ? serverName : "???";
        serverID_placeholder.text = "Server ID: " + getNameEmpty;
    }

    private void Reset_Login_Input()
    {

    }
    #endregion

    #region MAIN
    public void Login()
    {
        if (cloudFunction != null)
        {
            LoginPage_Script.thisPage.portNumber = 1;
            LoginInputControl(false);

            StartCoroutine(services.AuthenticateUser(userName.text, passWord.text));
            StartCoroutine(VerifyUser(5));
        }
    }

    public void Register()
    {
        const string API_Register_Site = "/database/transcripts/site7/database/sitemap/MeloMelo Site (GameHub)/signup.php";
        Application.OpenURL(MeloMelo_PlayerSettings.GetWebServerUrl() + API_Register_Site);
    }
    #endregion

    #region COMPONENT
    private IEnumerator VerifyUser(float timeOutCount)
    {
        float timer = 0f;
        cloudFunction.UpdateMessageIcon("Logging in...");

        while (!services.get_success && timer < timeOutCount)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (!services.get_success) LoginTimeOut();
        else LoginSuccessful();
    }

    private void LoginTimeOut()
    {
        if (cloudFunction != null)
        {
            cloudFunction.UpdateMessageIcon("Login Failed!");
            LoginInputControl(true);
        }
    }

    private void LoginSuccessful()
    {
        cloudFunction.UpdateMessageIcon("Login Successful!");
        LoginPage_Script.thisPage.UpdateUserProfileName(services.GetUserPlayerName());
        cloudFunction.LoadPlayer();
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

    private void LoginInputControl(bool visible)
    {
        userName.interactable = visible;
        passWord.interactable = visible;
        loginBtn.interactable = visible;
    }

    private void ToggleInput()
    {
        if (userName.isFocused) passWord.Select();
        else userName.Select();
    }
    #endregion
}
