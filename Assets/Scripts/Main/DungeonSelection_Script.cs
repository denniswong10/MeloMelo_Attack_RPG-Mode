using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
struct DugneonSelection_NagivatorMenu
{
    public GameObject[] menuTemplate;
}

public class DungeonSelection_Script : MonoBehaviour
{
    private GameObject[] BGM;
    [SerializeField] private GameObject selection;
    private DugneonSelection_NagivatorMenu currentSelection;

    [Header("Selection Settings: Configure")]
    [SerializeField] private DugneonSelection_NagivatorMenu mainSelection;
    [SerializeField] private DugneonSelection_NagivatorMenu battleSelection;
    [SerializeField] private DugneonSelection_NagivatorMenu exchangeSelection;

    // Start is called before the first frame update
    void Start()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }

        OpeningMain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region SETUP
    private void OpeningMain()
    {
        selection.GetComponent<Animator>().SetTrigger("Opening");
    }
    #endregion

    #region MAIN
    public void GetBattleMode_Nagivate()
    {
        ToggleBetweenVisible(mainSelection, false);
        ToggleBetweenVisible(battleSelection, true);
    }

    public void GetExchange_Nagivate()
    {
        ToggleBetweenVisible(mainSelection, false);
        ToggleBetweenVisible(exchangeSelection, true);
    }

    public void GetPrevious_Nagivate()
    {
        ToggleBetweenVisible(currentSelection, false);
        ToggleBetweenVisible(mainSelection, true);
    }

    public void ReturnToMainSelection()
    {
        SceneManager.LoadScene("Ref_PreSelection");
    }
    #endregion

    #region COMPONENT
    private Slider PointGuageBar()
    {
        return GameObject.Find("ScrollBar").GetComponent<Slider>();
    }

    private void ToggleBetweenVisible(DugneonSelection_NagivatorMenu target, bool active)
    {
        for (int visible = 0; visible < target.menuTemplate.Length; visible++)
            target.menuTemplate[visible].SetActive(active);

        if (active) currentSelection = target;
    }
    #endregion
}
