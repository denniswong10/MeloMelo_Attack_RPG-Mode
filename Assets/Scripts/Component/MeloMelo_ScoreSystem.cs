using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_GameProperties;

public class MeloMelo_ScoreSystem : MonoBehaviour
{
    public static MeloMelo_ScoreSystem thisSystem;

    // Combo Penatly: GameProperties
    private GameSystem_Score score3;

    private int maxPoint = 0;
    private int maxHiPoint = 0;

    private float hiScore = 0;
    private float estimatedScore = 0;
    private int estimatedPoint = 0;

    private Text RankTxt;
    private Text hiScoreTxt;
    private Text scoreTxt = null;
    private Text pointTxt = null;

    // Start is called before the first frame update
    void Start()
    {
        thisSystem = this;
        Invoke("Setup", 0.5f);
    }

    void Setup()
    {
        score3 = new GameSystem_Score();
        try { scoreTxt = GameObject.FindGameObjectWithTag("CP_System").GetComponent<Text>(); }
        catch { scoreTxt = null; }

        try { pointTxt = GameObject.FindGameObjectWithTag("PointSystem").GetComponent<Text>(); }
        catch { pointTxt = null; }

        try { hiScoreTxt = GameObject.FindGameObjectWithTag("HiScore_System").GetComponent<Text>(); }
        catch { hiScoreTxt = null; }

        try { RankTxt = GameObject.FindGameObjectWithTag("RankID_System").GetComponent<Text>(); }
        catch { RankTxt = null; }

        SetHiScore(PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_score" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0));
        SetMaxPoint(PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_point" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0));
    }

    #region MAIN
    // CP: Function
    private void CheckingForStatus()
    {
        switch (PlayerPrefs.GetInt("AutoRetreat", 0))
        {
            case 1:
                if (score3.get_score != 0)
                    GameManager.thisManager.RetreatTrigger();
                break;

            case 2:
                if (!BestScoreCondition(950000))
                    GameManager.thisManager.RetreatTrigger();
                break;

            case 3:
                if (!BestScoreCondition(980000))
                    GameManager.thisManager.RetreatTrigger();
                break;

            case 4:
                if (!BestScoreCondition(1000000))
                    GameManager.thisManager.RetreatTrigger();
                break;

            case 5:
                if (!BestScoreCondition(hiScore))
                    GameManager.thisManager.RetreatTrigger();
                break;

            default:
                break;
        }
    }

    // Point: Function
    #region SETUP [POINT]
    private void SetMaxPoint(int value)
    {
        maxPoint = (int)GameManager.thisManager.get_point.get_maxScore;
        maxHiPoint = value;
    }

    public void UpdatePointDisplay()
    {
        // Update from GameManager (Point)
        UpdatePoint();
        UpdateMaxPoint();
        UpdateHiPoints();
    }
    #endregion

    #region MAIN [POINT DISPLAY (MIN/MAX)]
    private void UpdatePoint()
    {
        if (pointTxt != null) pointTxt.text = GameManager.thisManager.get_point + "/" + maxPoint;
        if (PlayerPrefs.GetInt("ScoreDisplay2") == 1)
        {
            GameObject.FindGameObjectWithTag("SecondScoreDisplay").transform.GetChild(0).GetComponent<Text>().text = "CURRENT POINTS";
            GameObject.FindGameObjectWithTag("SecondScoreDisplay").GetComponent<Text>().text = GameManager.thisManager.get_point + "/" + maxPoint;
        }
    }

    private void UpdateMaxPoint()
    {
        if (PlayerPrefs.GetInt("ScoreDisplay2") == 3)
        {
            int expected = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted() * 3;
            estimatedPoint = maxPoint - ((int)GameManager.thisManager.get_point.get_score - expected);

            // Display Score
            SecondaryScoreDisplay("MAXIMUN POINT", ColorBasic(estimatedPoint), Binder(estimatedPoint));
        }
    }

    private void UpdateHiPoints()
    {
        if (PlayerPrefs.GetInt("ScoreDisplay2") == 7)
        {
            int min = (int)GameManager.thisManager.get_point.get_score;
            int max = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted() * 3;
            estimatedPoint = maxHiPoint - (min - max);

            // Display Score
            SecondaryScoreDisplay("HI_POINTS", ColorBasic(estimatedPoint), Binder(estimatedPoint));
        }
    }
    #endregion

    // Score: Function
    #region SETUP [SCORE]
    private void SetHiScore(float value)
    {
        hiScore = value;
    }

    public void UpdateScoreDisplay()
    {
        // Update from GameManager (Score)
        ScoreDisplay2_HiScore();
        MinScoreDisplay();
        MaxScoreDisplay();

        // Score With Rank
        RankCalculateDisplay(GameManager.thisManager.get_score1.get_score);

        // Hi-Score
        HiScoreDisplay();
    }

    public void ReceivedComboPenatly(float _receive)
    {
        // Add combo pentaly
        score3.ModifyScore((int)_receive);

        // Display panel
        ComboPenatlyOnDisplay();
        ComboPenatlyOnDisplay2();
    }
    #endregion

    #region MAIN [HI-SCORE]
    private void ScoreDisplay2_HiScore()
    {
        if (PlayerPrefs.GetInt("ScoreDisplay2") == 4)
        {
            float maxScore = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted() * BeatConductor.thisBeat.get_scorePerfect2 +
                GameManager.thisManager.ScoreRefactoring();

            float minScore = GameManager.thisManager.get_score1.get_score + GameManager.thisManager.ScoreRefactoring();

            estimatedScore = hiScore - (maxScore - minScore) - hiScore;

            // Display Score
            SecondaryScoreDisplay("HI-SCORE", ColorBasic((int)estimatedScore), Binder(estimatedScore) + " / " + hiScore);
        }
    }
    #endregion

    #region MAIN [SCORE DISPLAY (MIN/MAX)]
    private void MinScoreDisplay()
    {
        if (PlayerPrefs.GetInt("ScoreDisplay2") == 6)
        {
            // Count up indivdual score and add up
            float score_perfect2 = GameManager.thisManager.getJudgeWindow.get_perfect2 * BeatConductor.thisBeat.get_scorePerfect2;
            float score_perfect = GameManager.thisManager.getJudgeWindow.get_perfect * BeatConductor.thisBeat.get_scorePerfect;
            float score_bad = GameManager.thisManager.getJudgeWindow.get_bad * BeatConductor.thisBeat.get_scoreBad;

            // Calculate expected score to estimatedScore to apply the rest of the check
            string additional_info = FilterOffZero((int)(GameManager.thisManager.get_score1.get_maxScore - GameManager.thisManager.get_score1.get_score),
                (int)(GameManager.thisManager.get_score1.get_score - GameManager.thisManager.get_score1.get_maxScore));

            // Calculate min score which include factoring the score
            estimatedScore = GameManager.thisManager.ScoreRefactoring() + (score_perfect2 + score_perfect + score_bad);

            // Display score
            SecondaryScoreDisplay("MINIMUM SCORE", ColorDetails((int)estimatedScore), additional_info);
        }
    }

    private void MaxScoreDisplay()
    {
        if (PlayerPrefs.GetInt("ScoreDisplay2") == 5)
        {
            // Count up all notes which has called
            int totalCount = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted();

            // Compare both the score which is expected to get by the end of play
            float actualScore = GameManager.thisManager.get_score1.get_score;

            float expectedBeforeFactor = totalCount * BeatConductor.thisBeat.get_scorePerfect2;
            float expectedScore = expectedBeforeFactor + GameManager.thisManager.ScoreRefactoring();

            // Calculate expected score to estimatedScore to apply the rest of the check
            estimatedScore = GameManager.thisManager.get_score1.get_maxScore + (actualScore - expectedScore);

            string additional_info = FilterOffZero((int)(GameManager.thisManager.get_score1.get_maxScore - estimatedScore), (int)(actualScore - expectedScore));

            // Display Score
            SecondaryScoreDisplay("MAXIMUM SCORE", ColorDetails((int)estimatedScore), additional_info);

            // Check for border score
            CheckingForStatus();
        }
    }
    #endregion

    #region MAIN [SCORE CP]
    private void ComboPenatlyOnDisplay2()
    {
        if (PlayerPrefs.GetInt("ScoreDisplay2") == 2)
        {
            // Display Score 
            SecondaryScoreDisplay("COMBO PENALTY", ColorBasic((int)-score3.get_score), score3.get_score.ToString());
        }
    }
    #endregion

    #region MAIN [SCORE DISPLAY 1]
    private void HiScoreDisplay()
    {
        if (hiScoreTxt != null)
        {
            float maxScore = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted() * BeatConductor.thisBeat.get_scorePerfect2 +
                GameManager.thisManager.ScoreRefactoring();

            float minScore = GameManager.thisManager.get_score1.get_score + GameManager.thisManager.ScoreRefactoring();

            estimatedScore = hiScore - (maxScore - minScore) - hiScore;

            hiScoreTxt.text = estimatedScore.ToString();
            hiScoreTxt.color = ColorBasic((int)-estimatedScore);
        }
    }

    private void RankCalculateDisplay(float score)
    {
        if (RankTxt != null)
        {
            string currentRank = MeloMelo_GameSettings.GetScoreRankStructure((int)score).rank;
            Color colorBorder = MeloMelo_GameSettings.GetScoreRankStructure((int)score).colorBorder;

            RankTxt.text = currentRank;
            RankTxt.color = colorBorder;
        }
    }

    private void ComboPenatlyOnDisplay()
    {
        if (scoreTxt != null)
        {
            scoreTxt.color = ColorBasic((int)-score3.get_score);
            scoreTxt.text = score3.get_score.ToString();
        }
    }
    #endregion

    // SecondaryTools: Function
    #region COMPONENT [SCORE DISPLAY (SECONDARY TOOL)]
    private void SecondaryScoreDisplay(string title, Color color, string info)
    {
        GameObject.FindGameObjectWithTag("SecondScoreDisplay").transform.GetChild(0).GetComponent<Text>().text = title;
        GameObject.FindGameObjectWithTag("SecondScoreDisplay").GetComponent<Text>().color = color;
        GameObject.FindGameObjectWithTag("SecondScoreDisplay").GetComponent<Text>().text = estimatedScore.ToString("000000") + info;
    }

    private string FilterOffZero(int referenceValue, int finalValue = 0)
    {
        if (referenceValue == 0) return string.Empty;
        else return " / " + (finalValue == 0 ? referenceValue : finalValue);
    }

    private Color ColorBasic(int score)
    {
        if (score < 0) return Color.red;
        else if (score > 0) return Color.green;
        else return Color.yellow;
    }

    private Color ColorDetails(int score)
    {
        return MeloMelo_GameSettings.GetScoreRankStructure(score).colorBorder;
    }

    private string Binder(float reference)
    {
        if (reference >= 0) return "+" + reference;
        else return reference.ToString();
    }
    #endregion

    #region COMPONENT [SCORE CONDITIONAL CHECKING]
    private bool BestScoreCondition(float score)
    {
        if (estimatedScore > score)
            return true;
        else if (estimatedScore < score)
            return false;

        return true;
    }
    #endregion
    #endregion
}
