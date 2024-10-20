using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UsageOfItemDetail", menuName = "Item_Function")]
public class UsageOfItemDetail : ScriptableObject
{
    public string itemName;
    public enum UseType { Bundle, Consumable };
    public UseType useTypeDetail;
    [TextArea] public string dataArray;
}
