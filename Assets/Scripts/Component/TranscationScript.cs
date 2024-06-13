using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranscationScript : MonoBehaviour
{
    public static TranscationScript transcation;
    public GameObject thisZone;
    private bool CancelEnable;
    private int GetTabSelection;
    private ItemData ItemOrder;
    private int Item_Quantity;
    private bool CancelPurchase;

    private string userInput;

    private void GetTranscationSelection(int index)
    {
        for (int i = 0; i < thisZone.transform.childCount; i++)
        {
            if (i == index) thisZone.transform.GetChild(i).gameObject.SetActive(true);
            else thisZone.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void CloseTranscationSelection()
    {
        thisZone.transform.GetChild(GetTabSelection).gameObject.SetActive(false);
    }

    private void CancelTranscation()
    {
        thisZone.transform.GetChild(GetTabSelection).GetChild(2).GetComponent<Button>().interactable = true;
    }

    public void CancelTranscation_Button()
    {
        thisZone.transform.GetChild(GetTabSelection).GetChild(2).GetComponent<Button>().interactable = false;
        CloseTranscationSelection();
        PlayerPrefs.DeleteKey("TranscationTab_ID");
    }

    public void GetTrascationToPurchase(int id)
    {
        PlayerPrefs.SetInt("TranscationTab_ID", id);
        StartCoroutine(CallTranscationZone());
    }

    public IEnumerator CallTranscationZone()
    {
        GetTranscationSelection(1);
        GetTabSelection = 1;
        if (!CancelEnable) { CancelEnable = true; Invoke("CancelTranscation", 5); }
        yield return new WaitForSeconds(3);
        //yield return new WaitUntil(() => PlayerPrefs.HasKey("TranscationTab_ID"));

        if (PlayerPrefs.HasKey("TranscationTab_ID")) { Encode_CallTranscationZone(); }
    }

    private void Encode_CallTranscationZone()
    {
        CancelInvoke("CancelTranscation");
        CancelEnable = false;
        CloseTranscationSelection();
        OpenPaymentForm();
    }

    private void OpenPaymentForm()
    {
        try { userInput = LoginPage_Script.thisPage.get_user; } catch { userInput = "GUEST"; }

        GetTranscationSelection(0);
        GetTabSelection = 0;
        Item_Quantity = 1;

        ItemOrder = Resources.Load<ItemData>("Database_Item/#" + PlayerPrefs.GetInt("TranscationTab_ID"));
        thisZone.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "Item Order: " + ItemOrder.ItemName;
        thisZone.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Order Type: --";
        thisZone.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = "Quantity: " + Item_Quantity;

        thisZone.transform.GetChild(0).GetChild(5).GetComponent<Text>().text = "Cost Required: " + ItemOrder.CreditSet;
        thisZone.transform.GetChild(0).GetChild(6).GetComponent<Text>().text = "Credit Taken: " + PlayerPrefs.GetInt(userInput + "_Credits", 0) + ((ItemOrder.CreditSet * Item_Quantity) != 0 ? " (-" + (ItemOrder.CreditSet * Item_Quantity) + ")" : "");
        thisZone.transform.GetChild(0).GetChild(7).GetComponent<Text>().text = "Credit Balance: " + (PlayerPrefs.GetInt(userInput + "_Credits", 0) - (ItemOrder.CreditSet - Item_Quantity));

        CheckingForPurchase();
    }

    private void CheckingForPurchase()
    {
        if (PlayerPrefs.GetInt(userInput + "_Credits", 0) - (ItemOrder.CreditSet * Item_Quantity) >= 0)
        {
            thisZone.transform.GetChild(0).GetChild(8).GetComponent<Button>().interactable = true;
        }
        else
        {
            thisZone.transform.GetChild(0).GetChild(8).GetComponent<Button>().interactable = false;
        }
    }

    public void MakePruchaseProcess()
    {
        CloseTranscationSelection();
        GetTranscationSelection(2);
        GetTabSelection = 2;

        StartCoroutine(ProcessingToPurchase());
    }

    private IEnumerator ProcessingToPurchase()
    {
        // Server Coding...
        if (!CancelPurchase) { CancelPurchase = true; Invoke("AutoCancelPurchase", 30); }
        yield return new WaitUntil(() => PlayerPrefs.HasKey("TranscationComplete"));

        CloseTranscationSelection();
        if (PlayerPrefs.GetInt("TranscationComplete") == 0) { GetTranscationSelection(4); GetTabSelection = 4; }
        else 
        { 
            GetTranscationSelection(3); GetTabSelection = 3;
            try { Marathon_Selection.thisSelect.Get_InventoryData.SaveItemToLocal(PlayerPrefs.GetInt("TranscationTab_ID", 0)); } catch { }
        }
    }

    private void AutoCancelPurchase() { PlayerPrefs.SetInt("TranscationComplete", 0); }

    public void DoneTranscation()
    {
        CancelPurchase = false;
        CloseTranscationSelection();
        PlayerPrefs.DeleteKey("TranscationTab_ID");
    }

    // Start is called before the first frame update
    void Start()
    {
        transcation = this;
        CancelEnable = false;
        CancelPurchase = false;
        GetTabSelection = 0;
    }
}
