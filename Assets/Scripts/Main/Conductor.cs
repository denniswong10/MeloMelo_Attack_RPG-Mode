using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor thisCondut;
    public MusicScore Music_Database;

    // Loading up all component through the game
    void Start()
    {
        thisCondut = this;

        try { Music_Database = SelectionMenu_Script.thisSelect.get_selection.get_form; }
        catch { }

        GetComponent<AudioSource>().clip = Music_Database.Music;

        Invoke("StartEncode", 0.05f);
    }

    void StartEncode()
    {
        TutorialManager.thisManager.Invoke("GameStarting", 0.05f);
    }

    // Music PlayThrough: Starting Point
    public void StartMusicButton()
    {
        GameObject.Find("MidAlert").gameObject.SetActive(false);
        GetComponent<AudioSource>().Play();
    }
}
