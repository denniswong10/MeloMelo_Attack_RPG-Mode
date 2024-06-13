using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class MeloMelo_NoteSpeed_Settings
{
    public float bpm;
    public float baseSpeed;
}

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

    void Start()
    {
        thisServer = this;

        statusReady = false;
        BGM_Loader();
        LoadServerData();

        StartCoroutine(CheckingForNetwork());
        GetServerGatewayDescription();
        CurrentV.text = InstalledVerisionDisplay();
    }

    // Transition --> From StartMenu_Transition
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Invoke("Reboot_Application", 0.5f); }
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

    public void UpdateLoginType(int index)
    {
        serverGateSwitch = index;
    }

    private string CheckServerStatus(string status, int index)
    {
        if (serverTemplate[index].offlineMode)
            return serverStatus[0];
        else
            return status;
    }

    private void LoadServerData()
    {
        serverTemplate = Resources.LoadAll<ServerGateWayDetail>("Database_Server");
    }

    private void GetServerGatewayDescription()
    {
        for (int gateway = 0; gateway < serverTemplate.Length; gateway++)
        {
            RawImage template = Instantiate(SeverListTemplate);

            template.transform.GetChild(0).GetComponent<Text>().text = serverTemplate[gateway].serverTitle;
            template.transform.SetParent(GatewayList.transform);

            template.GetComponent<ServerTemplate_Script>().SetDestinationPoint(serverTemplate[gateway].destinationTitle);
            template.GetComponent<ServerTemplate_Script>().SetPort(gateway);
            StartCoroutine(GetServerConntectedToCloud(template, gateway));
        }
    }
    #endregion

    #region MAIN
    #endregion

    #region COMPONENT
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

    void HideIcon()
    {
        icon.SetActive(false);
    }
    #endregion

    #region NETWORK

    private IEnumerator GetServerConntectedToCloud(RawImage reference, int index)
    {
        yield return new WaitForSeconds(1);

        WWWForm get = new WWWForm();
        get.AddField("Title", Application.productName);

        UnityWebRequest server = UnityWebRequest.Post(StartMenu_Script.thisMenu.get_serverURL + "/database/transcripts/site5/UnityLogin_InternetChecker.php", get);
        yield return server.SendWebRequest();

        switch (server.downloadHandler.text)
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
        server.Dispose();
    }
    #endregion

    #region MISC
    private void Reboot_Application()
    {
        SceneManager.LoadScene("LoadScene");
        if (GameObject.Find("BGM").activeInHierarchy) Destroy(GameObject.Find("BGM"));
    }
    #endregion
}
