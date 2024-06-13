using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_MusicPlayer : MonoBehaviour
{
    private bool musicFading = false;

    void Start()
    {
        GetVolume_Setting();
    }

    void Update()
    {
        try
        {
            if (SelectionMenu_Script.thisSelect.get_loadBGM)
            {
                if (!musicFading && GetComponent<AudioSource>().time >= SelectionMenu_Script.thisSelect.get_selection.get_form.PreviewTime + 10 && GetComponent<AudioSource>().isPlaying)
                {
                    musicFading = true;
                    StartCoroutine(FadeMusic((int)SelectionMenu_Script.thisSelect.get_selection.get_ScrollNagivator_ProgressBar.value));
                }

                if (!GetComponent<AudioSource>().isPlaying)
                {
                    musicFading = false;
                    GetComponent<AudioSource>().time = SelectionMenu_Script.thisSelect.get_selection.get_form.PreviewTime;
                    GetVolume_Setting();
                    GetComponent<AudioSource>().Play();
                }
            }
        }
        catch
        {
            try
            {
                if (ArenaSelection_Script.thisArena.get_loadBGM)
                {
                    if (!musicFading && GetComponent<AudioSource>().time >= ArenaSelection_Script.thisArena.MusicList[ArenaSelection_Script.thisArena.get_selector - 1].PreviewTime + 10 && GetComponent<AudioSource>().isPlaying)
                    {
                        musicFading = true;
                        StartCoroutine(FadeMusic(ArenaSelection_Script.thisArena.get_selector));
                    }

                    if (!GetComponent<AudioSource>().isPlaying)
                    {
                        musicFading = false;
                        GetComponent<AudioSource>().time = ArenaSelection_Script.thisArena.MusicList[ArenaSelection_Script.thisArena.get_selector - 1].PreviewTime;
                        GetVolume_Setting();
                        GetComponent<AudioSource>().Play();
                    }
                }
            }
            catch { }
        }
    }

    IEnumerator FadeMusic(int current)
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<AudioSource>().volume -= 0.05f;
        try { if (GetComponent<AudioSource>().volume <= 0 || SelectionMenu_Script.thisSelect.get_selection.get_ScrollNagivator_ProgressBar.value != current) { GetComponent<AudioSource>().Stop(); } else { StartCoroutine(FadeMusic(current)); } }
        catch { if (GetComponent<AudioSource>().volume <= 0 || ArenaSelection_Script.thisArena.get_selector != current) { GetComponent<AudioSource>().Stop(); } else { StartCoroutine(FadeMusic(current)); } }
    }

    protected void GetVolume_Setting() { GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("BGM_VolumeGET", 1); }
}
