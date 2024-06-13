using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MeloMelo_NetworkChecker : MonoBehaviour
{
    public Texture[] server_log = new Texture[4];
    private enum ServerStatus { ERROR, OK, OFF };

    private bool serverOpen = false;
    public bool get_server { get { return serverOpen; } }

    // Start is called before the first frame update
    void Start()
    {
        CheckForNetwork();
    }

    #region SETUP
    private int GetServerStatus()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                ChangeOfNetworkDisplay((int)ServerStatus.OFF + 1);
                return 0;

            default:
                if (PlayerPrefs.GetString("GameWeb_URL", string.Empty) != string.Empty) StartCoroutine(GetServerNetworkLive());
                else ChangeOfNetworkDisplay((int)ServerStatus.OFF + 1);
                return 1;
        }
    }

    private void ChangeOfNetworkDisplay(int index)
    {
        GetComponent<RawImage>().texture = server_log[index];
    }
    #endregion

    #region MAIN
    private void CheckForNetwork()
    {
        serverOpen = GetServerStatus() != 0;
    }
    #endregion

    #region COMPONENT (NETWORK)
    private IEnumerator GetServerNetworkLive()
    {
        WWWForm net = new WWWForm();
        net.AddField("Title", Application.productName);

        UnityWebRequest getNet = UnityWebRequest.Post(StartMenu_Script.thisMenu.get_serverURL + "/database/transcripts/site5/UnityLogin_InternetChecker.php", net);
        yield return getNet.SendWebRequest();
        
        switch (getNet.downloadHandler.text)
        {
            case "off":
                ChangeOfNetworkDisplay((int)ServerStatus.OFF + 1);
                break;

            case "OK!":
                ChangeOfNetworkDisplay((int)ServerStatus.OK + 1);
                break;

            default:
                ChangeOfNetworkDisplay((int)ServerStatus.ERROR + 1);
                break;
        }

        Debug.Log("Server Output: " + getNet.downloadHandler.text);
        getNet.Dispose();
    }
    #endregion
}
