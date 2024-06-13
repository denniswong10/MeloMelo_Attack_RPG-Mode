using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_ExtraComponent;

public class HookNote_Script : MonoBehaviour
{
    public float note_speed;
    public int note_index;
    private bool move = true;
    public bool moveR = false;

    private bool perfectNote = false;
    public bool get_perfectNote { get { return perfectNote; } }

    private bool poorNote = false;
    public bool get_poorNote { get { return poorNote; } }
    public float NotePos;

    private ScoreFixedValue scoreF = new ScoreFixedValue();

    private int NextHitTiming;
    public int get_NextHitTiming { get { return NextHitTiming; } }

    void Start()
    {
        // Set note position from start
        transform.position = new Vector3(transform.position.x, NotePos, transform.position.z);
        CheckForFixedNote(PlayerPrefs.GetInt("NoteSpeed", 0));

        // Check For Option
        if (PlayerPrefs.GetInt("AirGuide_valve", 0) == 0 && (note_index == 4 || note_index == 6))
            try { transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Play(); } catch { }

        NextHitTiming = BeatConductor.thisBeat.get_spawnObject + 5;
    }

    // Battle-Setup: Reference Script
    void CheckForFixedNote(int index)
    {
        if (BeatConductor.thisBeat.Music_Database.BPM > 360) { note_speed = (65 + (index * 5)); }
        else if (BeatConductor.thisBeat.Music_Database.BPM > 340) { note_speed = (60 + (index * 5)); }
        else if (BeatConductor.thisBeat.Music_Database.BPM > 300) { note_speed = (55 + (index * 5)); }
        else if (BeatConductor.thisBeat.Music_Database.BPM > 280) { note_speed = (50 + (index * 5)); }
        else if (BeatConductor.thisBeat.Music_Database.BPM > 260) { note_speed = (45 + (index * 5)); }
        else if (BeatConductor.thisBeat.Music_Database.BPM > 240) { note_speed = (40 + (index * 5)); }
        else if (BeatConductor.thisBeat.Music_Database.BPM > 200) { note_speed = (30 + (index * 5)); }
        else if (BeatConductor.thisBeat.Music_Database.BPM > 160) { note_speed = (25 + (index * 5)); }
        else if (BeatConductor.thisBeat.Music_Database.BPM > 150) { note_speed = (23 + (index * 5)); }
        else if (BeatConductor.thisBeat.Music_Database.BPM <= 150) { note_speed = (20 + (index * 5)); }
    }

    void Note_Movement()
    {
        switch (note_index)
        {
            case 2:
            case 7:
                ObstacleEnd_Check(-0.5f);
                break;

            default:
                ObstacleEnd_Check(0.2f);
                break;
        }
    }

    // Note Check: End Point for Traps
    protected void ObstacleEnd_Check(float offset)
    {
        if (transform.position.z <= GameObject.Find("Judgement Line").transform.position.z + offset)
        {
            move = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.Find("Judgement Line").transform.position.z + offset);
            if (BeatConductor.thisBeat.get_spawnObject >= NextHitTiming) StartCoroutine(CheckingPerfect_Hit());
        }
        else
        {
            try { transform.Translate(Vector3.back * BeatConductor.thisBeat.get_BPM_Calcuate * note_speed * Time.deltaTime); }
            catch { transform.Translate(Vector3.back * note_speed * Time.deltaTime); }
        }
    }

    // Note Function: Reserve Mode
    public void HitNoteReserve() { move = false; moveR = true; }

    void NoteReserve_Movement()
    {
        if (transform.position.z >= 1)
        {
            Destroy(gameObject);
        }
        else
        {
            try { transform.Translate(Vector3.forward * BeatConductor.thisBeat.get_BPM_Calcuate * note_speed * Time.deltaTime); }
            catch { transform.Translate(Vector3.forward * note_speed * Time.deltaTime); }
        }
    }

    IEnumerator CheckingPerfect_Hit()
    {
        float i = 0;
        try { i = (BeatConductor.thisBeat.get_BPM_Calcuate / 4) * 2; }
        catch { i = 0.5f; }
        yield return new WaitForSeconds(i);

        if (note_index == 2 || note_index == 7)
        {
            // Traps (Ground) Traps (Air)
            Obstacle_NoteDodge();
        }

        StartCoroutine(CheckTiming_NoteHit());
    }

    // Note Check: Hit Timing
    IEnumerator CheckTiming_NoteHit()
    {
        float i = 0;
        try { i = (BeatConductor.thisBeat.get_BPM_Calcuate / 4) * 2; }
        catch { i = 0.5f; }
        yield return new WaitForSeconds(i);

        if (note_index == 1 || note_index == 6 || note_index == 5 || note_index == 9)
        {
            // Enemy Attack
            poorNote = true;

            try { i = BeatConductor.thisBeat.get_BPM_Calcuate; }
            catch { i = 0.5f; }
            yield return new WaitForSeconds(i);

            if (!moveR)
            {
                try
                {
                    GameManager.thisManager.UpdateNoteStatus("Miss");
                    if (GameObject.Find("Character").GetComponent<Character>().stats.get_name != "NA")
                    {
                        GameManager.thisManager.UpdateCharacter_Health(-(PlayerPrefs.GetInt("Enemy_OverallDamage", 0) * 2), false);
                    }
                }
                catch { TutorialManager.thisManager.UpdateNoteStatus("Miss"); }
                Destroy(gameObject);
            }
        }

        else
        {
            poorNote = true;

            try { i = BeatConductor.thisBeat.get_BPM_Calcuate; }
            catch { i = 0.5f; }
            yield return new WaitForSeconds(i);

            try
            {
                GameManager.thisManager.UpdateNoteStatus("Miss");
            }
            catch { TutorialManager.thisManager.UpdateNoteStatus("Miss"); }
            Destroy(gameObject);
        }
    }

    // Note Function: Trap Evasion
    protected void Obstacle_NoteDodge()
    {
        try
        {
            GameManager.thisManager.UpdateNoteStatus("Perfect_2");

            if (!GameManager.thisManager.DeveloperMode) { AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/EnemyAttack"), new Vector3(0, 0, -10f)); }
            GameManager.thisManager.UpdateScore(BeatConductor.thisBeat.get_scorePerfect + scoreF.score_combo());
        }
        catch
        {
            TutorialManager.thisManager.UpdateNoteStatus("Perfect_2");
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/EnemyAttack"), new Vector3(0, 0, -10f));
        }

        Destroy(gameObject);
    }

    void Update()
    {
        try
        {
            if (GameManager.thisManager.DeveloperMode)
            {
                Slider peace = GameObject.Find("PeaceAdjuster").GetComponent<Slider>();
                note_speed = peace.value;
            }
        }
        catch
        {
            //note_speed = PlayerPrefs.GetInt("NoteSpeed", 20);
        }

        if (move) { Note_Movement(); }
        if (moveR) { NoteReserve_Movement(); }
    }
}
