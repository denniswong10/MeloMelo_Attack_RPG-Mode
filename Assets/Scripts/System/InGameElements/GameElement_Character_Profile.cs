using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_RPGEditor;

[System.Serializable]
public class Character_Base_Data
{
    public string charName { get; private set; }
    public string className { get; private set; }

    public int level { get; private set; }
    public int experience { get; private set; }

    public Character_Stats_Structure fixedStats { get; private set; }
    public Character_Stats_Structure additionalStats { get; private set; }

    public Character_Extra_Structure additionalProfile { get; private set; }

    public Character_Base_Data()
    {
        charName = string.Empty;
        className = string.Empty;

        level = 1;
        experience = 0;
    }

    #region SETUP
    public void LoadCharacterData(ClassBase data)
    {
        charName = data.characterName;
        className = data.name;

        fixedStats = new Character_Stats_Structure();
        RefreshCharacterStats();
    }

    public void LoadCharacterLevelData(int level) => this.level = level;
    public void LoadCharacterExperienceData(int experience) => this.experience = experience;

    public void LoadAdditionalCharacterData(int str, int vit, int mag, int health)
    {
        additionalStats = new Character_Stats_Structure();

        additionalStats.strength = str;
        additionalStats.vitalilty = vit;
        additionalStats.magic = mag;
        additionalStats.baseHealth = health;
    }

    public void LoadExtraInfoCharacterData(int reborn_count, int addedMastery, int dosagePoint)
    {
        additionalProfile = new Character_Extra_Structure();
        additionalProfile.rebirth_count = reborn_count;
        additionalProfile.masteryPoint = addedMastery;
        additionalProfile.dosageMasteryPoint = dosagePoint;
    }
    #endregion

    #region MAIN
    public void RefreshCharacterStats()
    {
        if (fixedStats != null)
        {
            StatsManage_Database data = new StatsManage_Database(className);
            CharacterStats stats = data.GetCharacterStatus(level);
            UpdateCharacterFixedStats(stats.GetStrength, stats.GetVitality, stats.GetMagic, stats.GetHealth);
        }
    }

    public void CheckCharacterLeveling(int maxExperience)
    {
        if (experience >= maxExperience)
        {
            level++;
            experience -= maxExperience;
        }
    }

    public void UpdateCharacterExperience(int amount) => experience += amount;

    public void UpdateCharacterExtraProfile(int rebirth_amount, int mastery_amount)
    {
        if (additionalProfile != null)
        {
            additionalProfile.rebirth_count += rebirth_amount;
            additionalProfile.masteryPoint += mastery_amount;
        }
    }

    public void UpdateCharacterExtraMasteryPoint(int amount) => additionalProfile.dosageMasteryPoint += amount;

    private void UpdateCharacterFixedStats(int str, int vit, int mag, int health)
    {
        fixedStats.strength = str;
        fixedStats.vitalilty = vit;
        fixedStats.magic = mag;
        fixedStats.baseHealth = health;
    }

    public void UpdateCharacterExtraStats(int str, int vit, int mag, int health)
    {
        if (additionalStats != null)
        {
            additionalStats.strength += str;
            additionalStats.vitalilty += vit;
            additionalStats.magic += mag;
            additionalStats.baseHealth += health;
        }
    }
    #endregion

    #region MISC
    public int GetCharacterRawMasteryPoint()
    {
        const int pointAcquire = 2;
        StatsManage_Database getCharStats = new StatsManage_Database(className);
        int rawpointFromRebirth = pointAcquire * getCharStats.GetCharacterMaxLevel() * (additionalProfile != null ? additionalProfile.rebirth_count : 0);

        return MeloMelo_ItemUsage_Settings.GetPreSumbitOfMasteryPoint(className) + (rawpointFromRebirth + level * pointAcquire -
            (additionalProfile != null ? additionalProfile.masteryPoint : 0));
    }

    public void ResetProfile()
    {
        level = 1;
        experience = 0;

        if (additionalStats != null) additionalStats = null;
        if (additionalProfile != null) additionalProfile = null;
    }
    #endregion
}

[System.Serializable]
public class Character_Stats_Structure
{
    public int strength;
    public int vitalilty;
    public int magic;
    public int baseHealth;

    public Character_Stats_Structure()
    {
        strength = 0;
        vitalilty = 0;
        magic = 0;
        baseHealth = 0;
    }
}

[System.Serializable]
public class Character_Extra_Structure
{
    public int rebirth_count;
    public int masteryPoint;
    public int dosageMasteryPoint;

    public Character_Extra_Structure()
    {
        rebirth_count = 0;
        masteryPoint = 0;
        dosageMasteryPoint = 0;
    }
}