using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarathonTask_Script : MonoBehaviour
{
    [HideInInspector] public bool taskHasChosen;
    private string actionDescription;

    public enum TaskSelector { MainOption, ChallengeSelection, ConfirmationStart, Entry }
    public TaskSelector choiceOfAction;

    #region SETUP
    private void PerformAction(bool allowToPerform, string actionName)
    {
        taskHasChosen = true;

        switch (choiceOfAction)
        {
            case TaskSelector.Entry:
                CreateEntryGateWay(actionName, allowToPerform);
                break;

            case TaskSelector.MainOption:
            case TaskSelector.ConfirmationStart:
                actionDescription = actionName;
                break;

            case TaskSelector.ChallengeSelection:
                GameObject selectionList = GameObject.FindGameObjectWithTag("Challenge_Storage");
                for (int currentFind = 0; currentFind < selectionList.transform.childCount; currentFind++)
                {
                    if (allowToPerform && selectionList.transform.GetChild(currentFind).GetComponent<ChallengeTask_Scripts>().is_Picked)
                        actionDescription = selectionList.transform.GetChild(currentFind).GetComponent<ChallengeTask_Scripts>().name;
                        
                    else
                        selectionList.transform.GetChild(currentFind).GetComponent<ChallengeTask_Scripts>().SelectChallenge(false);
                }
                break;
        }
    }
    #endregion

    #region MAIN
    public void AllowActionTaken(string actionDescription)
    {
        PerformAction(true, actionDescription);
    }

    public void DisagreeToAction(string actionDescription)
    {     
        PerformAction(false, actionDescription);
    }
    #endregion

    #region COMPONENT
    private void CreateEntryGateWay(string key, bool processToCreate)
    {
        if (processToCreate) PlayerPrefs.SetInt(key, 1);
        else PlayerPrefs.DeleteKey(key);
    }
    #endregion

    #region MISC
    public string GetSelectionTitle(string reference, bool onlyKey)
    {
        switch (choiceOfAction)
        {
            case TaskSelector.MainOption:
                return actionDescription + (onlyKey ? string.Empty : " List");

            case TaskSelector.ConfirmationStart:
            case TaskSelector.ChallengeSelection:
                return actionDescription;

            default:
                return reference;
        }
    }
    #endregion
}
