using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualOpenChoice : MonoBehaviour
{
    [SerializeField] private RawImage ItemPlate;
    [SerializeField] private Text Title_Front;
    [SerializeField] private GameObject ItemPlateContent;
    private string currentItemSelection;
    private bool isItemClaimed;

    // Start is called before the first frame update
    void Start()
    {
        isItemClaimed = false;
    }

    #region SETUP
    public void GetChoiceAvailableReady(VirtualItemDatabase[] itemList)
    {
        if (itemList != null)
        {
            for (int plate = 0; plate < itemList.Length; plate++)
            {
                if (itemList[plate].amount > 0 && !PlayerPrefs.HasKey(plate + "_" + name + "_Slot_ItemBound"))
                {
                    RawImage itemInstance = Instantiate(ItemPlate, ItemPlateContent.transform);
                    itemInstance.name = plate.ToString();
                    itemInstance.gameObject.SetActive(true);

                    string amountToObtain = itemList[plate].amount > 1 ? " ( x" + itemList[plate].amount + " )" : string.Empty;
                    itemInstance.transform.GetChild(0).GetComponent<Text>().text = itemList[plate].itemName + amountToObtain;

                    PlayerPrefs.SetInt(plate + "_" + name + "_Slot_ItemBound_Amount", itemList[plate].amount);
                    PlayerPrefs.SetString(plate + "_" + name + "_Slot_ItemBound", itemList[plate].itemName);
                }
            }
        }
    }

    public void SetTitleDescription(string title)
    {
        Title_Front.text = title;
    }

    public void SetCurrentItemForSelection(string itemName)
    {
        currentItemSelection = itemName;
    }
    #endregion

    #region MAIN
    public void ProcessToClaimReward(GameObject id)
    {
        if (!isItemClaimed)
        {
            isItemClaimed = true;
            StartCoroutine(ClaimReward(PlayerPrefs.GetString(id.name + "_" + name + "_Slot_ItemBound", string.Empty),
                PlayerPrefs.GetInt(id.name + "_" + name + "_Slot_ItemBound_Amount", 1)));
        }
    }

    public void ClosePanel()
    {
        for (int instance = 1; instance < ItemPlateContent.transform.childCount; instance++)
            PlayerPrefs.DeleteKey(instance - 1 + "_" + name + "_Slot_ItemBound");

        PlayerPrefs.DeleteKey("GetOpenChoicePanel");
        Destroy(gameObject);
    }
    #endregion

    #region COMPONENT 
    private IEnumerator ClaimReward(string itemName, int amount)
    {
        StoragePage_Script inDirect_Script = GameObject.Find("GameInterface").GetComponent<StoragePage_Script>();
        inDirect_Script.ConfirmOnUsingItem(currentItemSelection);
        yield return new WaitForSeconds(3);

        inDirect_Script.GiveawayItemToPlayer(itemName, amount);
        ClosePanel();
    }
    #endregion
}
