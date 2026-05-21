using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MeloMelo_RPGEditor;
using MeloMelo_ExtraComponent;

public class LoadingTransition_Script : MonoBehaviour
{
    public static LoadingTransition_Script thisLoader;
    private StatsDistribution getstats = new StatsDistribution();
    public StatsDistribution get_statstoAll { get { return getstats; } }
    private QuickLook look = new QuickLook();
    private bool quicklook_open = false;

    public Animator Selection;
    private string userInput = "GUEST";

    void Start()
    {
        thisLoader = this;
        LoadPlayerIdentify();
        LoadTransitToGameplaySettings();
    }

    #region SETUP
    private void LoadPlayerIdentify()
    {
        userInput = LoginPage_Script.thisPage.GetUserPortOutput();
    }

    private void LoadTransitToGameplaySettings()
    {
        getstats.load_Stats();
        Selection.SetTrigger("Opening" + PlayerPrefs.GetString("Resoultion_Melo", string.Empty));

        // Filling in of all track information to dash
        GameObject.Find("BG").GetComponent<RawImage>().texture = MeloMelo_Environment_Settings.GetBackgroundCover();

        if (PlayerPrefs.HasKey("Mission_Played"))
        {
            LoadingContentDescriptionCover(
                StoryMode_Scripts.thisStory.missionTrack.ArtistName,
                StoryMode_Scripts.thisStory.missionTrack.Title,
                StoryMode_Scripts.thisStory.missionTrack.Background_Cover
                );
        }
        else
        {
            LoadingContentDescriptionCover(
                SelectionMenu_Script.thisSelect.get_selection.get_form.ArtistName,
                SelectionMenu_Script.thisSelect.get_selection.get_form.Title,
                SelectionMenu_Script.thisSelect.get_selection.get_form.Background_Cover
                );
        }

        // Identify level difficulty and level value
        switch (PlayerPrefs.GetInt("DifficultyLevel_valve", 1))
        {
            case 1:
                if (SelectionMenu_Script.thisSelect.get_selection.get_normal >= 6) { GameObject.Find("DifficultyMeter").transform.GetChild(0).GetComponent<Text>().text = "NORMAL " + PlayerPrefs.GetString("Difficulty_Normal_selectionTxt", "?"); }
                else { GameObject.Find("DifficultyMeter").transform.GetChild(0).GetComponent<Text>().text = "NORMAL " + PlayerPrefs.GetString("Difficulty_Normal_selectionTxt", "?"); }
                GameObject.Find("DifficultyMeter").GetComponent<RawImage>().color = Color.blue;
                break;

            case 2:
                if (SelectionMenu_Script.thisSelect.get_selection.get_hard >= 16) { GameObject.Find("DifficultyMeter").transform.GetChild(0).GetComponent<Text>().text = "EXPERT " + PlayerPrefs.GetString("Difficulty_Hard_selectionTxt", "?"); }
                else { GameObject.Find("DifficultyMeter").transform.GetChild(0).GetComponent<Text>().text = "HARD " + PlayerPrefs.GetString("Difficulty_Hard_selectionTxt", "?"); }
                GameObject.Find("DifficultyMeter").GetComponent<RawImage>().color = (SelectionMenu_Script.thisSelect.get_selection.get_hard >= 16 ? new Color(1, 0.09f, 0.87f) : SelectionMenu_Script.thisSelect.get_selection.get_hard >= 11 ? new Color(0.47f, 0, 1) : Color.red);
                break;

            case 3:
                GameObject.Find("DifficultyMeter").transform.GetChild(0).GetComponent<Text>().text = "ULTIMATE " + PlayerPrefs.GetString("Difficulty_Ultimate_selectionTxt", "?");
                GameObject.Find("DifficultyMeter").GetComponent<RawImage>().color = new Color(1, 0.4f, 0);
                break;
        }

        // Get help for beginner starting as new player
        if (PlayerPrefs.GetString("Control_notice", "T") == "T") { quicklook_open = true; look.Opening_QuickLook("Selection_PlayControl"); }
        StartCoroutine(TransitToBattle());

        // Load up information from database
        foreach (Character_Base_Data character in getstats.slot_Stats)
        {
            if (character != null)
            {
                character.RefreshCharacterStats();
            }
        }

        // Load in ready for character status
        if (!PlayerPrefs.HasKey("MarathonPermit"))
        {
            PlayerPrefs.SetInt("Character_OverallHealth", getstats.get_UnitHealth("Character"));
            PlayerPrefs.SetInt("Character_OverallDamage", getstats.get_UnitDamage("Character", getstats.get_unitPierceDamage("Character")));
            PlayerPrefs.SetInt("Enemy_OverallHealth", getstats.get_UnitHealth("Enemy"));
            PlayerPrefs.SetInt("Enemy_OverallDamage", getstats.get_UnitDamage("Enemy", getstats.get_unitPierceDamage("Enemy")));

            PlayerPrefs.SetInt("Character_MagicDefense", (int)(getstats.get_UnitSpellResist("Character") * 0.01f * (100 - getstats.get_unitBurstDamage("Enemy"))));
            PlayerPrefs.SetInt("Enemy_MagicDefense", (int)(getstats.get_UnitSpellResist("Enemy") * 0.01f * (100 - getstats.get_unitBurstDamage("Character"))));
            PlayerPrefs.SetInt("PartySize_Index", getstats.get_unitSize());

            PlayerPrefs.SetFloat("Extra_Stats_1", getstats.get_criticalRate("Character"));
            PlayerPrefs.SetFloat("Extra_Stats_2", getstats.get_unitPierceDamage("Character"));
            PlayerPrefs.SetFloat("Extra_Stats_3", getstats.get_unitBurstDamage("Character"));

            // Get additional status
            StatsDistribution allChar = new StatsDistribution();
            allChar.load_Stats();

            if (allChar.slot_Stats != null)
            {
                foreach (Character_Base_Data character in allChar.slot_Stats)
                {
                    if (character != null)
                    {
                        StatsManage_Database charInfo = new StatsManage_Database(character.className);
                        int totalPoint = (character.level + (character.additionalProfile != null ? character.additionalProfile.rebirth_count * charInfo.GetCharacterMaxLevel() : 0)) * 2;

                        if (character.additionalProfile != null)
                        {
                            PlayerPrefs.SetFloat(character.className + "_MaxExpObtain",
                            (float)MeloMelo_CharacterInfo_Settings.GetCharacterMaxExperienceObtain(
                                totalPoint, character.additionalProfile.dosageMasteryPoint,
                                character.additionalProfile.masteryPoint)
                            );

                            //Debug.Log("Profile: " + (character.additionalProfile != null ? "OK!" : "Error!"));

                            //if (character.additionalProfile != null)
                            //{
                            //    Debug.Log("Total Point:" + character.additionalProfile.masteryPoint + " / " + totalPoint);
                            //    Debug.Log("Dosage Point:" + character.additionalProfile.dosageMasteryPoint);
                            //}
                        }
                        else
                            PlayerPrefs.SetFloat(character.className + "_MaxExpObtain", 100);
                    }
                }
            }
        }
        else
        {
            PlayerPrefs.SetInt("Character_OverallHealth", 1);
            PlayerPrefs.DeleteKey("Character_OverallDamage");
            PlayerPrefs.SetInt("Enemy_OverallHealth", 1);
            PlayerPrefs.DeleteKey("Enemy_OverallDamage");
        }
    }
    #endregion

    public void CloseFunction_ControlPlay() { quicklook_open = false; look.CloseFunction_NoticePlay("Control_notice"); }

    IEnumerator TransitToBattle()
    {
        yield return new WaitUntil(() => !quicklook_open);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Battleground Stage" + PlayerPrefs.GetString("Resoultion_Melo", string.Empty));
    }

    #region COMPONENT 
    private void LoadingContentDescriptionCover(string artist, string title, Texture coverImage)
    {
        GameObject.Find("Artist").GetComponent<Text>().text = "[ " + artist + " ]";
        GameObject.Find("Title").GetComponent<Text>().text = title;
        GameObject.Find("CoverImage").GetComponent<RawImage>().texture = coverImage;
        GameObject.Find("Designer").GetComponent<Text>().text = "Played as " + userInput;
    }
    #endregion
}
