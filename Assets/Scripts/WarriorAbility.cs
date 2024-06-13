using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_RPGEditor;

public class WarriorAbility : MonoBehaviour
{
    private StatsDistribution stats = new StatsDistribution();

    void Start()
    {
        stats.load_Stats();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "note")
        {
            GameManager.thisManager.UpdateNoteStatus("Perfect");
            GameManager.thisManager.UpdateScore_Tech((stats.get_UnitPower("Character") / 3) * 2);
            if (other.GetComponent<Note_Script>().note_index == 1 || other.GetComponent<Note_Script>().note_index == 6) { GameManager.thisManager.UpdateBattle_Progress((float)100 / GameManager.thisManager.getGameplayComponent.getTotalEnemy); }
            Destroy(other.gameObject);
        }     
    }
}
