using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_RPGEditor;

public class CharacterInfo_DataBuild : MonoBehaviour
{
    [SerializeField] private Text AttackDamage;
    [SerializeField] private Text AttackDefense;
    [SerializeField] private Text MagicDamage;
    [SerializeField] private Text MagicDefense;
    [SerializeField] private Text Health;
    private StatsDistribution reference;

    private ElemetStartingStats currentStats;
    private ClassBase characterReference = null;
    private StatsManage_Database characterStatsReference = null;

    void Start()
    {
        reference = new StatsDistribution();
    }

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

            Health.text = Mathf.Clamp(GetCharacterHealth(), 0, GetCharacterHealth()).ToString();
            AttackDamage.text = Mathf.Clamp(GetCharacterPhysical(currentStats.strength, currentStats.strength), 0, GetCharacterPhysical(currentStats.strength, currentStats.strength)).ToString();
            AttackDefense.text = Mathf.Clamp(GetCharacterPhysicalDef(currentStats.vitality), 0, GetCharacterPhysicalDef(currentStats.vitality)).ToString();
            MagicDefense.text = Mathf.Clamp(GetCharacterMagicDef(currentStats.multipler), 0, GetCharacterMagicDef(currentStats.multipler)).ToString();
            MagicDamage.text = Mathf.Clamp(GetCharacterMagic(currentStats.magic), 0, GetCharacterMagic(currentStats.magic)).ToString();
        }
    }
    #endregion

    #region COMPONENT 
    private int GetCharacterHealth(float multipler = 1)
    {
        int baseValue = characterStatsReference.GetCharacterStatus(characterReference.level).GetVitality;
        int permanentValue = MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(characterReference.name);
        int healthValue = characterStatsReference.GetCharacterStatus(characterReference.level).GetHealth + MeloMelo_ExtraStats_Settings.GetExtraBaseHealth(characterReference.name);

        return (int)((baseValue + permanentValue) * (reference.baseHealth * multipler) + healthValue);
    }

    private int GetCharacterPhysical(float defaultValue, float multipler = 1)
    {
        int baseValue = characterStatsReference.GetCharacterStatus(characterReference.level).GetStrength;
        int permanentValue = MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(characterReference.name);
        return (int)(defaultValue + (baseValue + permanentValue) * multipler);
    }

    private float GetCharacterPhysicalDef(float multipler = 1)
    {
        int baseValue = characterStatsReference.GetCharacterStatus(characterReference.level).GetVitality;
        int permanentValue = MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(characterReference.name);
        return (baseValue + permanentValue) * multipler;
    }

    private int GetCharacterMagic(float multipler = 1)
    {
        int baseValue = characterStatsReference.GetCharacterStatus(characterReference.level).GetMagic;
        int permanentValue = MeloMelo_ExtraStats_Settings.GetExtraMagicStats(characterReference.name);
        return (int)((baseValue + permanentValue) * multipler);
    }

    private float GetCharacterMagicDef(float multipler = 1)
    {
        int baseMagicFormula = characterStatsReference.GetCharacterStatus(characterReference.level).GetMagic;
        int permanentMagValue = MeloMelo_ExtraStats_Settings.GetExtraMagicStats(characterReference.name);

        int baseVitFormula = characterStatsReference.GetCharacterStatus(characterReference.level).GetVitality;
        int permanentVitValue = MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(characterReference.name);

        return (baseMagicFormula + permanentMagValue - (baseVitFormula + permanentVitValue)) * multipler;
    }
    #endregion

    #region MISC
    public void GetCharacterBase(ClassBase character, StatsManage_Database characterStats)
    {
        characterReference = character;
        characterStatsReference = characterStats;
    }
    #endregion
}
