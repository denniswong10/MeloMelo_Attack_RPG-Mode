using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShortPanel_Icon_Script : MonoBehaviour
{
    [SerializeField] private GameObject Icon;
    [SerializeField] private GameObject panelTarget;

    #region MAIN
    public void OpenSceneChoice(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void SpawnPanel()
    {
        GameObject main = GameObject.Find("GameInterface");
        if (main) Instantiate(panelTarget, main.transform);
    }

    public void ToggleIconDescription(bool active)
    {
        Icon.SetActive(active);
    }

    public void ToggleObjectShown(bool active)
    {
        panelTarget.SetActive(active);
    }
    #endregion
}
