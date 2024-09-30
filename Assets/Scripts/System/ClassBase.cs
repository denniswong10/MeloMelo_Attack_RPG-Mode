using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_RPGEditor;

[CreateAssetMenu(fileName = "stats",menuName = "CharacterStats",order = 1)]
public class ClassBase : ScriptableObject
{
    public int health;
    public int strength;
    public int magic;
    public int vitality;
    public int experience;
    public float expLimit;
    public int level;
    public string characterName;
    public Sprite icon;
    public enum ElementStats { Light, Dark, Earth };
    public ElementStats elementType;

    #region MAIN
    public void CheckLeveling(int max) 
    {
        // Character is bound to level up, raise a level,
        // remaining from stock goes into the experience as of next level
        if (experience >= max)
        {
            PlayerPrefs.SetInt(name + "_LEVEL", level + 1);
            int addonsToNewMastery = MeloMelo_ExtraStats_Settings.GetMasteryPoint(name) + 2;
            MeloMelo_ExtraStats_Settings.SetMasteryPoint(name, addonsToNewMastery);

            int experienceInStock = experience - max;
            if (experienceInStock > 0) PlayerPrefs.SetInt(name + "_EXP", experienceInStock);
        }

        // Update local character stats to the latest one
        level = PlayerPrefs.GetInt(name + "_LEVEL");
        experience = PlayerPrefs.GetInt(name + "_EXP"); ;
        expLimit = max;
    }

    // Stats Loader: Function
    public void StatLoader() {  }
    #endregion

    #region CHECKER_ZONE
    public void UpdateCurrentStats(bool set)
    {
        // Manage of level and experience been assign to this character
        if (set)
        {
            PlayerPrefs.SetInt(name + "_LEVEL", level);
            PlayerPrefs.SetInt(name + "_EXP", experience);
        }

        level = PlayerPrefs.GetInt(name + "_LEVEL", 1);
        experience = PlayerPrefs.GetInt(name + "_EXP", 0);
    }

    public void UpdateStatsCache(bool set)
    {
        // Manage of character main stats been assign to this character
        if (set)
        {
            PlayerPrefs.SetInt(name + "_HEALTH", health);
            PlayerPrefs.SetInt(name + "_STRENGTH", strength);
            PlayerPrefs.SetInt(name + "_VITALITY", vitality);
            PlayerPrefs.SetInt(name + "_MAGIC", magic);
        }

        health = PlayerPrefs.GetInt(name + "_HEALTH", 0);
        strength = PlayerPrefs.GetInt(name + "_STRENGTH", 0);
        vitality = PlayerPrefs.GetInt(name + "_VITALITY", 0);
        magic = PlayerPrefs.GetInt(name + "_MAGIC", 0);
    }
    #endregion
}
