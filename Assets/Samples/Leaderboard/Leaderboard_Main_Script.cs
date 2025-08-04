using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;

public class Leaderboard_Main_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (MeloMelo_PlayerSettings.GetLocalUserAccount() && !AuthenticationService.Instance.IsSignedIn)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
