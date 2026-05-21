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
    private Character_Base_Data characterReference = null;
    private StatsManage_Database characterStatsReference = null;
    private ClassBase characterTemplate = null;

    void Start()
    {
        reference = new StatsDistribution();
    }

    void Update()
    {
        GetStatsUpdated();
    }

    #region SETUP
    private void GetStatsUpdated()
    {
        if (characterReference != null && characterStatsReference != null)
        {
            currentStats =
                MeloMelo_ExtensionContent_Settings.GetStatsWithElementBonus(characterTemplate.elementType == ClassBase.ElementStats.Light ? "Light" :
                characterTemplate.elementType == ClassBase.ElementStats.Dark ? "Dark" :
                characterTemplate.elementType == ClassBase.ElementStats.Earth ? "Earth" : "None");

            if (characterReference.additionalStats != null)
            {
                int healthPoint_value =
                    MeloMelo_CharacterInfo_Settings.GetCharacterHealth(
                        reference.baseHealth,
                        characterReference.fixedStats.vitalilty + characterReference.additionalStats.vitalilty,
                        characterReference.fixedStats.baseHealth + characterReference.additionalStats.baseHealth
                        );

                int attackDamage_value =
                    MeloMelo_CharacterInfo_Settings.GetCharacterPhysical(
                        currentStats.strength,
                        characterReference.fixedStats.strength + characterReference.additionalStats.strength,
                        0,
                        currentStats.strength
                        );

                float defense_value =
                    MeloMelo_CharacterInfo_Settings.GetCharacterPhysicalDef(
                        characterReference.fixedStats.vitalilty + characterReference.additionalStats.vitalilty,
                        0,
                        currentStats.vitality
                        );

                float magicDefense_value =
                    MeloMelo_CharacterInfo_Settings.GetCharacterMagicDef(
                        characterReference.fixedStats.magic + characterReference.additionalStats.magic,
                        characterReference.fixedStats.vitalilty + characterReference.additionalStats.vitalilty,
                        0,
                        currentStats.multipler
                        );

                int magicDamage_value =
                    MeloMelo_CharacterInfo_Settings.GetCharacterMagic(
                        characterReference.fixedStats.magic + characterReference.additionalStats.magic,
                        0,
                        currentStats.magic
                        );

                Health.text = Mathf.Clamp(healthPoint_value, 0, healthPoint_value).ToString();
                AttackDamage.text = Mathf.Clamp(attackDamage_value, 0, attackDamage_value).ToString();
                AttackDefense.text = Mathf.Clamp(defense_value, 0, defense_value).ToString();
                MagicDefense.text = Mathf.Clamp(magicDefense_value, 0, magicDefense_value).ToString();
                MagicDamage.text = Mathf.Clamp(magicDamage_value, 0, magicDamage_value).ToString();
            }
            else
            {
                Health.text = MeloMelo_CharacterInfo_Settings.GetCharacterHealth(
                        reference.baseHealth, characterReference.fixedStats.vitalilty, characterReference.fixedStats.baseHealth).ToString();

                AttackDamage.text = MeloMelo_CharacterInfo_Settings.GetCharacterPhysical(
                    currentStats.strength, characterReference.fixedStats.strength, 0, currentStats.strength).ToString();

                AttackDefense.text = MeloMelo_CharacterInfo_Settings.GetCharacterPhysicalDef(
                        characterReference.fixedStats.vitalilty, 0, currentStats.vitality).ToString();

                float rawMagicDefenseValue = MeloMelo_CharacterInfo_Settings.GetCharacterMagicDef(
                    characterReference.fixedStats.magic, characterReference.fixedStats.vitalilty, 0, currentStats.multipler);

                MagicDefense.text = Mathf.Clamp(rawMagicDefenseValue, 0, rawMagicDefenseValue).ToString();

                MagicDamage.text = MeloMelo_CharacterInfo_Settings.GetCharacterMagic(
                    characterReference.fixedStats.magic, 0, currentStats.magic).ToString();
            }
        }
    }
    #endregion

    #region MISC
    public void GetCharacterBase(Character_Base_Data character, ClassBase template, StatsManage_Database characterStats)
    {
        characterReference = character;
        characterTemplate = template;
        characterStatsReference = characterStats;
    }
    #endregion
}
