using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonTemplateData : MonoBehaviour
{
    private int assigned_index;
    private bool season_entryRestriction;

    #region MAIN
    public void AssignNewIndex(int index)
    {
        assigned_index = index;
    }

    public void SetEntryRestriction(bool restrict)
    {
        season_entryRestriction = restrict;
    }

    public void ContinueIndexBrowsing()
    {
        SeasonSelection.thisSelection.ContinueTemplateSelection(season_entryRestriction, assigned_index);
    }
    #endregion
}
