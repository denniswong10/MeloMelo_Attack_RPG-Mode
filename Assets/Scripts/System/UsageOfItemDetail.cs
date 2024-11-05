using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UsageOfItemDetail", menuName = "Item_Function")]
public class UsageOfItemDetail : ScriptableObject
{
    [Header("Name of the item based on ItemData")]
    public string itemName;

    public enum UseType { Bundle, Instant, RandomObtain, OpenChoice, OpenResultUsage };
    [Header("Use of Item Detail must be use in the correct format based on dataArray as stated")]
    public UseType useTypeDetail;

    [Header("Bundle - Use to pack all sort of item together" + 
        "[Format: VirtualItemDatabase using json end with /]" +

        "Instant - Use to store data currency" + 
        "[Format: NameOfDataUse<string>,AmountToGiveaway<int>]" +

        "RandomObtain - Same as bundle but only receive item randomly after used" +
        "[Format VirtualItemDatabase using json end with /]" +

        "OpenChoice - List of items to choose from. Chosen item will be added to your storage bag" +
        "[Format VirtualItemDatabase using json end with /]")]
    [TextArea] public string dataArray;
}
