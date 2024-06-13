using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerTemplate_Script : MonoBehaviour
{
    private string destination;
    private int port;

    #region MAIN
    public void SetDestinationPoint(string title)
    {
        destination = title;
    }

    public void SetPort(int value)
    {
        port = value;
    }

    public void GoToLoginPoint()
    {
        if (transform.GetChild(1).GetComponent<Text>().text == "Status: OK")
        {
            GameObject.Find("GameInterface").GetComponent<ServerGateway_Script>().UpdateLoginType(port);
            SceneManager.LoadScene(destination);
        }
    }
    #endregion
}
