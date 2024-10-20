using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarathonPassProgram : MonoBehaviour
{
    public ItemData itemId;
    public GameObject Ref;

    private void Setup()
    {
        Ref.transform.GetChild(1).GetComponent<RawImage>().texture = itemId.Icon;
        Ref.transform.GetChild(2).GetComponent<Text>().text = itemId.itemName;
        Ref.transform.GetChild(3).GetComponent<Text>().text = itemId.CreditSet + " Credit";
    }

    void Start()
    {
        Setup();
    }

    public void SelectPassOption()
    {

    }
}
