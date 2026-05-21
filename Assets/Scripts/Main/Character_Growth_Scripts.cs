using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Growth_Scripts : MonoBehaviour
{
    [SerializeField] private GameObject PromptPanel;
    [SerializeField] private Text MainStatus;
    [SerializeField] private Text SubStatus;
    [SerializeField] private Text GrowthValue;

    private ElemetStartingStats currentStats;
    private Character_Base_Data mainStatsReference;
    private ClassBase characterInfo;

    private int totalPhysicalDamage = 0;
    private int totalMagicDamage = 0;
    private float totalPhysicalDefense = 0;
    private float totalMagicDefense = 0;
    private int characterMaxLevel = 1;

    #region SETUP
    public void InputCharacterInfo(Character_Base_Data character, ClassBase profile, int maxLevel)
    {
        mainStatsReference = character;
        characterInfo = profile;
        characterMaxLevel = maxLevel;

        LoadStartingStatus();
        GetMainStatusContent();
        GetSubStatusContent();
        GetTotalGrowthPower();
    }
    #endregion

    #region MAIN
    public void GetDetailForMainStatus(bool visible)
    {
        PromptMessage("Attack Damage, Magic Damage, Physical Defense, Magic Defense, Health Point", visible);
    }

    public void GetDetailForSubStatus(bool visible)
    {
        PromptMessage("Piercing Damage, Brust Damage, Critical Rate, Experience Obtain", visible);
    }

    public void GetDetailOnGrowthPower(bool visible)
    {
        PromptMessage("Growth Power\n( Overall status for this character as of unit power )", visible);
    }
    #endregion

    #region COMPONENT
    private void LoadStartingStatus()
    {
        currentStats =
                MeloMelo_ExtensionContent_Settings.GetStatsWithElementBonus(characterInfo.elementType == ClassBase.ElementStats.Light ? "Light" :
                characterInfo.elementType == ClassBase.ElementStats.Dark ? "Dark" :
                characterInfo.elementType == ClassBase.ElementStats.Earth ? "Earth" : "None");

        mainStatsReference.RefreshCharacterStats();

        totalPhysicalDamage =
            MeloMelo_CharacterInfo_Settings.GetCharacterPhysical(
                currentStats.strength,
                mainStatsReference.fixedStats.strength + (mainStatsReference.additionalStats != null ? mainStatsReference.additionalStats.strength : 0),
                0,
                currentStats.strength
                );

        totalPhysicalDefense =
            MeloMelo_CharacterInfo_Settings.GetCharacterPhysicalDef(
                mainStatsReference.fixedStats.vitalilty + (mainStatsReference.additionalStats != null ? mainStatsReference.additionalStats.vitalilty : 0),
                0,
                currentStats.vitality
                );

        totalMagicDamage =
            MeloMelo_CharacterInfo_Settings.GetCharacterMagic(
                 mainStatsReference.fixedStats.magic + (mainStatsReference.additionalStats != null ? mainStatsReference.additionalStats.magic : 0),
                 0,
                 currentStats.magic
                 );

        totalMagicDefense =
            MeloMelo_CharacterInfo_Settings.GetCharacterMagicDef(
                mainStatsReference.fixedStats.magic + (mainStatsReference.additionalStats != null ? mainStatsReference.additionalStats.magic : 0),
                mainStatsReference.fixedStats.vitalilty + (mainStatsReference.additionalStats != null ? mainStatsReference.additionalStats.vitalilty : 0),
                0,
                currentStats.multipler
                );
    }

    private void GetMainStatusContent()
    {
        int health =
            MeloMelo_CharacterInfo_Settings.GetCharacterHealth(
                10,
                mainStatsReference.fixedStats.vitalilty + (mainStatsReference.additionalStats != null ? mainStatsReference.additionalStats.vitalilty : 0),
                mainStatsReference.fixedStats.baseHealth + (mainStatsReference.additionalStats != null ? mainStatsReference.additionalStats.baseHealth : 0)
                );

        MainStatus.text = totalPhysicalDamage + " / " + totalMagicDamage + " / "
            + totalPhysicalDefense + " / " + Mathf.Clamp(totalMagicDefense, 0, totalMagicDefense) + " / " + health;
    }

    private void GetSubStatusContent()
    {
        float pericingDamage = (float)MeloMelo_CharacterInfo_Settings.GetCharacterExceedLimit(
            currentStats.multipler, currentStats.limit,
            mainStatsReference.level + (mainStatsReference.additionalProfile != null ? mainStatsReference.additionalProfile.rebirth_count : 0) * characterMaxLevel,
            totalPhysicalDamage);

        float brustDamage = (float)MeloMelo_CharacterInfo_Settings.GetCharacterExceedLimit(
            currentStats.multipler, currentStats.limit,
            mainStatsReference.level + (mainStatsReference.additionalProfile != null ? mainStatsReference.additionalProfile.rebirth_count : 0) * characterMaxLevel,
            totalMagicDamage);

        float criticalRate = (float)MeloMelo_CharacterInfo_Settings.GetCharacterCriticalRate(
            totalPhysicalDamage, totalPhysicalDefense + totalMagicDamage + totalMagicDefense, 3,
            mainStatsReference.level + (mainStatsReference.additionalProfile != null ? mainStatsReference.additionalProfile.rebirth_count : 0) * characterMaxLevel);

        float maxObtainExperience = 0;
        if (mainStatsReference.additionalProfile != null)
        {
            int totalPoint = (mainStatsReference.level + (mainStatsReference.additionalProfile.rebirth_count * characterMaxLevel)) * 2;
            int unusedPoint = totalPoint - mainStatsReference.additionalProfile.masteryPoint;

            maxObtainExperience =
                (float)MeloMelo_CharacterInfo_Settings.GetCharacterMaxExperienceObtain(
                    totalPoint, mainStatsReference.additionalProfile.dosageMasteryPoint,
                    Mathf.Clamp(unusedPoint, 0, unusedPoint) + mainStatsReference.additionalProfile.masteryPoint
                    );
        }

        SubStatus.text = pericingDamage + "% / " + brustDamage + "% / " + Mathf.Clamp(criticalRate, 0, criticalRate) + "% / " + maxObtainExperience + "%";
    }

    private void GetTotalGrowthPower()
    {
        float damagePower = (mainStatsReference.fixedStats.strength +
            (mainStatsReference.additionalStats != null ? mainStatsReference.additionalStats.strength : 0)) * currentStats.strength
            +
            (mainStatsReference.fixedStats.magic +
            (mainStatsReference.additionalStats != null ? mainStatsReference.additionalStats.magic : 0)) * currentStats.magic;

        float defensivePower = (mainStatsReference.fixedStats.vitalilty +
            (mainStatsReference.additionalStats != null ? mainStatsReference.additionalStats.vitalilty : 0)) * currentStats.vitality;

        int lifeSupportStrength = (int)((mainStatsReference.fixedStats.baseHealth +
            (mainStatsReference.additionalStats != null ? mainStatsReference.additionalStats.baseHealth : 0)) * 10 * currentStats.multipler);

        GrowthValue.text = MeloMelo_PlayerSettings.GetScoreConfigure((int)(damagePower + defensivePower + lifeSupportStrength));
    }

    private void PromptMessage(string message, bool isVisible)
    {
        PromptPanel.SetActive(isVisible);
        PromptPanel.GetComponentInChildren<Text>().text = message;
    }
    #endregion

    #region MISC 
    public void OpenSubPanel(GameObject target)
    {
        target.SetActive(true);
    }

    public void CloseSubPanel(GameObject target)
    {
        target.SetActive(false);
    }
    #endregion
}
