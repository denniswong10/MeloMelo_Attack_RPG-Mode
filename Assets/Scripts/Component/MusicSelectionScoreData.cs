using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelectionScoreData : MonoBehaviour
{
    private const float overallPercentage = 10;
    private List<int> data = new List<int>();

    private int meterCounter_complextity = 0; // Complextity = (overallPercent / (AssignedNote + NotAssignedNote)) * counter (Measure of Creative Note Design)
    private int meterCounter_Difficult = 0; // Difficult = (overallPercent / overallCombo) * counter (Measure of threat on enemys and traps)
    private int meterCounter_speed = 0; // Speed = (overallPercent / (AssignedNote + NotAssignedNote)) * counter (Measure of Assigned Note without break design)

    #region SETUP
    private float FormatDetail()
    {
        return overallPercentage / data.ToArray().Length;
    }

    private int FindAllEmptyNote()
    {
        int total = 0;

        foreach (int note in data)
            if (note == 0) total++;

        return total;
    }

    private int FindAllAssignedNote()
    {
        int total = 0;

        foreach (int note in data)
            if (note != 0) total++;

        return total;
    }

    private float CalculateMeterSelection(int total, int overall)
    {
        return (overallPercentage / overall) * total;
    }
    #endregion

    #region MAIN
    public void SetupSelectionData(int[] scoreData)
    {
        data.AddRange(scoreData);
    }

    public string GetBattleDetailWithData()
    {
        string description = string.Empty;

        description += "Speed: " + CalculateMeterSelection(meterCounter_speed, FindAllAssignedNote() + FindAllEmptyNote());
        description += "Complextity: " + CalculateMeterSelection(meterCounter_complextity, FindAllAssignedNote() + FindAllEmptyNote());
        description += "Difficult: " + CalculateMeterSelection(meterCounter_Difficult, 0);

        return description;
    }
    #endregion
}
