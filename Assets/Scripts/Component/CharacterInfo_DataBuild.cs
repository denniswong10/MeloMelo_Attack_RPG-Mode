using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo_DataBuild : MonoBehaviour
{
    [SerializeField] private Text AttackDamage;
    [SerializeField] private Text AttackDefense;
    [SerializeField] private Text MagicDamage;
    [SerializeField] private Text MagicDefense;
    [SerializeField] private Text Health;

    private ElemetStartingStats currentStats;
    private ClassBase characterReference = null;
    private MeloMelo_RPGEditor.StatsManage_Database characterStatsReference = null;

    void Update()
    {
        GetStatsUpdated();
        characterReference.UpdateCurrentStats(false);
    }

    #region SETUP
    private void GetStatsUpdated()
    {
        if (characterReference != null && characterStatsReference != null)
        {
            currentStats =
                MeloMelo_ExtensionContent_Settings.GetStatsWithElementBonus(characterReference.elementType == ClassBase.ElementStats.Light ? "Light" :
                characterReference.elementType == ClassBase.ElementStats.Dark ? "Dark" :
                characterReference.elementType == ClassBase.ElementStats.Earth ? "Earth" : "None");

            Health.text = Mathf.Clamp(GetCharacterHealth(currentStats.multipler), 0, GetCharacterHealth(currentStats.multipler)).ToString();
            AttackDamage.text = Mathf.Clamp(GetCharacterPhysical(currentStats.strength, currentStats.multipler), 0, GetCharacterPhysical(currentStats.strength, currentStats.multipler)).ToString();
            AttackDefense.text = Mathf.Clamp(GetCharacterPhysicalDef(currentStats.multipler), 0, GetCharacterPhysicalDef(currentStats.multipler)).ToString();
            MagicDefense.text = Mathf.Clamp(GetCharacterMagicDef(currentStats.multipler), 0, GetCharacterMagicDef(currentStats.multipler)).ToString();
            MagicDamage.text = Mathf.Clamp(GetCharacterMagic(currentStats.multipler), 0, GetCharacterMagic(currentStats.multipler)).ToString();
        }
    }
    #endregion

    #region COMPONENT 
    private int GetCharacterHealth(float multipler = 1)
    {
        int fixedValue = characterStatsReference.GetCharacterStatus(characterReference.level).GetVitality;
        int originalValue = MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(characterReference.name);
        int originalBaseHealth = characterStatsReference.GetCharacterStatus(characterReference.level).GetHealth;

        return (int)((fixedValue + originalValue) * (10 * multipler) + originalBaseHealth + MeloMelo_ExtraStats_Settings.GetExtraBaseHealth(characterReference.name));
    }

    private int GetCharacterPhysical(float defaultValue, float multipler = 1)
    {
        int fixedValue = characterStatsReference.GetCharacterStatus(characterReference.level).GetStrength;
        int originalValue = MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(characterReference.name);
        return (int)(defaultValue + (fixedValue + originalValue) * multipler);
    }

    private float GetCharacterPhysicalDef(float multipler = 1)
    {
        int fixedValue = characterStatsReference.GetCharacterStatus(characterReference.level).GetVitality;
        int originalValue = MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(characterReference.name);
        return (fixedValue + originalValue) * multipler;
    }

    private int GetCharacterMagic(float multipler = 1)
    {
        int fixedValue = characterStatsReference.GetCharacterStatus(characterReference.level).GetMagic;
        int originalValue = MeloMelo_ExtraStats_Settings.GetExtraMagicStats(characterReference.name);
        return (int)((fixedValue + originalValue) * multipler);
    }

    private float GetCharacterMagicDef(float multipler = 1)
    {
        int fixed_magicFormula = characterStatsReference.GetCharacterStatus(characterReference.level).GetMagic;
        int originalValue = MeloMelo_ExtraStats_Settings.GetExtraMagicStats(characterReference.name);

        int currentVitValue = characterStatsReference.GetCharacterStatus(characterReference.level).GetVitality;
        int originalVitValue = MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(characterReference.name);

        return fixed_magicFormula + originalValue - (currentVitValue + originalVitValue) * multipler;
    }
    #endregion

    #region MISC
    public void GetCharacterBase(ClassBase character, MeloMelo_RPGEditor.StatsManage_Database characterStats)
    {
        characterReference = character;
        characterStatsReference = characterStats;
    }
    #endregion
}
