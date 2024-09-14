using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Authentication;

public class Auto_Authenticate_Config : MonoBehaviour
{
    private bool isDone = false;
    public bool IsDone { get { return isDone; } }

    void Start()
    {
        if (ServerGateway_Script.thisServer.get_loginType == (int)MeloMelo_GameSettings.LoginType.GuestLogin)
        {
            if (AuthenticationService.Instance.IsSignedIn) AuthenticationService.Instance.SignOut();
            AuthenticateServerLogin();
        }
    }

    #region MAIN
    public string GetAccountID()
    {
        if (AuthenticationService.Instance.PlayerId != string.Empty)
            return AuthenticationService.Instance.PlayerId;
        else
            return "---";
    }
    #endregion

    #region SETUP
    async void AuthenticateServerLogin()
    {
        isDone = false;
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync
                    (
                         PlayerPrefs.GetString("AccountSync_PlayerID"),
                         PlayerPrefs.GetString("AccountSync_UniqueID")
                    );

                Debug.Log("Sign in account of : " + GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByPlayerId());
            }
            catch
            {
                try
                {
                    await AuthenticationService.Instance.AddUsernamePasswordAsync
                    (
                        PlayerPrefs.GetString("AccountSync_PlayerID"),
                         PlayerPrefs.GetString("AccountSync_UniqueID")
                    );

                    Debug.Log("Created account of : " + GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByPlayerId());
                }
                catch { 
                    Debug.Log("Invalid account of : " + GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByPlayerId());
                }
            }
        }
        else
            Debug.Log("Cloud Services [LOCAL]: Disable");

        isDone = true;
    }

    public void LogOff_AccountSync()
    {
        if (ServerGateway_Script.thisServer.get_loginType == (int)MeloMelo_GameSettings.LoginType.GuestLogin && AuthenticationService.Instance.IsSignedIn)
           AuthenticationService.Instance.SignOut();
    }
    #endregion
}
