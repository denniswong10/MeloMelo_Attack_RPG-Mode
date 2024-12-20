using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginAccount_Script : MonoBehaviour
{
    [SerializeField] private InputField userName;
    [SerializeField] private InputField passWord;
    [SerializeField] private Button LoginBtn;
    [SerializeField] private Text ServerID_Tag;

    // Start is called before the first frame update
    void Start()
    {
        GetServerID_Tag();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region SETUP
    private void GetServerID_Tag()
    {
        ServerID_Tag.text = "Server ID: " + (PlayerPrefs.GetString("ServerTag", string.Empty) != string.Empty ? PlayerPrefs.GetString("ServerTag") : "???");
    }
    #endregion

    #region MAIN
    public void LoginAccount()
    {

    }

    public void RegisterAccount()
    {
        Application.OpenURL(PlayerPrefs.GetString("GameWeb_URL", string.Empty) + "/form_content.php");
    }

    public void UpdateInformationFiller()
    {
        LoginBtn.interactable = CheckingInputFilled();
    }
    #endregion

    #region COMPONENT
    private bool CheckingInputFilled()
    {
        return userName.text != string.Empty && passWord.text != string.Empty;
    }
    #endregion
}
