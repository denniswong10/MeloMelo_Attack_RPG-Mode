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

    void Update()
    {
        GetStatsUpdated();
    }

    #region SETUP
    private void GetStatsUpdated()
    {
        if (characterReference != null)
        {
            currentStats = 
                MeloMelo_GameSettings.GetStatsWithElementBonus(characterReference.elementType == ClassBase.ElementStats.Light ? "Light" :
                characterReference.elementType == ClassBase.ElementStats.Dark ? "Dark" :
                characterReference.elementType == ClassBase.ElementStats.Earth ? "Earth" : "None");

            Health.text = Mathf.Clamp((characterReference.vitality + MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(characterReference.name)) * 
                (10 * currentStats.multipler) + characterReference.health + 
                MeloMelo_ExtraStats_Settings.GetExtraBaseHealth(characterReference.name), 0,

                (characterReference.vitality + MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(characterReference.name)) * 
                (10 * currentStats.multipler) + characterReference.health +
                MeloMelo_ExtraStats_Settings.GetExtraBaseHealth(characterReference.name)).ToString();

            AttackDamage.text = Mathf.Clamp((characterReference.strength + MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(characterReference.name))
                * currentStats.strength + currentStats.strength, 0,
                (characterReference.strength + MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(characterReference.name)) * currentStats.strength
                + currentStats.strength).ToString();

            AttackDefense.text = Mathf.Clamp((characterReference.vitality + MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(characterReference.name)) 
                * currentStats.vitality, 0,
               (characterReference.vitality + MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(characterReference.name)) * currentStats.vitality).ToString();

            MagicDefense.text = Mathf.Clamp((characterReference.magic + MeloMelo_ExtraStats_Settings.GetExtraMagicStats(characterReference.name)
                - (characterReference.vitality + MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(characterReference.name))) * currentStats.multipler, 0,

                (characterReference.magic + MeloMelo_ExtraStats_Settings.GetExtraMagicStats(characterReference.name)
                - (characterReference.vitality + MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(characterReference.name))) * currentStats.multipler).ToString();

            MagicDamage.text = Mathf.Clamp((characterReference.magic + MeloMelo_ExtraStats_Settings.GetExtraMagicStats(characterReference.name)) 
                * currentStats.magic, 0,
                (characterReference.magic + MeloMelo_ExtraStats_Settings.GetExtraMagicStats(characterReference.name)) * currentStats.magic).ToString();
        }
    }
    #endregion

    #region MISC
    public void GetCharacterBase(ClassBase character)
    {
        characterReference = character;
    }
    #endregion
}
