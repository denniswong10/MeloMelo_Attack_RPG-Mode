using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ServerGateway_Script : MonoBehaviour
{
    public static ServerGateway_Script thisServer;

    private GameObject[] BGM;
    private string[] serverStatus = { "OK", "Error", "OFF" };
    private bool statusReady;

    public GameObject icon;

    private string[] network_promptMessage =
    {
        "[Game Network]\nChecking Database...",
        "[Game Network]\nCompleted..."
    };

    [SerializeField] private Text CurrentV;
    [SerializeField] private GameObject GatewayList;
    [SerializeField] private RawImage SeverListTemplate;

    private ServerGateWayDetail[] serverTemplate;
    private int serverGateSwitch = 0;
    public int get_loginType { get { return serverGateSwitch; } }

    [SerializeField] private Text ServerCounter;
    private int totalserverCount;

    void Start()
    {
        thisServer = this;
        totalserverCount = 0;

        statusReady = false;
        BGM_Loader();
        LoadServerData();

        StartCoroutine(CheckingForNetwork());
        GetServerGatewayDescription();
        GetServerExtensionSocket();

        CurrentV.text = InstalledVerisionDisplay();
        ServerCounter.text = "Total Server: " + totalserverCount;
    }

    // Transition --> From StartMenu_Transition
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            GameObject.Find("ReturnBtn").GetComponent<Button>().interactable = false;
            Invoke("Reboot_Application", 0.5f); 
        }
    }

    #region SETUP
    private void BGM_Loader()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }

    private string InstalledVerisionDisplay()
    {
        return "Installed Version: " + StartMenu_Script.thisMenu.get_version;
    }

    private string CheckServerStatus(string status, int index)
    {
        if (index < serverTemplate.Length && serverTemplate[index].offlineMode)
            return serverStatus[0];
        else
            return status;
    }

    private void LoadServerData()
    {
        serverTemplate = Resources.LoadAll<ServerGateWayDetail>("Database_Server");
    }
    #endregion

    #region COMPONENT (Server Creation)
    private void GetServerGatewayDescription()
    {
        for (int gateway = 0; gateway < serverTemplate.Length; gateway++)
        {
            RawImage template = Instantiate(SeverListTemplate);
            template.transform.GetChild(0).GetComponent<Text>().text = serverTemplate[gateway].serverTitle;
            template.transform.SetParent(GatewayList.transform);
            ServerCreatorHub(template, gateway, 1);
        }

        // Count server instance
        totalserverCount += serverTemplate.Length;
    }

    private void GetServerExtensionSocket()
    {
        string fileDirectory = (Application.isEditor ? "Assets/" : "MeloMelo_Data/") + "StreamingAssets/SERVER.ini";

        if (System.IO.File.Exists(fileDirectory))
        {
            System.IO.StreamReader socket = new System.IO.StreamReader(fileDirectory);
            string[] socket_dat = socket.ReadToEnd().Split("*");

            for (int data = 0; data < socket_dat.Length / 3; data++)
            {
                RawImage template = Instantiate(SeverListTemplate);
                template.transform.GetChild(0).GetComponent<Text>().text = socket_dat[data * 3];
                template.transform.SetParent(GatewayList.transform);
                ServerCreatorHub(template, data + 1, int.Parse(socket_dat[data * 3 + 2]), socket_dat[data * 3 + 1]);
            }

            // Count server instance
            totalserverCount += socket_dat.Length / 3;
        }
    }

    private void ServerCreatorHub(RawImage template, int port, int connectionType, string url = "")
    {
        // Update server settings
        template.GetComponent<ServerTemplate_Script>().SetDestinationPoint(port == 0 ? "LoginPage3" : port != 0 && connectionType == 1 ? "LoginPage2" : "LoginPage1");
        template.GetComponent<ServerTemplate_Script>().SetServerType(port == 0 ? 0 : port != 0 && connectionType == 1 ? 1 : 2);
        template.GetComponent<ServerTemplate_Script>().SetServerIP(url);
        template.GetComponent<ServerTemplate_Script>().SetPort(port);

        if (url != string.Empty) GetServerConntectedToCloud(template, port, connectionType);
        else GetServerConntectedToCloud(template, port, string.Empty);
    }
    #endregion

    #region COMPONENT (Data Transition) 
    private IEnumerator CheckingForNetwork()
    {
        int length = network_promptMessage.Length;

        for (int sendMessage = 0; sendMessage < length; sendMessage++)
        {
            if (sendMessage + 1 < length)
            {
                if (!icon.activeInHierarchy) icon.SetActive(true);
                icon.transform.GetChild(1).GetComponent<Text>().text = network_promptMessage[sendMessage];
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitUntil(() => statusReady);
                icon.transform.GetChild(1).GetComponent<Text>().text = network_promptMessage[sendMessage];
            }
        }

        Invoke("HideIcon", 1);
    }

    private void HideIcon()
    {
        icon.SetActive(false);
    }
    #endregion

    #region NETWORK
    private void GetServerConntectedToCloud(RawImage reference, int index, int connectionType)
    {
        MeloMelo_Network.CloudServices_ControlPanel services = 
            new MeloMelo_Network.CloudServices_ControlPanel(reference.GetComponent<ServerTemplate_Script>().get_serverURL);

        StartCoroutine(services.CheckNetwork_ServerStatus(Application.productName, reference, index));
    }

    public void GetServerConntectedToCloud(RawImage reference, int index, string received_status)
    {
        switch (received_status)
        {
            case "OK!":
                reference.transform.GetChild(1).GetComponent<Text>().text = "Status: " + CheckServerStatus(serverStatus[0], index);
                break;

            case "error!":
                reference.transform.GetChild(1).GetComponent<Text>().text = "Status: " + CheckServerStatus(serverStatus[1], index);
                break;

            default:
                reference.transform.GetChild(1).GetComponent<Text>().text = "Status: " + CheckServerStatus(serverStatus[2], index);
                break;
        }

        statusReady = true;
    }
    #endregion

    #region MISC
    public void Reboot_Application()
    {
        SceneManager.LoadScene("LoadScene");
        if (GameObject.Find("BGM").activeInHierarchy) Destroy(GameObject.Find("BGM"));
    }

    public void UpdateLoginType(int index)
    {
        serverGateSwitch = index;
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
    #endregion
}
