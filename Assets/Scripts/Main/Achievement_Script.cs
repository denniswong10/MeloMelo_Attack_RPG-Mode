using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Achievement_Script : MonoBehaviour
{
    public GameObject achievement_List;
    public GameObject Achievement_Template;
    public GameObject[] Tier_GroupContext;
    private int currentTierIndex = -1;

    private List<BundleSeasonChecker> buildchecks;
    private BundleSeasonChecker restrictCheck = null;
    public GameObject LoadingScreen;
    private bool[] isFinishedCount = new bool[2];

    public Slider progressBar;
    public Text progressText;
    public Text TrackCounter;

    void Start()
    {
        buildchecks = new List<BundleSeasonChecker>();
        for (int check = 0; check < isFinishedCount.Length; check++) isFinishedCount[check] = false;

        CreateAchievementAssets();
        StartCoroutine(CreateSeasonCompletetionBuild());
        StartCoroutine(CreateRestrictedCompletionBuild());

        Invoke("UpdateAllAchievement", 0.5f);
    }

    void Update()
    {
        LoadingScreen.SetActive(!isFinishedCount[0] || !isFinishedCount[1]);
        if (LoadingScreen.activeInHierarchy) GetLoadingCounter();
    }

    #region SETUP
    private void CreateAchievementAssets()
    {
        foreach (AchievementTab achieve in CollectAchievementInfo())
        {
            FindNextTier(achieve.Tier);

            GameObject board = Instantiate(Achievement_Template, achievement_List.transform);
            board.name = achieve.name;
            board.transform.GetChild(0).GetComponent<Text>().text = achieve.title;
            board.transform.GetChild(1).GetComponent<Text>().text = achieve.description + FindRewardTitle(achieve.titleReward_name);
        }
    }

    private void AchievementMarker(string target, bool isComplete, bool isLocked = false)
    {
        for (int instance = 0; instance < achievement_List.transform.childCount; instance++)
        {
            if (target == achievement_List.transform.GetChild(instance).name)
            {
                if (isLocked) achievement_List.transform.GetChild(instance).GetChild(4).gameObject.SetActive(true);
                else if (isComplete) achievement_List.transform.GetChild(instance).GetChild(2).gameObject.SetActive(true);
                else achievement_List.transform.GetChild(instance).GetChild(3).gameObject.SetActive(true);
                break;
            }
        }

        if (isComplete) UpdateProgressBar();
    }

    private AchievementTab[] CollectAchievementInfo()
    {
        return Resources.LoadAll<AchievementTab>("Database_Achievement");
    }
    #endregion

    #region SETUP (Extension Track Search)
    private IEnumerator CreateSeasonCompletetionBuild()
    {
        for (int season = 0; season < StartMenu_Script.thisMenu.get_seasonNum + 1; season++)
        {
            BundleSeasonChecker checker = new BundleSeasonChecker();
            checker.RegisterArea(season);
            checker.GetTotalTracks();
            StartCoroutine(checker.StartSearchComplete(false));

            // ???
            yield return new WaitUntil(() => checker.get_isCompleted);
            buildchecks.Add(checker);
        }

        isFinishedCount[0] = true;
    }

    private IEnumerator CreateRestrictedCompletionBuild()
    {
        yield return new WaitUntil(() => isFinishedCount[0]);
        restrictCheck = new BundleSeasonChecker();

        for (int season = 0; season < StartMenu_Script.thisMenu.get_seasonNum + 1; season++)
        {
            restrictCheck.RegisterArea(season);
            restrictCheck.GetTotalTracks();
            StartCoroutine(restrictCheck.StartSearchComplete(true));

            // ???
            yield return new WaitUntil(() => restrictCheck.get_isCompleted);
        }

        isFinishedCount[1] = true;
        Invoke("UpdateTrackCounter", 0.5f);
    }
    #endregion

    #region MAIN
    public void ReturnToMain()
    {
        SceneManager.LoadScene("Menu");
    }

    private void UpdateAllAchievement()
    {
        foreach (AchievementTab condition in CollectAchievementInfo())
        {
            if (!IsTierRestricted(condition.Tier))
            {
                switch (condition.useConditionReward)
                {
                    case AchievementTab.Reward_Condition.RatePoint:
                        if (condition.GetConditionData(ConditionState.Condition_Filler.MainScore) != null)
                            AchievementMarker(condition.name, PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + 
                                "totalRatePoint", 0) >= int.Parse(condition.GetConditionData(ConditionState.Condition_Filler.MainScore).value));
                        break;

                    case AchievementTab.Reward_Condition.PlayedCount:
                        if (condition.GetConditionData(ConditionState.Condition_Filler.MainScore) != null)
                            AchievementMarker(condition.name, PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + 
                                "PlayedCount_Data", 0) >= int.Parse(condition.GetConditionData(ConditionState.Condition_Filler.MainScore).value));
                        break;

                    case AchievementTab.Reward_Condition.SeasonCompletion:
                        if (condition.GetConditionData(ConditionState.Condition_Filler.MainScore) != null)
                            StartCoroutine(SearchOrCountSeasonCompletion(condition.name, 
                                int.Parse(condition.GetConditionData(ConditionState.Condition_Filler.MainScore).value)));
                        break;

                    case AchievementTab.Reward_Condition.RankAchiever:
                    case AchievementTab.Reward_Condition.StatusAchiever:
                    case AchievementTab.Reward_Condition.PointAchiever:
                    case AchievementTab.Reward_Condition.BattleWonAchiever:
                        if (condition.GetConditionData(ConditionState.Condition_Filler.MainScore) != null &&
                            condition.GetConditionData(ConditionState.Condition_Filler.NumberOfTimes) != null)
                            StartCoroutine(MassCountingAchieverCompletion(condition.useConditionReward, condition.name,
                                int.Parse(condition.GetConditionData(ConditionState.Condition_Filler.NumberOfTimes).value),
                                condition.GetConditionData(ConditionState.Condition_Filler.MainScore).value));
                            break;

                    default:
                        if (condition.conditionOperate != string.Empty) Invoke(condition.conditionOperate, 0.1f);
                        break;
                }
            }
            else
                AchievementMarker(condition.name, false, true);
        }
    }
    #endregion

    #region COMPONENT (Tier Condition)
    private void FindNextTier(AchievementTab.Title_Tier tier)
    {
        if ((int)tier != currentTierIndex)
        {
            Instantiate(Tier_GroupContext[(int)tier], achievement_List.transform);
            currentTierIndex++;
        }
    }

    private string FindRewardTitle(string title_name)
    {
        if (title_name != string.Empty) return "\n Reward: " + title_name + " (Title)";
        else return string.Empty;
    }

    private bool IsTierRestricted(AchievementTab.Title_Tier tier)
    {
        if (tier == AchievementTab.Title_Tier.Rhondonite) return !IsTierRegularCompleted();
        else return false;
    }

    private bool IsTierRegularCompleted()
    {
        int totalCount = 0;
        int currentCount = 0;

        foreach (AchievementTab achievement in CollectAchievementInfo())
            if (achievement.Tier != AchievementTab.Title_Tier.Rhondonite)
                totalCount++;

        foreach (AchievementTab achievement in CollectAchievementInfo())
        {
            for (int billBoard = 0; billBoard < achievement_List.transform.childCount; billBoard++)
            {
                if (achievement.name == achievement_List.transform.GetChild(billBoard).name &&
                    achievement_List.transform.GetChild(billBoard).GetChild(2).gameObject.activeInHierarchy)
                    currentCount++;
            }
        }

        return currentCount == totalCount;
    }
    #endregion

    #region COMPONENT (Extension Track Search)
    private IEnumerator SearchOrCountSeasonCompletion(string target, int season)
    {
        yield return new WaitUntil(() => isFinishedCount[0]);
        AchievementMarker(target, buildchecks[season].GetTotalCompletion(false));
    }

    private IEnumerator MassCountingAchieverCompletion(AchievementTab.Reward_Condition condition, string target, int playedAmount, string result)
    {
        yield return new WaitUntil(() => isFinishedCount[0]);
        int count = 0;

        foreach (BundleSeasonChecker check in buildchecks)
        {
            switch (condition)
            {
                case AchievementTab.Reward_Condition.RankAchiever:
                    count += check.GetNumberOfTotalScoreAchiever(true, result);
                    break;

                case AchievementTab.Reward_Condition.StatusAchiever:
                    count += check.GetNumberOfTotalStatusAchiever(true, result);
                    break;

                case AchievementTab.Reward_Condition.PointAchiever:
                    count += check.GetNumberOfTotalPointAchiever(result);
                    break;

                case AchievementTab.Reward_Condition.BattleWonAchiever:
                    count += check.GetNumberOfSuccessAchiever(result);
                    break;
            }
        }

        AchievementMarker(target, count >= playedAmount);
    }
    #endregion

    #region COMPONENT
    private void UpdateProgressBar()
    {
        progressBar.maxValue = GetTotalActiveAchievement();
        progressBar.value++;
        progressText.text = progressBar.value + "/" + progressBar.maxValue + " [ " + (int)(100 / progressBar.maxValue * progressBar.value) + "% ]";
    }
     
    private int GetTotalActiveAchievement()
    {
        int totalCount = 0;

        for (int achievement = 0; achievement < achievement_List.transform.childCount; achievement++)
        {
            if (achievement_List.transform.GetChild(achievement).GetChild(3).gameObject.activeInHierarchy)
                totalCount++;
        }

        return totalCount;
    }

    private void UpdateTrackCounter()
    {
        int totalCountLoaded = 0;
        int totalCountActive = 0;

        foreach (BundleSeasonChecker checker in buildchecks)
        {
            totalCountLoaded += checker.GetLoadedTrackCount(true) + checker.GetLoadedTrackCount(false);
            totalCountActive += checker.GetLoadedTrackCount(true);
        }

        TrackCounter.text = totalCountLoaded + " Track Loaded / " + totalCountActive + " Track Active";
    }

    private void GetLoadingCounter()
    {
        int currentLoadCount = 0;

        foreach (BundleSeasonChecker checks in buildchecks)
            currentLoadCount += checks.get_loadedTracks;

        TrackCounter.text = currentLoadCount + " loaded tracks...";
    }
    #endregion

    #region MISC (Start Of Play)
    private void StartingOut()
    {
        // Custom Setup: Get login port user isn't guest
        AchievementMarker("AW1", LoginPage_Script.thisPage.GetUserPortOutput() != "GUEST");
    }

    private void MakeTransfer()
    {
        // Custom Setup: Get confirmation about transfer of data is been perform
        AchievementMarker("AW2", PlayerPrefs.HasKey("ReviewTransfer"));
    }

    private void Configuration()
    {
        // Custom Setup: Get confirmation about option is visited
        AchievementMarker("AW3", PlayerPrefs.HasKey("ReviewOption"));
    }
    #endregion

    #region MISC (Middle Of Play)
    private void BattleSetupReview()
    {
        // Custom Setup: Check on the following file that is currently been created
        LocalFileChecker battleSetupCheck = new LocalFileChecker
            (
                "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress", 
                "savelog_GameplaySettings"
            );

        AchievementMarker("AW4", battleSetupCheck.GetSetupByExistingUser(LoginPage_Script.thisPage.GetUserPortOutput()));
    }

    private void PlayRestrictedTrack()
    {
        // Custom Setup: Get all restricted tracks is been played out
        StartCoroutine(AwaitForRestrictCount());
    }

    private IEnumerator AwaitForRestrictCount()
    {
        yield return new WaitUntil(() => isFinishedCount[1]);
        AchievementMarker("E_Extra_A1", restrictCheck.GetTotalCompletion(true) && restrictCheck.GetTotalScoreAchiever(true, "SS"));
    }
    #endregion

    #region MISC (End Of Play)
    private void AllPlayedForSS()
    {
        // Custom Setup: Get rank SS to all played tracks with difficulty in every season
        StartCoroutine(AwaitForRankAchieverCount("SS"));
    }

    private IEnumerator AwaitForRankAchieverCount(string rank)
    {
        yield return new WaitUntil(() => isFinishedCount[0]);
        int count = 0;

        foreach (BundleSeasonChecker list in buildchecks)
        {
            if (list.GetTotalScoreAchiever(false, rank))
                count++;
        }

        AchievementMarker("E_Extra_A2", count == buildchecks.ToArray().Length);
    }

    private void JourneyStartline()
    {
        // Custom Setup: Check on the following file that is currently been created
        LocalFileChecker areaVisited = new LocalFileChecker
            (
            "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress", 
            "savelog_SelectionBase"
            );

        AchievementMarker("AW5", areaVisited.GetSetupByExistingUser(LoginPage_Script.thisPage.GetUserPortOutput()));
    }
    #endregion
}

public class LocalFileChecker
{
    private string filePath;
    private string fileName;

    public LocalFileChecker(string path, string fileName, string format = ".txt")
    {
        string platformFormat = Application.isEditor ? "Assets/" : "MeloMelo_Data/";
        filePath = platformFormat + path;
        this.fileName = fileName + format;
    }

    #region MAIN 
    public bool GetSetupExisted()
    {
        return File.Exists(filePath + "/" + fileName);
    }

    public bool GetSetupByExistingUser(string user)
    {
        string confirmPath = filePath + "/" + user + "_" + fileName;
        return File.Exists(confirmPath);
    }
    #endregion
}

public class BundleSeasonChecker
{
    private List<AreaInfo> AreaList;
    private List<MusicScore> TrackList;

    private int totalTracks;
    private bool isTrackRegistered;
    private int loadedTracks;

    // Get confirmation about tracks is done registering the area
    public bool get_isCompleted { get { return isTrackRegistered; } }
    // Get confirmation with the amount of tracks been loaded
    public int get_loadedTracks { get { return loadedTracks; } }

    public BundleSeasonChecker()
    {
        AreaList = new List<AreaInfo>();
        TrackList = new List<MusicScore>();

        totalTracks = 0;
        loadedTracks = 0;
        isTrackRegistered = false;
    }

    #region SETUP
    // Start register area from resource
    public void RegisterArea(int season)
    {
        foreach (AreaInfo info in Resources.LoadAll<AreaInfo>("Database_Area/Season" + season))
            AreaList.Add(info);
    }

    // Start counting up all tracks on the registered area
    public void GetTotalTracks()
    {
        foreach (AreaInfo info in AreaList)
            totalTracks += info.totalMusic;
    }
    #endregion

    #region MAIN
    // Scan the tracks of all existing area have been registered
    public IEnumerator StartSearchComplete(bool onlyRestricted)
    {
        yield return new WaitForSeconds(0.5f);

        foreach (AreaInfo areaInfo in AreaList)
        {
            for (int track = 0; track < areaInfo.totalMusic; track++)
            {
                ResourceRequest startLoading = Resources.LoadAsync<MusicScore>("Database_Area/" + areaInfo.AreaName + "/M" + (track + 1));
                yield return new WaitUntil(() => startLoading.isDone);

                MusicScore track_detail = startLoading.asset as MusicScore;
                if (track_detail != null) if (!onlyRestricted || (onlyRestricted && track_detail.SetRestriction)) TrackList.Add(track_detail);
                loadedTracks++;
            }
        }

        isTrackRegistered = true;
    }
    #endregion

    #region COMPONENT
    private bool IsTrackAchieveCondition(AchievementTab.Reward_Condition achieverType, string track, int difficulty, string result)
    {
        // Find any of this achiever which are available
        switch (achieverType)
        {
            case AchievementTab.Reward_Condition.RankAchiever:
                return PlayerPrefs.HasKey(track + "_score" + difficulty) && PlayerPrefs.GetInt(track + "_score" + difficulty) >= MeloMelo_GameSettings.GetScoreRankStructure(result).score;

            case AchievementTab.Reward_Condition.StatusAchiever:
                return PlayerPrefs.HasKey(track + "_BattleRemark_" + difficulty) && PlayerPrefs.GetInt(track + "_BattleRemark_" + difficulty) <= int.Parse(result);

            case AchievementTab.Reward_Condition.PlayedCount:
                return PlayerPrefs.HasKey(track + "_score" + difficulty) && PlayerPrefs.GetInt(track + "_score" + difficulty) > 0;

            case AchievementTab.Reward_Condition.BattleWonAchiever:
                return PlayerPrefs.HasKey(track + "_SuccessBattle_" + difficulty + 1) && PlayerPrefs.GetString(track + "_SuccessBattle_" + difficulty + 1) == result;
        
            case AchievementTab.Reward_Condition.PointAchiever:
                if (PlayerPrefs.HasKey(track + "_point" + difficulty) && PlayerPrefs.HasKey(track + "_maxPoint" + difficulty))
                {
                    float percentage = 100f / PlayerPrefs.GetInt(track + "_maxPoint" + difficulty);
                    float currentAchiever = percentage * PlayerPrefs.GetInt(track + "_point" + difficulty);
                    return Mathf.Floor(currentAchiever) >= int.Parse(result);
                }
                else
                    return false;
        }

        // Get final result not achieved when there is none
        return false;
    }

    private int GetNumberOfAchievedTrack(AchievementTab.Reward_Condition condition, bool playedTrack, string result = "")
    {
        // Count up all types of achieved tracks
        int currentAchievedCount = 0;

        foreach (MusicScore track in TrackList)
        {
            // Count any difficulty type which are played
            if (playedTrack)
                if (IsTrackAchieveCondition(condition, track.Title, 1, result) || IsTrackAchieveCondition(condition, track.Title, 2, result) ||
                    (track.UltimateAddons && IsTrackAchieveCondition(condition, track.Title, 3, result)))
                    currentAchievedCount++;

            // Otherwise all difficulty track must be played
            else 
                if (IsTrackAchieveCondition(condition, track.Title, 1, result) && IsTrackAchieveCondition(condition, track.Title, 2, result) &&
                    (!track.UltimateAddons || (track.UltimateAddons && IsTrackAchieveCondition(condition, track.Title, 3, result))))
                    currentAchievedCount++;
        }

        // Result all achieved tracks
        return currentAchievedCount;
    }

    private int GetNumberOfActiveTrack(bool active)
    {
        // Count up tracks which are active
        int currentActiveCount = 0;

        foreach (MusicScore track in TrackList)
        {
            // Get counting of active or non-active tracks
            if ((active && track.ScoreObject != null) || (!active && track.ScoreObject == null))
                currentActiveCount++;
        }

        // Result tracks count
        return currentActiveCount;
    }
    #endregion

    #region MISC
    // Get total number of played track equal to the overall track for this season
    public bool GetTotalCompletion(bool includeDifficulty)
    {
        return GetNumberOfAchievedTrack(AchievementTab.Reward_Condition.PlayedCount, includeDifficulty) == totalTracks;
    }

    // Get total number of available or loaded
    public int GetLoadedTrackCount(bool availableTrack)
    {
        return GetNumberOfActiveTrack(availableTrack);
    }

    // Get total number which are achieved equal to the overall track for this season
    public bool GetTotalScoreAchiever(bool playedTrack, string score)
    {
        return GetNumberOfAchievedTrack(AchievementTab.Reward_Condition.RankAchiever, playedTrack, score) == totalTracks;
    }

    // Get total number of achieved score
    public int GetNumberOfTotalScoreAchiever(bool playedTrack, string score)
    {
        return GetNumberOfAchievedTrack(AchievementTab.Reward_Condition.RankAchiever, playedTrack, score);
    }

    // Get total number of status achieved
    public int GetNumberOfTotalStatusAchiever(bool playedTrack, string status)
    {
        return GetNumberOfAchievedTrack(AchievementTab.Reward_Condition.StatusAchiever, playedTrack, status);
    }

    // Get total number of achieved point
    public int GetNumberOfTotalPointAchiever(string result)
    {
        return GetNumberOfAchievedTrack(AchievementTab.Reward_Condition.PointAchiever, true, result);
    }

    // Get total number of successful battle
    public int GetNumberOfSuccessAchiever(string result)
    {
        return GetNumberOfAchievedTrack(AchievementTab.Reward_Condition.BattleWonAchiever, true, result);
    }
    #endregion
}
