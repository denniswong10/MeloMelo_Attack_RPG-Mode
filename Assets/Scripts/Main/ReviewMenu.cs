using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MeloMelo_Network;

public class ReviewMenu : MonoBehaviour
{
    public InputField User;
    public InputField Comment;
    public Button submitBtn;
    private ServerData data;

    void Start()
    {
        data = new ServerData(LoginPage_Script.thisPage.GetUserPortOutput(), PlayerPrefs.GetString("GameWeb_URL"));
        UpdateUserInput();
    }

    public void BackButton() { SceneManager.LoadScene("Options"); }

    public void SubmitButton()
    {
        GameObject.Find("Icon").transform.GetChild(1).GetComponent<Text>().text = "CONNECTING...";

        GameObject.Find("Icon").GetComponent<Animator>().SetTrigger("Open");
        StartCoroutine(data.WebReviewProgram(Comment.text));
        StartCoroutine(PendingSubmission());
    }

    IEnumerator PendingSubmission()
    {
        yield return new WaitForSeconds(2);
        if (PlayerPrefs.GetString("ReviewCompleted", "F") == "T") { GameObject.Find("Icon").transform.GetChild(1).GetComponent<Text>().text = "COMPLETED!"; }
        else { GameObject.Find("Icon").transform.GetChild(1).GetComponent<Text>().text = "SERVER ERROR!"; }

        yield return new WaitForSeconds(1);
        GameObject.Find("Icon").GetComponent<Animator>().SetTrigger("Close");
    }

    public void ReviewWrite()
    {
        if (Comment.text != string.Empty) { submitBtn.interactable = true; }
        else { submitBtn.interactable = false; }
    }

    void UpdateUserInput()
    {
        User.transform.GetChild(0).GetComponent<Text>().text = LoginPage_Script.thisPage.GetUserPortOutput();
    }
}
