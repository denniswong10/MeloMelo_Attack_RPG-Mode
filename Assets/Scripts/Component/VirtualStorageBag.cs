using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualStorageBag : MonoBehaviour
{
    [SerializeField] private GameObject storagePanel;
    private bool limitedUseTime;
    private GameObject AlertPop;

    void Start()
    {
        ReAlignOfPanelPosition();
    }

    #region SETUP
    private void ReAlignOfPanelPosition()
    {
        if (PlayerPrefs.HasKey(name + "_PositionStand"))
            GetComponent<RectTransform>().position = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString(name + "_PositionStand", string.Empty));
        else
            GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void SetDefaultDescription(string description)
    {
        transform.GetChild(2).GetComponent<Text>().text = description;
    }

    public void SetAlertPopReference(GameObject panel)
    {
        AlertPop = panel;
    }

    public void SetLimitedUsageTime(bool active)
    {
        limitedUseTime = active;
    }

    public void SetItemForDisplay(VirtualItemDatabase[] itemList)
    {
        for (int itemId = 0; itemId < itemList.Length; itemId++)
        {
            for (int slot = 0; slot < storagePanel.transform.childCount; slot++)
            {
                if (itemList[itemId].amount > 0 && !PlayerPrefs.HasKey(slot + "_" + name + "_Slot_ItemBound"))
                {
                    storagePanel.transform.GetChild(slot).gameObject.SetActive(true);
                    storagePanel.transform.GetChild(slot).GetComponent<RawImage>().texture = FindItemToPlot(itemList[itemId].itemName).Icon;
                    PlayerPrefs.SetString(slot + "_" + name + "_Slot_ItemBound", itemList[itemId].itemName);
                    break;
                }
            }
        }
    }
    #endregion

    #region MAIN
    public void ToggleItemInfo(int index)
    {
        if (AlertPop)
        {
            string itemFound = PlayerPrefs.GetString(index + "_" + name + "_Slot_ItemBound", string.Empty);
            AlertPop.SetActive(true);

            AlertPop.transform.GetChild(0).GetComponent<Text>().text = itemFound + 
                " ( x " + Mathf.Clamp(GetTotalCountItem(itemFound), 0, 9999) + " )";
        }
    }

    public void ToggleOutItemInfo()
    {
        if (AlertPop)
        {
            AlertPop.SetActive(false);
        }
    }

    public void UseStorageItem(int index)
    {
        if (PlayerPrefs.GetString(name + "_ItemUsingBound", string.Empty) ==
            PlayerPrefs.GetString(index + "_" + name + "_Slot_ItemBound", string.Empty))
            ActivationOfItemUsage(PlayerPrefs.GetString(index + "_" + name + "_Slot_ItemBound", string.Empty));
        else 
            PlayerPrefs.SetString(name + "_ItemUsingBound", 
                PlayerPrefs.GetString(index + "_" + name + "_Slot_ItemBound", string.Empty));
    }

    public void DragPanel()
    {
        transform.position = new Vector3(Mathf.Clamp(Input.mousePosition.x, transform.localScale.x * 0.5f,
            Screen.width - (transform.localScale.x * 0.5f)),
                Mathf.Clamp(Input.mousePosition.y, transform.localScale.y * 0.5f,
                    Screen.height - (transform.localScale.y * 0.5f)), 0);

        string saveCurrentPosition = JsonUtility.ToJson(transform.position);
        PlayerPrefs.SetString(name + "_PositionStand", saveCurrentPosition);
    }

    public void ClosePanel()
    {
        for (int slot = 0; slot < storagePanel.transform.childCount; slot++)
            PlayerPrefs.DeleteKey(slot + "_" + name + "_Slot_ItemBound");

        Destroy(gameObject);
    }
    #endregion

    #region COMPONENT
    private ItemData FindItemToPlot(string itemName)
    {
        foreach (ItemData item in Resources.LoadAll<ItemData>("Database_Item")) if (item.itemName == itemName) return item;
        return Resources.Load<ItemData>("Database_Item/#0");
    }

    private int GetTotalCountItem(string itemName)
    {
        // Item counted in storage bag and current used items
        int itemAmount = MeloMelo_GameSettings.GetAllItemFromLocal(itemName).amount -
            MeloMelo_ItemUsage_Settings.GetItemUsed(itemName);

        return itemAmount;
    }

    private void ActivationOfItemUsage(string itemToUsed)
    {
        VirtualItemDatabase itemFound = MeloMelo_GameSettings.GetAllItemFromLocal(itemToUsed);
        if (itemFound.itemName == itemToUsed && GetTotalCountItem(itemToUsed) > 0)
        {
            PlayerPrefs.DeleteKey(name + "_ItemUsingBound");

            // Use of item and activation
            foreach (UsageOfItemDetail item in Resources.LoadAll<UsageOfItemDetail>
                ("Database_Item/Filtered_Items/" + itemToUsed.Split(" ")[1] + "_" + itemToUsed.Split(" ")[2]))
            {
                if (item.itemName == itemFound.itemName)
                {
                    if (PlayerPrefs.HasKey("Character_VirtualItem_UsageOfItem"))
                    {
                        // Stackable: Comsumable for one character
                        string onlyCharacter = PlayerPrefs.GetString("Character_VirtualItem_UsageOfItem", "None");
                        int currentAmount = PlayerPrefs.GetInt(onlyCharacter + item.dataArray.Split(",")[0], 0);

                        PlayerPrefs.SetInt(onlyCharacter + item.dataArray.Split(",")[0], 
                            currentAmount + int.Parse(item.dataArray.Split(",")[1]));
                    }
                    else
                    {
                        // Single Usage: Consumable for all characters 
                        for (int slot_id = 0; slot_id < 3; slot_id++)
                        {
                            string currentCharacter = PlayerPrefs.GetString("Slot" + (slot_id + 1) + "_charName", "None");
                            PlayerPrefs.SetInt(currentCharacter + item.dataArray.Split(",")[0], 
                                int.Parse(item.dataArray.Split(",")[1]));
                        }
                    }

                    MeloMelo_ItemUsage_Settings.SetItemUsed(itemFound.itemName);
                    if (limitedUseTime) ClosePanel();
                }
            }
        }
    }
    #endregion
}
