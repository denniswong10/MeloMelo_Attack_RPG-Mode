using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePurchase_Transcation_Processing : MonoBehaviour
{
    [SerializeField] private GameObject Transcation_Completed;
    [SerializeField] private GameObject Transcation_Failed;

    [SerializeField] private Button CancelBtn;
    public Coroutine pendingTranscation { get; private set; }

    private bool isItemFound;

    #region MAIN
    public void CheckingInStockPurchasing(ItemData itemPending, int amountToObtain, int paidAmount, MeloMelo_Economy.CurrencyType currency)
    {
        isItemFound = false;
        CancelBtn.interactable = false;
        gameObject.SetActive(true);

        pendingTranscation = StartCoroutine(TranscationBegin_Roll(itemPending, amountToObtain, paidAmount, currency));
        StartCoroutine(TranscationError_TimeOut());
    }

    public void GetTranscationCancel()
    {
        gameObject.SetActive(false);

        if (pendingTranscation != null)
        {
            StopCoroutine(pendingTranscation);
            pendingTranscation = null;
        }
    }
    #endregion

    #region COMPONENT (Shown Result)
    private void TranscationCompleted()
    {
        gameObject.SetActive(false);
        Transcation_Completed.SetActive(true);
    }

    private void TranscationFailed()
    {
        gameObject.SetActive(false);
        Transcation_Failed.SetActive(true);
    }
    #endregion

    #region COMPONENT (Awaiting Result)
    private IEnumerator TranscationError_TimeOut()
    {
        yield return new WaitForSeconds(5);
        CancelBtn.interactable = true;
    }

    private IEnumerator TranscationBegin_Roll(ItemData itemReference, int obatinAmount, int currency, MeloMelo_Economy.CurrencyType typeOfCurrency)
    {
        StartCoroutine(CheckingForItemAvailable(itemReference));
        yield return new WaitUntil(() => isItemFound);

        switch (typeOfCurrency)
        {
            case MeloMelo_Economy.CurrencyType.HonorCoin:
                ExchangeCurrencyThroughItem(itemReference, obatinAmount, currency);
                break;

            case MeloMelo_Economy.CurrencyType.Credits:
                PurchaseItemThroughCurrency(itemReference, obatinAmount, currency);
                break;

            case MeloMelo_Economy.CurrencyType.MagicStone:
                break;
        }

        pendingTranscation = null;
    }
    #endregion

    #region MISC
    private IEnumerator CheckingForItemAvailable(ItemData itemToFind)
    {
        List<ItemData> temp_item_listing = new List<ItemData>();
        int itemId = 1;

        while (true)
        {
            ResourceRequest getItem = Resources.LoadAsync<ItemData>("Database_Item/#" + itemId);
            yield return new WaitUntil(() => getItem.isDone);

            if (getItem.asset == null)
                break;

            temp_item_listing.Add(getItem.asset as ItemData);
            itemId++;
        }

        foreach (ItemData getWantedItem in temp_item_listing)
        {
            if (itemToFind.itemName == getWantedItem.itemName)
            {
                isItemFound = true;
                break;
            }
        }
    }

    private void ExchangeCurrencyThroughItem(ItemData itemToBeExchange, int totalAmountForExchange, int currenyToBeDeduct)
    {
        // To Receive: Item had added into save file
        MeloMelo_Local.LocalSave_DataManagement itemReceiver = new MeloMelo_Local.LocalSave_DataManagement(
                            LoginPage_Script.thisPage.GetUserPortOutput(), "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");
        itemReceiver.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);

        // Item is to be exchanged
        itemReceiver.SaveVirtualItemFromPlayer(itemToBeExchange.itemName, totalAmountForExchange, itemToBeExchange.stackable);

        // Currency have be deducted
        itemReceiver.SaveVirtualItemFromPlayer("HONOR COIN", -currenyToBeDeduct, itemToBeExchange.stackable);

        // To Load: Item have been reimport into the game
        MeloMelo_ItemUsage_Settings.OverwriteActiveItem("HONOR COIN", -currenyToBeDeduct);
        MeloMelo_ItemUsage_Settings.OverwriteActiveItem(itemToBeExchange.itemName, totalAmountForExchange);
        
        PlayerPrefs.SetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_" + MeloMelo_Economy.currencyTagInArray[(int)MeloMelo_Economy.CurrencyType.HonorCoin],
            MeloMelo_ItemUsage_Settings.GetActiveItem("HONOR COIN").amount);

        // Continue user interface 
        TranscationCompleted();
    }

    private void PurchaseItemThroughCurrency(ItemData itemToBePurchased, int totalObtainAmount, int currenyToBeDeduct)
    {

    }
    #endregion
}
