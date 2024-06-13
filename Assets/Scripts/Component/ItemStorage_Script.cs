using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemStorage_Script : MonoBehaviour
{
    public GameObject[] Slots;
    private ItemData ItemDatabase;

    private void InsertItemToStorageView()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (PlayerPrefs.HasKey("Slot" + (i + 1) + "_ItemStore"))
            {
                ItemDatabase = Resources.Load<ItemData>("Database_Item/#" + PlayerPrefs.GetInt("Slot" + (i + 1) + "_ItemStore"));
            }
            else { ItemDatabase = Resources.Load<ItemData>("Database_Item/#0"); }

            if (ItemDatabase.ItemName == "None")
            {
                Slots[i].transform.GetChild(0).GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                Slots[i].transform.GetChild(0).GetComponent<RawImage>().texture = null;
                Slots[i].transform.GetChild(1).GetComponent<Text>().text = string.Empty;
            }
            else
            {
                Slots[i].transform.GetChild(0).GetComponent<RawImage>().color = new Color(0, 0, 0, 1);
                Slots[i].transform.GetChild(0).GetComponent<RawImage>().texture = ItemDatabase.Icon;
                Slots[i].transform.GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetInt("Slot" + (i + 1) + "_ItemStore_Quantity", 0).ToString();
            }
        }
    }

    public void SelectSlot(int slotId)
    {
        PlayerPrefs.SetInt("SlotSelected_Item", slotId);
    }

    // Update is called once per frame
    void Start()
    {
        InsertItemToStorageView();
    }
}
