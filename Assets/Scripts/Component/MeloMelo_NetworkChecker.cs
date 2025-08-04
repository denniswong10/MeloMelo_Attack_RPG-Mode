using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MeloMelo_NetworkChecker : MonoBehaviour
{
    public Texture[] server_log = new Texture[4];
    private enum ServerStatus { ERROR, OK, OFF };

    private bool serverOpen;
    [SerializeField] private bool refreshConnection;

    // Start is called before the first frame update
    void Start()
    {
        serverOpen = false;
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
                if (MeloMelo_PlayerSettings.GetWebServerUrl() != string.Empty) GetServerCheckingData();
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
        if (refreshConnection) serverOpen = GetServerStatus() != 0;
        else GetServerNetworkLive(PlayerPrefs.GetString("storeCache_Connection", "off"));
    }
    #endregion

    #region COMPONENT (NETWORK)
    private void GetServerCheckingData()
    {
        MeloMelo_Network.CloudServices_ControlPanel services = new MeloMelo_Network.CloudServices_ControlPanel(MeloMelo_PlayerSettings.GetWebServerUrl());
        StartCoroutine(services.CheckNetwork_GlobalStatus(gameObject, Application.productName));
    }

    public void GetServerNetworkLive(string received_status)
    {
        PlayerPrefs.SetString("storeCache_Connection", received_status);

        switch (received_status)
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
    }
    #endregion
}
