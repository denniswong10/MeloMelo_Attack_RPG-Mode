using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_GameProperties;
using System;

public struct ScoreBundleWithMultipler
{
    public int totalCount;
    public float latestMultipler;

    public float GetScore(float score)
    {
        //int convertToInt = (int)(score * latestMultipler);
        return totalCount * (score * latestMultipler);
    }
}

public abstract class ScoringIndicator_Base
{
    protected float actualScore;
    protected float estimatedScore;
    protected abstract void UpdateScore_Interface();

    #region MAIN
    public void GetIndicatorUpdate()
    {
        UpdateScore_Interface();
    }

    public float GetActualScore()
    {
        return actualScore;
    }

    public float GetExpectedScore()
    {
        return estimatedScore;
    }
    #endregion

    #region MISC
    public void ResetDisplay()
    {
        actualScore = 0;
        estimatedScore = 0;
    }
    #endregion
}

public class ScoringIndicator_MinScore : ScoringIndicator_Base
{
    private float score_perfect2;
    private float score_perfect;
    private float score_bad;

    public ScoringIndicator_MinScore()
    {
        score_perfect2 = 0;
        score_perfect = 0;
        score_bad = 0;
    }

    #region MAIN
    protected override void UpdateScore_Interface()
    {
        // Count up indivdual score and add up
        score_perfect2 = GameManager.thisManager.getJudgeWindow.get_perfect2 * BeatConductor.thisBeat.get_scorePerfect2;
        score_perfect = GameManager.thisManager.getJudgeWindow.get_perfect * BeatConductor.thisBeat.get_scorePerfect;
        score_bad = GameManager.thisManager.getJudgeWindow.get_bad * BeatConductor.thisBeat.get_scoreBad;

        actualScore = GameManager.thisManager.get_score1.get_score;
        estimatedScore = GameManager.thisManager.ScoreRefactoring() + (score_perfect2 + score_perfect + score_bad);
    }
    #endregion
}

public class ScoringIndicator_MaxScore : ScoringIndicator_Base
{
    private float currentMultipler;
    private List<ScoreBundleWithMultipler> allPieceOfScore;

    public ScoringIndicator_MaxScore()
    {
        currentMultipler = 1;
        estimatedScore = 0;
        allPieceOfScore = new List<ScoreBundleWithMultipler>();
    }

    #region MAIN
    protected override void UpdateScore_Interface()
    {
        float liveValueMultipler = GameManager.thisManager.CurrentValueMultipler();

        if (liveValueMultipler != currentMultipler)
            AddNewScoring(liveValueMultipler, currentMultipler, GameManager.thisManager.getJudgeWindow.TotalJudgeCounted());

        if (allPieceOfScore != null && allPieceOfScore.Count > 0)
            GetAdvanceCalculate();
        else
            GetBasicCalculate();
    }
    #endregion

    #region COMPONENT
    private void GetBasicCalculate()
    {
        // Count up all notes which has called
        int totalCount = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted();

        // Compare both the score which is expected to get by the end of play
        actualScore = GameManager.thisManager.get_score1.get_score;

        float expectedBeforeFactor = totalCount * BeatConductor.thisBeat.get_scorePerfect2;
        float expectedScore = expectedBeforeFactor + GameManager.thisManager.ScoreRefactoring();

        // Calculate expected score to estimatedScore to apply the rest of the check
        estimatedScore = expectedScore;
    }

    private void GetAdvanceCalculate()
    {
        // Count up all notes which has called
        int totalCount = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted();

        // Compare both the score which is expected to get by the end of play
        actualScore = GameManager.thisManager.get_score1.get_score;

        // Calculate all score with split multipler value
        float shardPieceScore = 0;
        int lastCountAfterMultipler = 0;

        foreach (ScoreBundleWithMultipler score in allPieceOfScore)
        {
            shardPieceScore += score.GetScore(BeatConductor.thisBeat.get_scorePerfect2);
            lastCountAfterMultipler += score.totalCount;
        }

        float balanceCountScore = (totalCount - lastCountAfterMultipler) * (int)(BeatConductor.thisBeat.get_scorePerfect2 * currentMultipler);
        float addFactorToCount = balanceCountScore + GameManager.thisManager.ScoreRefactoring();

        estimatedScore = shardPieceScore + addFactorToCount;
    }
    #endregion

    #region MISC
    private void AddNewScoring(float afterNewMultipler, float beforeNewMultipler, int lastCount)
    {
        currentMultipler = afterNewMultipler;

        if (allPieceOfScore != null)
        {
            ScoreBundleWithMultipler scoreForReplacement = new ScoreBundleWithMultipler();
            int splitValueOnCount = lastCount;

            if (allPieceOfScore.Count > 0)
            {
                foreach (ScoreBundleWithMultipler value in allPieceOfScore)
                    splitValueOnCount -= value.totalCount;
            }

            scoreForReplacement.totalCount = splitValueOnCount;
            scoreForReplacement.latestMultipler = beforeNewMultipler;
            allPieceOfScore.Add(scoreForReplacement);
        }
    }
    #endregion
}

public class MeloMelo_ScoreSystem : MonoBehaviour
{
    public static MeloMelo_ScoreSystem thisSystem;

    // Combo Penatly: GameProperties
    private GameSystem_Score score3;
    private ScoringIndicator_Base specialIndicator_minScore;
    private ScoringIndicator_Base specialIndicator_maxScore;

    private int maxPoint = 0;
    private int maxHiPoint = 0;

    private float hiScore = 0;
    private float estimatedScore = 0;
    private float boundaryScoreCheck = 0;
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

        switch (PlayerPrefs.GetInt("ScoreDisplay2"))
        {
            case 5:
                specialIndicator_maxScore = new ScoringIndicator_MaxScore();
                specialIndicator_maxScore.ResetDisplay();
                MaxScoreDisplay();
                break;

            case 6:
                specialIndicator_minScore = new ScoringIndicator_MinScore();
                specialIndicator_minScore.ResetDisplay();
                MinScoreDisplay();
                break;
        }
       
        SetHiScore(PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_score" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0));
        SetMaxPoint(PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_point" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0));

        // Get update score for the first time
        UpdatePointDisplay();
        UpdateScoreDisplay();
    }

    #region MAIN
    // CP: Function
    public void CheckingForStatus()
    {
        switch (PlayerPrefs.GetInt("AutoRetreat", 0))
        {
            case 1:
                if (IsComboPenatlyVisible())
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
        if (pointTxt != null) pointTxt.text = GameManager.thisManager.get_point.get_score + " / " + maxPoint;
        if (PlayerPrefs.GetInt("ScoreDisplay2") == 1)
        {
            GameObject.FindGameObjectWithTag("SecondScoreDisplay").transform.GetChild(0).GetComponent<Text>().text = "CURRENT POINTS";
            GameObject.FindGameObjectWithTag("SecondScoreDisplay").GetComponent<Text>().text = GameManager.thisManager.get_point.get_score + " / " + maxPoint;
        }
    }

    private void UpdateMaxPoint()
    {
        if (PlayerPrefs.GetInt("ScoreDisplay2") == 3)
        {
            int expected = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted() * 3;
            estimatedPoint = maxPoint - ((int)GameManager.thisManager.get_point.get_score - expected);

            // Display Score
            GameObject.FindGameObjectWithTag("SecondScoreDisplay").transform.GetChild(0).GetComponent<Text>().text = "MAXIMUM POINTS";
            GameObject.FindGameObjectWithTag("SecondScoreDisplay").GetComponent<Text>().text = Binder(estimatedPoint);
        }
    }

    private void UpdateHiPoints()
    {
        if (PlayerPrefs.GetInt("ScoreDisplay2") == 7)
        {
            int min = (int)GameManager.thisManager.get_point.get_score;
            int max = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted() * 3;
            estimatedPoint = (min > 0 ? 3 : 0) + (min - max);

            // Display Score
            GameObject.FindGameObjectWithTag("SecondScoreDisplay").transform.GetChild(0).GetComponent<Text>().text = "POINTS ( + / - )";
            GameObject.FindGameObjectWithTag("SecondScoreDisplay").GetComponent<Text>().text = Binder(estimatedPoint);
            GameObject.FindGameObjectWithTag("SecondScoreDisplay").GetComponent<Text>().color = ColorBasic(estimatedPoint);
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
        ReceivedComboPenatly();

        // Score With Rank
        RankCalculateDisplay(GameManager.thisManager.get_score1.get_score);

        // Score On ( + / - )
        ScoreLostDisplay();
    }

    public void ReceivedComboPenatly()
    {
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

            float minScore = GameManager.thisManager.get_score1.get_score;

            estimatedScore = hiScore - (minScore - maxScore) - hiScore;

            // Display Score
            GameObject.FindGameObjectWithTag("SecondScoreDisplay").transform.GetChild(0).GetComponent<Text>().text = "BEST SCORE";
            GameObject.FindGameObjectWithTag("SecondScoreDisplay").GetComponent<Text>().text = Binder(-estimatedScore);
            GameObject.FindGameObjectWithTag("SecondScoreDisplay").GetComponent<Text>().color = ColorBasic((int)-estimatedScore);
        }
    }
    #endregion

    #region MAIN [SCORE DISPLAY (MIN/MAX)]
    private void MinScoreDisplay()
    {
        //if (PlayerPrefs.GetInt("ScoreDisplay2") == 6)
        //{
        //    // Count up indivdual score and add up
        //    float score_perfect2 = GameManager.thisManager.getJudgeWindow.get_perfect2 * BeatConductor.thisBeat.get_scorePerfect2;
        //    float score_perfect = GameManager.thisManager.getJudgeWindow.get_perfect * BeatConductor.thisBeat.get_scorePerfect;
        //    float score_bad = GameManager.thisManager.getJudgeWindow.get_bad * BeatConductor.thisBeat.get_scoreBad;

        //    // Calculate expected score to estimatedScore to apply the rest of the check
        //    string additional_info = FilterOffZero((int)(GameManager.thisManager.get_score1.get_maxScore - GameManager.thisManager.get_score1.get_score),
        //        (int)(GameManager.thisManager.get_score1.get_score - GameManager.thisManager.get_score1.get_maxScore));

        //    // Calculate min score which include factoring the score
        //    estimatedScore = GameManager.thisManager.ScoreRefactoring() + (score_perfect2 + score_perfect + score_bad);

        //    // Display score
        //    SecondaryScoreDisplay("MINIMUM SCORE", ColorDetails((int)estimatedScore), additional_info);
        //}

        if (specialIndicator_minScore != null)
        {
            specialIndicator_minScore.GetIndicatorUpdate();
            estimatedScore = specialIndicator_minScore.GetExpectedScore();

            // Calculate expected score to estimatedScore to apply the rest of the check
            float referenceValue = GameManager.thisManager.get_score1.get_maxScore - (GameManager.thisManager.get_score1.get_maxScore + GameManager.thisManager.ScoreRefactoring());
            float finalValue = specialIndicator_minScore.GetActualScore() - (GameManager.thisManager.get_score1.get_maxScore + GameManager.thisManager.ScoreRefactoring());
            string additional_info = FilterOffZero((int)referenceValue, (int)finalValue);

            // Display score
            SecondaryScoreDisplay("MINIMUM SCORE", ColorDetails((int)estimatedScore), additional_info);
        }
    }

    private void MaxScoreDisplay()
    {
        if (PlayerPrefs.GetInt("AutoRetreat") > 1 && PlayerPrefs.GetInt("AutoRetreat") < 6)
        {
            // Count up all notes which has called
            int totalCount = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted();

            // Compare both the score which is expected to get by the end of play
            float actualScore = GameManager.thisManager.get_score1.get_score;

            float expectedBeforeFactor = totalCount * BeatConductor.thisBeat.get_scorePerfect2;
            float expectedScore = expectedBeforeFactor + GameManager.thisManager.ScoreRefactoring();

            // Calculate expected score to estimatedScore to apply the rest of the check
            boundaryScoreCheck = GameManager.thisManager.get_score1.get_maxScore + (actualScore - expectedScore);

            //string additional_info = FilterOffZero((int)(GameManager.thisManager.get_score1.get_maxScore - estimatedScore), (int)(actualScore - expectedScore));

            //// Display Score
            //SecondaryScoreDisplay("MAXIMUM SCORE", ColorDetails((int)estimatedScore), additional_info);

            //// Check for border score
            //CheckingForStatus();
        }

        if (specialIndicator_maxScore != null)
        {
            specialIndicator_maxScore.GetIndicatorUpdate();
            estimatedScore = GameManager.thisManager.get_score1.get_maxScore + (specialIndicator_maxScore.GetActualScore() - specialIndicator_maxScore.GetExpectedScore());

            float referenceValue = estimatedScore;
            float finalValue = specialIndicator_maxScore.GetActualScore() - specialIndicator_maxScore.GetExpectedScore();
            string additional_info = FilterOffZero((int)referenceValue, (int)finalValue);

            SecondaryScoreDisplay("MAXIMUM SCORE", ColorDetails((int)estimatedScore), additional_info);
        }
    }
    #endregion

    #region MAIN [SCORE CP]
    private void ComboPenatlyOnDisplay2()
    {
        if (PlayerPrefs.GetInt("ScoreDisplay2") == 2)
        {
            // Display Score 
            SecondaryScoreDisplay("COMBO PENALTY", ColorBasic(Math.Round(GameManager.thisManager.CurrentValueMultipler(), 2) >= 1 ? 1 : -1), 
                string.Empty);

            GameObject.FindGameObjectWithTag("SecondScoreDisplay").GetComponent<Text>().text = "x" +
                Math.Round(GameManager.thisManager.CurrentValueMultipler(), 2);
        }
    }

    private bool IsComboPenatlyVisible()
    {
        return Mathf.Ceil(GameManager.thisManager.CurrentValueMultipler()) < 1;
    }
    #endregion

    #region MAIN [SCORE DISPLAY 1]
    private void ScoreLostDisplay()
    {
        if (hiScoreTxt != null)
        {
            float maxScore = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted() * BeatConductor.thisBeat.get_scorePerfect2 +
                GameManager.thisManager.ScoreRefactoring();

            float minScore = GameManager.thisManager.get_score1.get_score;

            estimatedScore = minScore - maxScore;

            hiScoreTxt.text = Binder(estimatedScore);
            hiScoreTxt.color = ColorBasic((int)estimatedScore);
        }
    }

    private void RankCalculateDisplay(float score)
    {
        if (RankTxt != null)
        {
            string currentRank = MeloMelo_GameSettings.GetScoreRankStructure(score.ToString()).rank;
            Color colorBorder = MeloMelo_GameSettings.GetScoreRankStructure(score.ToString()).colorBorder;

            RankTxt.text = currentRank;
            RankTxt.color = colorBorder;
        }
    }

    private void ComboPenatlyOnDisplay()
    {
        if (scoreTxt != null)
        {
            scoreTxt.color = ColorBasic(GameManager.thisManager.CurrentValueMultipler() >= 1 ? 1 : -1);
            scoreTxt.text = "x" + Math.Round(GameManager.thisManager.CurrentValueMultipler(), 2);
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
        return MeloMelo_GameSettings.GetScoreRankStructure(score.ToString()).colorBorder;
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
        if (GameManager.thisManager.getJudgeWindow.TotalJudgeCounted() > 0)
        {
            if (boundaryScoreCheck > score)
                return true;
            else if (boundaryScoreCheck < score)
                return false;
        }

        return true;
    }
    #endregion
    #endregion
}
