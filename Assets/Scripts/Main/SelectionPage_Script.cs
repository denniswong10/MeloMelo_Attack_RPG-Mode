using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionPage_Script : MonoBehaviour
{
    private GameObject[] BGM;
    [SerializeField] private GameObject[] Selection;
    private int currentSelection = 0;

    // Start is called before the first frame update
    void Start()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }

        OpeningSelection(Selection[currentSelection], string.Empty);
    }

    #region SETUP
    private void BattleDifficultSelection()
    {
        currentSelection = 2;
        OpeningSelection(Selection[currentSelection], string.Empty);
    }

    private void UpdateMainSectionMenu()
    {
        Selection[0].transform.GetChild(5).GetComponent<Button>().interactable = Application.isEditor;
        Selection[0].transform.GetChild(6).GetComponent<Button>().interactable = Application.isEditor;
    }
    #endregion

    #region MAIN
    public void BackToPreviousPage()
    {
        if (Selection[currentSelection].activeInHierarchy) { SceneManager.LoadScene("Menu"); }
        else { SceneManager.LoadScene("Ref_PreSelection"); }
    }

    public void GetToggleSelection(int index)
    {
        string destinationZone = string.Empty;

        switch (index)
        {
            case 0:
                destinationZone = "StoryModeTransition";
                break;

            case 1:
                destinationZone = "BattleModeTransition";
                break;

            case 2:
                destinationZone = "CollectionTransition";
                break;

            case 3:
                destinationZone = "DungeonTransition";
                break;

            default:
                break;
        }

        ClosingSelection(Selection[currentSelection], destinationZone);
    }

    public void GetPlayModeSelection(string destinationZone)
    {
        ClosingSelection(Selection[currentSelection], destinationZone);
    }

    public void GetPlayDifficultSelection(int index)
    {
        PlayerPrefs.SetInt("BattleDifficulty_Mode", index);
        ClosingSelection(Selection[currentSelection], "SeasonTransition");
    }
    #endregion

    #region COMPONENT (TRANSITION)
    private void OpeningSelection(GameObject target, string LoadAfterTransition)
    {
        target.GetComponent<Animator>().SetTrigger("Opening");
        if (LoadAfterTransition != string.Empty) Invoke(LoadAfterTransition, 1);
        if (currentSelection == 0) UpdateMainSectionMenu();
    }

    private void ClosingSelection(GameObject target, string LoadAfterTransition)
    {
        target.GetComponent<Animator>().SetTrigger("Closing");
        if (LoadAfterTransition != string.Empty) Invoke(LoadAfterTransition, 1);
    }
    #endregion

    #region MISC
    private void StoryModeTransition()
    {
        SceneManager.LoadScene("StoryMode");
    }

    private void BattleModeTransition()
    {
        currentSelection = 1;
        OpeningSelection(Selection[currentSelection], string.Empty);
    }

    private void CollectionTransition()
    {
        SceneManager.LoadScene("Collections");
    }

    private void DungeonTransition()
    {
        SceneManager.LoadScene("DungeonSelection");
    }

    private void SeasonTransition()
    {
        SceneManager.LoadScene("SeasonSelection");
    }

    private void MarathonSelection()
    {
        SceneManager.LoadScene("MarathonSelection");
    }
    #endregion
}
