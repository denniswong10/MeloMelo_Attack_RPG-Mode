using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_RPGEditor;

[CreateAssetMenu(fileName = "stats",menuName = "CharacterStats",order = 1)]
public class ClassBase : ScriptableObject
{
    public string characterName;
    public Sprite icon;
    public enum ElementStats { Light, Dark, Earth };
    public ElementStats elementType;
    public enum ClassTypeChar { Warrior, Mage, Marksman, Support };
    public ClassTypeChar fixClassType;

    #region MAIN
    #endregion

    #region CHECKER_ZONE
    public string GetClassType()
    {
        switch (fixClassType)
        {
            case ClassTypeChar.Warrior:
                return "Warrior";

            case ClassTypeChar.Marksman:
                return "Marksman";

            case ClassTypeChar.Mage:
                return "Mage";

            default:
                return "Jobless";
        }
    }
    #endregion
}
