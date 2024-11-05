using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStore_Scripts : MonoBehaviour
{
    private GameObject[] BGM;
    [SerializeField] private GameObject Selection;

    [SerializeField] private GameObject ItemSelectionList;
    private GameObject ItemContentGerenator;
    [SerializeField] private GameObject ItemEmptyText;

    // Start is called before the first frame update
    void Start()
    {
        BGM_Loader();
        Selection.GetComponent<Animator>().SetTrigger("Opening");
        StoreSetup();
    }

    private void BGM_Loader()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }

    #region SETUP
    private void StoreSetup()
    {
        ItemContentGerenator = ItemSelectionList.transform.GetChild(0).gameObject;
    }
    #endregion

    #region MAIN
    public void ReturnToMain()
    {
        SceneManager.LoadScene("Ref_PreSelection");
    }
    #endregion
}
