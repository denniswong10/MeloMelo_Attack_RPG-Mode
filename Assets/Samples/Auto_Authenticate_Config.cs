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
            AuthenticateServerLogin();
    }

    #region MAIN
    public async Task<string> GetAccountID()
    {
        if (!isDone) await Setup();

        if (AuthenticationService.Instance.PlayerId != string.Empty)
            return AuthenticationService.Instance.PlayerId;
        else
            return "---";
    }
    #endregion

    #region SETUP
    async Task Setup()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync
                    (
                        GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByPlayerId(),
                        GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByUniqueId()
                    );
            }
            catch { }
        }

        isDone = true;
    }

    async void AuthenticateServerLogin()
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            try
            {
                await AuthenticationService.Instance.AddUsernamePasswordAsync
                    (
                        GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByPlayerId(),
                        GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByUniqueId()
                    );

                Debug.Log("Created account of : " + GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByPlayerId());
            }
            catch
            {
                Debug.Log("Existing account of : " + GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByPlayerId());
            }

            AuthenticationService.Instance.SignOut();
            await Setup();
        }
        else
            Debug.Log("Cloud Services [LOCAL]: Disable");
    }
    #endregion
}
