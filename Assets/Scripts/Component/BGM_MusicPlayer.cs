using System.Collections;
using UnityEngine;

class MusicPreviewData
{
    public AudioClip track;
    public float previewTime;
    public int currentSelectionTrack;
    
    public MusicPreviewData(AudioClip track, float previewTime, int currentSelection)
    {
        this.track = track;
        this.previewTime = previewTime;
        currentSelectionTrack = currentSelection;
    }
}

public class BGM_MusicPlayer : MonoBehaviour
{
    private bool musicFading = false;
    private readonly float musicFadeTime = 30;
    private MusicPreviewData musicPlayerData = null;

    void Start()
    {
        GetVolume_Setting();
    }

    void Update()
    {
        if (musicPlayerData != null)
        {
            switch (GetComponent<AudioSource>().isPlaying)
            {
                case true:
                    if (!musicFading && GetComponent<AudioSource>().time >= musicPlayerData.previewTime + musicFadeTime)
                    {
                        musicFading = true;
                        StartCoroutine(FadeMusic(musicPlayerData.currentSelectionTrack));
                    }
                    break;

                case false:
                    musicFading = false;
                    GetComponent<AudioSource>().time = musicPlayerData.previewTime;
                    GetVolume_Setting();
                    GetComponent<AudioSource>().Play();
                    break;
            }
        }
    }

    IEnumerator FadeMusic(int current)
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<AudioSource>().volume -= 0.05f;

        if (GetComponent<AudioSource>().volume <= 0 || SelectionMenu_Script.thisSelect.get_selection.get_ScrollNagivator_ProgressBar.value != current)
        { GetComponent<AudioSource>().Stop(); }
        else { StartCoroutine(FadeMusic(current)); }
    }

    private void GetVolume_Setting() 
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAudioMute_ValueKey) == 1 ?
            0 : PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetBGM_ValueKey); 
    }

    public void UpdateTrackDetails(AudioClip trackData, float preview, int currentSelect)
    {
        GetComponent<AudioSource>().clip = trackData;
        GetComponent<AudioSource>().time = preview;
        GetComponent<AudioSource>().Play();

        musicPlayerData = new MusicPreviewData(trackData, preview, currentSelect);
    }
}
