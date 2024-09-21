using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerTemplate_Script : MonoBehaviour
{
    private string destination;
    private string serverURL;
    public string get_serverURL { get { return serverURL; } }
    private int port;

    public Texture[] serverType;

    #region MAIN
    public void SetDestinationPoint(string title)
    {
        destination = title;
    }

    public void SetServerIP(string url)
    {
        serverURL = url;
    }

    public void SetPort(int value)
    {
        port = value;
    }

    public void SetServerType(int index)
    {
        transform.GetChild(2).GetComponent<RawImage>().texture = serverType[index];
    }

    public void GoToLoginPoint()
    {
        if (transform.GetChild(1).GetComponent<Text>().text == "Status: OK")
        {
            PlayerPrefs.SetString("GameWeb_URL", serverURL);
            PlayerPrefs.SetString("ServerTag", transform.GetChild(0).GetComponent<Text>().text);

            GameObject.Find("GameInterface").GetComponent<ServerGateway_Script>().UpdateLoginType(port);
            SceneManager.LoadScene(destination);
        }
    }
    #endregion
}
