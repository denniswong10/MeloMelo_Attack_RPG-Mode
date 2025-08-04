using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MeloMelo_VirtualItem;
using UnityEngine.UI;

public class StoragePage_Script : MonoBehaviour
{
    private GameObject[] BGM;

    public GameObject Selection;
    public GameObject LoadingBar;
    [SerializeField] private GameObject[] slots;

    [SerializeField] private Text PageIndicator;
    [SerializeField] private Button[] NagivatorSelector;
    [SerializeField] private GameObject ItemDesPanel;
    [SerializeField] private GameObject AlertPop;
    [SerializeField] private GameObject DiscardPit;

    [SerializeField] private GameObject[] CurrencyPanel;
    [SerializeField] private GameObject OpenChoiceTemplate;

    private Queue<string> batchList;
    private bool isBatchPromptRunning;

    void Start()
    {
        BGM_Loader();
        isBatchPromptRunning = false;
        batchList = new Queue<string>();

        Selection.GetComponent<Animator>().SetTrigger("Opening");
        Invoke("RefreshStorageLoader", 2);
        Invoke("GetNagivator", 2);
    }

    #region SETUP
    private void GetStorageOnCurrency()
    {
        PlayerPrefs.SetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_Active Items", MeloMelo_ItemUsage_Settings.GetActiveItems() != null
            ? MeloMelo_ItemUsage_Settings.GetActiveItems().Length : 0);
        for (int instance = 0; instance < CurrencyPanel.Length; instance++)
            CurrencyPanel[instance].GetComponent<CurrencyInTag_Scripts>().UpdateCurrencyValue();
    }

    private void BGM_Loader()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }

    private void ClearSlotData()
    {
        for (int slot_id = 0; slot_id < slots.Length; slot_id++)
        {
            slots[slot_id].transform.GetChild(0).GetComponent<RawImage>().enabled = false;
            slots[slot_id].transform.GetChild(1).GetComponent<Text>().text = string.Empty;
            PlayerPrefs.DeleteKey(slot_id + "_Slot_ItemName");
        }
    }

    private void RefreshStorageLoader()
    {
        int currentPage = PlayerPrefs.GetInt("StorageBag_PageIndex", 0);
        ClearSlotData();
        //PerformItemFileLoader();

        VirtualItemDatabase[] allItems = MeloMelo_ItemUsage_Settings.GetActiveItems();
        GetStorageOnCurrency();

        if (allItems != null)
        {
            for (int slot_id = 0; slot_id < slots.Length; slot_id++)
            {
                int currentSlot = slots.Length * currentPage + slot_id;

                if (allItems.Length != 0 && currentSlot < allItems.Length)
                {
                    int totalAmount = allItems[currentSlot].amount - MeloMelo_ItemUsage_Settings.GetItemUsed(allItems[currentSlot].itemName);

                    if (totalAmount > 0)
                    {
                        slots[slot_id].transform.GetChild(0).GetComponent<RawImage>().enabled = true;
                        slots[slot_id].transform.GetChild(0).GetComponent<RawImage>().texture = RetrieveItemData(allItems[currentSlot].itemName).Icon;
                        slots[slot_id].transform.GetChild(1).GetComponent<Text>().text = "x" + Mathf.Clamp(totalAmount, 0, 9999);
                        PlayerPrefs.SetString(slot_id + "_Slot_ItemName", allItems[currentSlot].itemName);
                    }
                    else
                        PlayerPrefs.DeleteKey(slot_id + "_Slot_ItemName");
                }
                else
                    break;
            }
        }
    }
    #endregion

    #region MAIN
    public void ToggleCurrencyInfo(GameObject currencyPanel)
    {
        currencyPanel.transform.GetChild(3).gameObject.SetActive(true);
    }

    public void CloseCurrencyInfo(GameObject currencyPanel)
    {
        currencyPanel.transform.GetChild(3).gameObject.SetActive(false);
    }

    public void NagivatePage(int index)
    {
        PlayerPrefs.SetInt("StorageBag_PageIndex", PlayerPrefs.GetInt("StorageBag_PageIndex", 0) + index);
        RefreshStorageLoader();
        GetNagivator();
    }

    public void ToogleSlotInfo(int slot_index)
    {
        if (PlayerPrefs.HasKey(slot_index + "_Slot_ItemName"))
        {
            ItemData currentData = RetrieveItemData(PlayerPrefs.GetString(slot_index + "_Slot_ItemName", string.Empty));
            string[] typeOfItems = { "Item", "Consumable", "Artifact", "Non-Item" };
            ItemDesPanel.transform.GetChild(0).GetComponent<Text>().text = currentData.itemName == string.Empty ? "???" :
                currentData.itemName;

            ItemDesPanel.transform.GetChild(1).GetComponent<Text>().text = currentData.description;

            ItemDesPanel.transform.GetChild(2).GetComponent<Text>().text =
                currentData.itemValue > 0 ? "Item Rank: " + currentData.itemValue : string.Empty;

            ItemDesPanel.transform.GetChild(3).GetComponent<Text>().text = "Item Type: " + typeOfItems[(int)currentData.thisItemType];
        }
        else
        {
            for (int index = 0; index < ItemDesPanel.transform.childCount; index++)
            {
                ItemDesPanel.transform.GetChild(index).GetComponent<Text>().text = index == 1 ? 
                    "Select an item to view details" : string.Empty;
            }
        }
    }

    public void UseItemSelection(int slot_index)
    {
        if (PlayerPrefs.HasKey(slot_index + "_Slot_ItemName"))
        {
            if (PlayerPrefs.GetInt("StorageBag_UseItem_Bound", -1) == slot_index)
            {
                PlayerPrefs.DeleteKey("StorageBag_UseItem_Bound");

                switch (RetrieveItemData(PlayerPrefs.GetString(slot_index + "_Slot_ItemName", string.Empty)).thisItemType)
                {
                    case ItemData.ItemType.Item:
                        ItemData itemfound = RetrieveItemData(PlayerPrefs.GetString(slot_index + "_Slot_ItemName", string.Empty));
                        PerformItemInCaterogy(itemfound.itemName);
                        break;

                    case ItemData.ItemType.Consumable:
                        StartCoroutine(PromptMessageBox("Item cannot be used this way"));
                        break;

                    case ItemData.ItemType.Artifact:
                        StartCoroutine(PromptMessageBox("This item cannot be used"));
                        break;

                    default:
                        StartCoroutine(PromptMessageBox("Invalid Item"));
                        break;
                }
            }
            else
                PlayerPrefs.SetInt("StorageBag_UseItem_Bound", slot_index);
        }
        else
            PlayerPrefs.DeleteKey("StorageBag_UseItem_Bound");
    }

    public void ExitStorageBag()
    {
        Selection.GetComponent<Animator>().SetTrigger("Closing");
        Invoke("LeaveStoragePage_2", 2);
    }
    #endregion

    #region COMPONENT
    private void PerformItemInCaterogy(string item_Reference)
    {
        // Use item and perform item
        foreach (UsageOfItemDetail item in Resources.LoadAll<UsageOfItemDetail>("Database_Item/Filtered_Items/Others"))
        {
            if (item_Reference == item.itemName)
            {
                switch (item.useTypeDetail)
                {
                    case UsageOfItemDetail.UseType.Bundle:
                        ConfirmOnUsingItem(item.itemName);
                        StartCoroutine(UsePackableItem(item));
                        break;

                    case UsageOfItemDetail.UseType.Instant:
                        ConfirmOnUsingItem(item.itemName);
                        StartCoroutine(UseEconomyClassic(item));
                        break;

                    case UsageOfItemDetail.UseType.RandomObtain:
                        ConfirmOnUsingItem(item.itemName);
                        StartCoroutine(UseMysteryPackItem(item));
                        break;

                    case UsageOfItemDetail.UseType.OpenChoice:
                        UseSelectionPackItem(item);
                        break;

                    case UsageOfItemDetail.UseType.OpenResultUsage:
                        break;
                }

                return;
            }           
        }
        
        StartCoroutine(PromptMessageBox("This item isn't available at the moment"));
    }

    private void GetNagivator()
    {
        NagivatorSelector[0].interactable = PlayerPrefs.GetInt("StorageBag_PageIndex", 0) > 0;
        NagivatorSelector[1].interactable = PlayerPrefs.GetInt("StorageBag_PageIndex", 0) < GetTotalPageAvailable();
        PageIndicator.text = PlayerPrefs.GetInt("StorageBag_PageIndex", 0) + "/" + GetTotalPageAvailable();
    }

    private int GetTotalPageAvailable()
    {
        int total = MeloMelo_ItemUsage_Settings.GetActiveItems() != null ? MeloMelo_ItemUsage_Settings.GetActiveItems().Length : 0;
        return total / slots.Length;
    }

    private ItemData RetrieveItemData(string itemName)
    {
        foreach (ItemData item in Resources.LoadAll<ItemData>("Database_Item")) if (item.itemName == itemName) return item;
        return Resources.Load<ItemData>("Database_Item/#0");
    }

    private IEnumerator PromptMessageBox(string message)
    {
        AlertPop.SetActive(true);
        AlertPop.transform.GetChild(0).GetComponent<Text>().text = message;
        yield return new WaitForSeconds(3);
        AlertPop.SetActive(isBatchPromptRunning); 
    }

    private IEnumerator BatchPromptBox()
    {
        isBatchPromptRunning = true;
        AlertPop.SetActive(true);

        while (batchList.Count > 0)
        {
            string message = batchList.Dequeue();
            AlertPop.transform.GetChild(0).GetComponent<Text>().text = message;
            yield return new WaitForSeconds(2);
        }

        AlertPop.SetActive(false);
        isBatchPromptRunning = false;
    }

    private void LeaveStoragePage_2() { SceneManager.LoadScene("Collections"); }
    #endregion

    #region MISC (Item Cateogry Sorter)
    private IEnumerator UsePackableItem(UsageOfItemDetail production)
    {
        string[] listOfItems = production.dataArray.Split("/");
        foreach (string itemThroughJson in listOfItems)
        {
            if (itemThroughJson != string.Empty)
            {
                VirtualItemDatabase fromJson = JsonUtility.FromJson<VirtualItemDatabase>(itemThroughJson);
                GiveawayItemToPlayer(fromJson.itemName, fromJson.amount);
                yield return new WaitForSeconds(3);
                RefreshStorageLoader();
            }
        }
    }

    private IEnumerator UseEconomyClassic(UsageOfItemDetail prodution)
    {
        string[] instant_value = prodution.dataArray.Split(",");
        GiveawayCurrencyToPlayer(instant_value[0], int.Parse(instant_value[1]));
        yield return new WaitForSeconds(0.5f);

        RefreshStorageLoader();
        GetStorageOnCurrency();
    }

    [System.Serializable]
    struct ItemRateContainer
    {
        public string itemName;
        public int amount;
        public float obtainableRate;
    }

    private IEnumerator UseMysteryPackItem(UsageOfItemDetail production)
    {
        const float fixedPercentageRate = 100;
        float generateNumber = Random.Range(0, fixedPercentageRate);

        List<ItemRateContainer> itemListing = new List<ItemRateContainer>();
        string[] itemInArray = production.dataArray.Split("/");

        foreach (string item in itemInArray)
        {
            if (item != string.Empty) 
                itemListing.Add(JsonUtility.FromJson<ItemRateContainer>(item));
        }

        foreach (ItemRateContainer itemSearch in itemListing)
        {
            if (fixedPercentageRate - itemSearch.obtainableRate >= generateNumber)
            {               
                GiveawayItemToPlayer(itemSearch.itemName, itemSearch.amount);
                Debug.Log("SuccessRate: " + generateNumber + " %");
                Debug.Log("Item Obtained: " + itemSearch.itemName + " | " + itemSearch.obtainableRate + " %");

                yield return new WaitForSeconds(0.5f);
                RefreshStorageLoader();
                break;
            }
        }
    }

    private void UseSelectionPackItem(UsageOfItemDetail production)
    {
        string[] allItemToChoose = production.dataArray.Split("/");
        List<VirtualItemDatabase> listOfItemForChoose = null;

        for (int id = 0; id < allItemToChoose.Length - 1; id++)
        {
            if (allItemToChoose[id] != string.Empty)
            {
                if (listOfItemForChoose == null) listOfItemForChoose = new List<VirtualItemDatabase>();
                VirtualItemDatabase getItem = JsonUtility.FromJson<VirtualItemDatabase>(allItemToChoose[id]);
                listOfItemForChoose.Add(getItem);
            }
        }

        if (!PlayerPrefs.HasKey("GetOpenChoicePanel"))
        {
            PlayerPrefs.SetInt("GetOpenChoicePanel", 1);
            GameObject panel = Instantiate(OpenChoiceTemplate, Selection.transform);

            panel.GetComponent<VirtualOpenChoice>().SetCurrentItemForSelection(production.itemName);
            panel.GetComponent<VirtualOpenChoice>().SetTitleDescription(production.dataArray.Split("^")[1]);
            panel.GetComponent<VirtualOpenChoice>().GetChoiceAvailableReady(listOfItemForChoose.ToArray());
        }
    }
    #endregion

    #region MISC (System File)
    private void PerformItemUpdateFile(string itemName, int updateAmount)
    {
        switch (LoginPage_Script.thisPage.portNumber)
        {
            case (int)MeloMelo_PlayerSettings.LoginType.GuestLogin:
                MeloMelo_Local.LocalSave_DataManagement itemLoader = new MeloMelo_Local.LocalSave_DataManagement(
                            LoginPage_Script.thisPage.GetUserPortOutput(), "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");
                itemLoader.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);
                itemLoader.SaveVirtualItemFromPlayer(itemName, updateAmount, MeloMelo_ExtensionContent_Settings.GetItemIsStackable(itemName));
                break;

            case (int)MeloMelo_PlayerSettings.LoginType.TempPass:
                break;

            default:
                break;
        }

        MeloMelo_ItemUsage_Settings.OverwriteActiveItem(itemName, updateAmount);
        PlayerPrefs.DeleteKey(itemName + "_VirtualItem_Unsaved_Used");
    }

    private void PerformItemFileLoader()
    {
        switch (LoginPage_Script.thisPage.portNumber)
        {
            case (int)MeloMelo_PlayerSettings.LoginType.GuestLogin:
                MeloMelo_Local.LocalLoad_DataManagement itemLoader = new MeloMelo_Local.LocalLoad_DataManagement(
                    LoginPage_Script.thisPage.GetUserPortOutput(), "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");
                itemLoader.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);
                //StartCoroutine(itemLoader.PostLoading_VirtualItemData());
                break;

            default:
                break;
        }
    }

    private void PerformForceUpdateFile()
    {
        MeloMelo_Local.LocalSave_DataManagement saveToDrive = new MeloMelo_Local.LocalSave_DataManagement(
            LoginPage_Script.thisPage.GetUserPortOutput(), "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");
        saveToDrive.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileProfileData);
        saveToDrive.SaveProfileState();
    }

    public void ConfirmOnUsingItem(string itemName)
    {
        MeloMelo_ItemUsage_Settings.SetItemUsed(itemName);
        PerformItemUpdateFile(itemName, -MeloMelo_ItemUsage_Settings.GetItemUsed(itemName));

        batchList.Enqueue("Successfully Used: " + itemName);
        if (!isBatchPromptRunning) StartCoroutine(BatchPromptBox());
        RefreshStorageLoader();
    }
    #endregion

    #region MISC (Player Economy)
    public void GiveawayItemToPlayer(string itemName, int amount)
    {
        ItemData itemInDatabase = RetrieveItemData(itemName);
        if (itemInDatabase != null)
        {
            PerformItemUpdateFile(itemName, amount);
            batchList.Enqueue(amount + "X Obtained Item: " + itemName);
            if (!isBatchPromptRunning) StartCoroutine(BatchPromptBox());
        }
    }

    public void GiveawayCurrencyToPlayer(string nameOfCurrency, int new_balance)
    {
        int currentBalance = PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_" + nameOfCurrency, 0);
        PlayerPrefs.SetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_" + nameOfCurrency,
            currentBalance + new_balance);

        if (new_balance > 0)
        {
            PerformForceUpdateFile();
            batchList.Enqueue("Get " + new_balance + " " + nameOfCurrency);
            if (!isBatchPromptRunning) StartCoroutine(BatchPromptBox());
        }
    }
    #endregion
}
