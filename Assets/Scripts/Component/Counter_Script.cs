using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class ServerTime
{
    private static long serverUnix;
    private static float syncRealtime;

    public static void Sync(long unixSeconds)
    {
        serverUnix = unixSeconds;
        syncRealtime = UnityEngine.Time.realtimeSinceStartup;
    }

    public static long CurrentUnix
    {
        get
        {
            float elapsed = UnityEngine.Time.realtimeSinceStartup - syncRealtime;
            return serverUnix + (long)elapsed;
        }
    }
}

public class Counter_Script : MonoBehaviour
{
    void Start()
    {
        //if (gameObject.activeInHierarchy == false) GetComponent<Counter_Script>().enabled = false;

        // Get current UTC time
        //DateTime utcTime = DateTime.UtcNow;

        //// Print full UTC time
        //Debug.Log("UTC Time: " + utcTime);

        //// Print individual values
        //Debug.Log("Hour: " + utcTime.Hour);
        //Debug.Log("Minute: " + utcTime.Minute);
        //Debug.Log("Second: " + utcTime.Second);

        //GetComponent<Text>().text = "Currently Time:" + utcTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.thisManager.getInGameObjectWindow.notationBundle. != null)
        // GetComponent<Text>().text = "Max Pooling (NOTE): \n" + BeatConductor.thisBeat.get_noteUsage_listing.get_maxPoolingCount;

        //DateTime dt = DateTimeOffset.FromUnixTimeSeconds(ServerTime.CurrentUnix).UtcDateTime;
        //GetComponent<Text>().text = "Time: " + dt;
    }
}
