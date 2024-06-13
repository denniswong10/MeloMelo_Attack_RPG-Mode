using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MeloMelo_VirtualItem;
using UnityEngine.UI;

public class StoragePage_Script : MonoBehaviour
{
    public GameObject Selection;
    public GameObject LoadingBar;

    private InventoryManagement storeBag;

    private void Setup()
    {
        Selection.GetComponent<Animator>().SetTrigger("Opening");
    }

    private void LeaveStoragePage()
    {
        Selection.GetComponent<Animator>().SetTrigger("Closing");
        if (storeBag.Get_UnsaveItem) { StartCoroutine(SavingData_Storage()); }
        Invoke("LeaveStoragePage_2", 2);
    }

    void LeaveStoragePage_2() { SceneManager.LoadScene("Collection"); }

    void Start()
    {
        storeBag = new InventoryManagement();
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) LeaveStoragePage();
    }

    public void DismantleItem()
    {
        storeBag.RemoveItemToLocal(PlayerPrefs.GetInt("SlotSelected_Item", 0));
    }

    private IEnumerator SavingData_Storage()
    {
        LoadingBar.SetActive(true);
        LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "CHECKING DATA...";
        yield return new WaitForSeconds(3);

        LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "COMPLETED!";
        storeBag.ClearCache_ItemDatabase();
    }
}
