using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot_Nagivator_Script : MonoBehaviour
{
    private VirtualStorageBag connectStorage;
    private VirtualItemDatabase itemReference;
    private bool isActivationConfirm;
    private bool isToggleTransitting;

    void Start()
    {
        isActivationConfirm = false;
        isToggleTransitting = false; 
    }

    #region MAIN
    public void UpdateItemSlot(VirtualStorageBag reference, VirtualItemDatabase item)
    {
        connectStorage = reference;
        itemReference = item;
    }

    public void ToggleItemSlot()
    {
        if (connectStorage != null)
        {
            int displayId = isActivationConfirm ? 1 : 0;

            if (!isToggleTransitting)
            {
                GetStatusOfToggle(0);
                isToggleTransitting = true;
            }

            connectStorage.ToggleItemInfo(itemReference.itemName);
        }
    }

    public void ToggleItemOut()
    {
        if (connectStorage != null)
        {
            GetStatusOfToggle(3);
            isToggleTransitting = false;

            connectStorage.ToggleOutItemInfo();
            if (isActivationConfirm) isActivationConfirm = false;
        }
    }

    public void ActivateUsedOfSlot()
    {
        if (connectStorage != null)
        {
            if (isActivationConfirm)
            {
                int tempCount = MeloMelo_ItemUsage_Settings.GetItemUsed(itemReference.itemName);
                if (itemReference.amount - tempCount > 0) GetStatusOfToggle(itemReference.amount - tempCount > 0 ? 1 : 2);

                connectStorage.UseStorageItem(itemReference.itemName);
                if (MeloMelo_ItemUsage_Settings.GetItemUsed(itemReference.itemName) == tempCount) GetStatusOfToggle(2);
            }
            else isActivationConfirm = true;
        }
    }
    #endregion

    private void GetStatusOfToggle(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == index) transform.GetChild(i).gameObject.SetActive(true);
            else transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
