using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter_Script : MonoBehaviour
{
    void Start()
    {
        if (gameObject.activeInHierarchy == false) GetComponent<Counter_Script>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (BeatConductor.thisBeat.get_noteUsage_listing != null)
        //    GetComponent<Text>().text = "Max Pooling (NOTE): \n" + BeatConductor.thisBeat.get_noteUsage_listing.get_maxPoolingCount;
    }
}
