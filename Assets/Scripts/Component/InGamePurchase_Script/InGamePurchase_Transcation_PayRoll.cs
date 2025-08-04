using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class PayRollCalculator
{
    public int costPerUnit { get; private set; }
    public int quantity { get; private set; }
    public int totalBalance { get; private set; }

    public int minQuantity { get; private set; }
    public int maxQuantity { get; private set; }

    public PayRollCalculator(int startingCost, int balance)
    {
        minQuantity = 1;
        maxQuantity = 99;

        costPerUnit = startingCost;
        quantity = minQuantity;
        totalBalance = balance;
    }

    #region MAIN 
    public void ModifyAmount(int amount) => quantity += amount; 
    public void MakeChangeForBalance(int cost) => totalBalance -= cost;

    public void SetQuantityBoundary(int min, int max)
    {
        minQuantity = min;
        maxQuantity = max;
    }
    #endregion

    #region MISC
    public int GetTotalCost() { return quantity * costPerUnit; }
    public string BeforeDeduction() { return totalBalance + " (" + (-GetTotalCost()) + ")"; }
    public string AfterDeduction() { return (totalBalance - GetTotalCost()).ToString(); }
    #endregion
}

public class InGamePurchase_Transcation_PayRoll : MonoBehaviour
{
    [Header("Core: InGamePurhcase Script")]
    [SerializeField] private InGamePurchase_Transcation_Processing inGamePurchase_Core;

    private MeloMelo_Economy.CurrencyType currencyType;
    private PayRollCalculator roll = null;

    [Header("Main Component: Purchasing Details")]
    [SerializeField] private Text itemOrder_Txt;
    [SerializeField] private Text itemType_Txt;
    [SerializeField] private Text Quantity_Txt;
    [SerializeField] private Text CostRequired_Txt;
    [SerializeField] private Text CostTaken_Txt;
    [SerializeField] private Text RemainingBalance_Txt;

    [Header("Main Component: Button Interaction")]
    [SerializeField] private Button LowerQuantity;
    [SerializeField] private Button RaiseQuantity;
    [SerializeField] private Button Purchase_Btn;

    // Reference: Item
    private ItemData itemChosen;

    #region MAIN
    public void GetTranscation(ItemData item, int costPerUnit, MeloMelo_Economy.CurrencyType typeOfCurrency)
    {
        if (inGamePurchase_Core.pendingTranscation == null)
        {
            gameObject.SetActive(true);
            currencyType = typeOfCurrency;
            itemChosen = item;

            int balance = PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_" + MeloMelo_Economy.currencyTagInArray[(int)currencyType], 0);
            roll = new PayRollCalculator(costPerUnit, balance);
            UpdateTranscationDetails();
        }
    }

    public void AmountPurchasingChange(bool isLower)
    {
        if (roll != null)
        {
            if (isLower && roll.quantity > 0) roll.ModifyAmount(-1);
            else roll.ModifyAmount(1);

            RefreshCostOfTranscation();
            RefreshQuantityBoundary();
        }
    }

    public void ProcessTranscation()
    {
        gameObject.SetActive(false);
        inGamePurchase_Core.CheckingInStockPurchasing(itemChosen, roll.quantity, roll.GetTotalCost(), currencyType);
    }

    public void CancelTranscation()
    {
        gameObject.SetActive(false);
    }

    public void DoneTranscation(GameObject panel)
    {
        panel.SetActive(false);
    }
    #endregion

    #region COMPONENT
    private void UpdateTranscationDetails()
    {
        itemOrder_Txt.text = "Item Order: " + itemChosen.itemName;
        itemType_Txt.text = "Order Type: " + (itemChosen.thisItemType == ItemData.ItemType.Item ? "Item" : itemChosen.thisItemType == ItemData.ItemType.Consumable ? "Consumable" : "Artifcat");

        RefreshCostOfTranscation();
        RefreshQuantityBoundary();
    }

    private void RefreshCostOfTranscation()
    {
        if (roll != null)
        {
            Quantity_Txt.text = "Quantity: " + roll.quantity;
            CostRequired_Txt.text = LabelCurrencyType("Cost Required") + roll.GetTotalCost();
            CostTaken_Txt.text = "Before Cost Deduction: " + roll.BeforeDeduction();
            RemainingBalance_Txt.text = LabelCurrencyType("Remaining Balance") + roll.AfterDeduction();
        }
    }

    private void RefreshQuantityBoundary()
    {
        if (roll != null)
        {
            LowerQuantity.interactable = roll.quantity > roll.minQuantity;
            RaiseQuantity.interactable = roll.quantity < roll.maxQuantity;
            Purchase_Btn.interactable = roll.totalBalance >= roll.GetTotalCost();
        }
    }

    private string LabelCurrencyType(string title)
    {
        switch (currencyType)
        {
            case MeloMelo_Economy.CurrencyType.Credits:
                return title + " [ Credits ]: ";

            case MeloMelo_Economy.CurrencyType.MagicStone:
                return title + " [ MagicStone ]: ";

            case MeloMelo_Economy.CurrencyType.HonorCoin:
                return title + " [ Honor Coin ]: ";
        }

        return title;
    }
    #endregion
}
