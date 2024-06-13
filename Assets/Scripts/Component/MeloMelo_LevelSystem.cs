using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeloMelo_LevelSystem : MonoBehaviour
{
    public Text CharacterLevel;
    public Text EnemyLevel;

    private int mainSet = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StandingBy", 0.5f);
    }

    void SearchMainCharacter()
    {
        for (int i = 1; i < 4; i++)
            if (PlayerPrefs.GetString("Slot" + i + "_mainSet", "F") == "T")
                mainSet = i;
    }

    private void StandingBy()
    {
        SearchMainCharacter();

        CharacterLevel.text = "LV. " + PlayerPrefs.GetInt(PlayerPrefs.GetString("Slot" + mainSet + "_charName", "Warrior") + "_LEVEL", 1);
        EnemyLevel.text = "LV. " + BeatConductor.thisBeat.Music_Database.Insert_Enemy[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].level;
    }
}
