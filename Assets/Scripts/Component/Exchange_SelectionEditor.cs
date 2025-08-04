using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exchange_SelectionEditor : MonoBehaviour
{
    public ItemData itemReference { get; private set; }
    public int costInData { get; private set; }

    [SerializeField] private RawImage Icon;
    [SerializeField] private Text Title;
    [SerializeField] private Text Description;
    [SerializeField] private Text Cost;

    [Header("Manual Importing")]
    [SerializeField] private bool importedItem;
    [SerializeField] private int id;
    [SerializeField] private int costForItem;

    // Start is called before the first frame update
    void Start()
    {
        if (importedItem)
        {
            itemReference = Resources.Load<ItemData>("Database_Item/#" + id);
            UpdateSlotInfo();
        }
    }

    #region MAIN
    public void GetSlotUpdateItem(ItemData itemToBeDisplay)
    {
        itemReference = itemToBeDisplay;
        UpdateSlotInfo();
    }

    public void GetSlotUpdatePricing(int cost)
    {
        costInData = cost;

        if (itemReference != null)
            Cost.text = cost.ToString();
        else
            Cost.text = "0";
    }

    public void GetTranscationCalled()
    {
        if (itemReference != null)
            MarathonSelection_Script.thisMarathon.MakeExchangeThroughTranscation(itemReference, costInData);
        else
            Debug.Log("Transcation failed to load...");
    }
    #endregion

    #region COMPONENT
    private void UpdateSlotInfo()
    {
        Icon.texture = itemReference.Icon;
        Title.text = itemReference.itemName;
        Description.text = itemReference.description;

        if (importedItem)
        {
            costInData = costForItem;
            Cost.text = costForItem.ToString();
        }
    }
    #endregion
}
