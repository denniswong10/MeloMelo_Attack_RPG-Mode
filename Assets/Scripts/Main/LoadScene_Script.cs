using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("Resoultion_Melo", string.Empty) != string.Empty && SceneManager.GetActiveScene().name != "LoadScene2") { SceneManager.LoadScene("LoadScene2"); }
        Invoke("TransitionTime", 2);
    }

    void TransitionTime() { SceneManager.LoadScene("StartMenu"); }
}
