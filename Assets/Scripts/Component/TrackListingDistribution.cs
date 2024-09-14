using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class TrackEventEntry
{
    public string title;
    public string cover;
    public int difficulty;
    public string level;
    public float score;
    public int point;
}

public class TrackListingDistribution : MonoBehaviour
{
    public static TrackListingDistribution thisList;

    private List<TrackEventEntry> list = new List<TrackEventEntry>();
    private List<TrackEventEntry> temp = new List<TrackEventEntry>();
    [SerializeField] private RawImage[] ChartOptions;
    [SerializeField] private GameObject contentBoard;
    [SerializeField] private GameObject emptyList;
    [SerializeField] private Text listNo;

    private string[] ChartNameArray =
    {
        MeloMelo_GameSettings.GetLocalFileChartLegacy,
        MeloMelo_GameSettings.GetLocalFileChartOld,
        MeloMelo_GameSettings.GetLocalFileChartNew
    };

    void Start()
    {
        thisList = this;
        GetListTrack(1);
    }

    #region SETUP
    private string GetDifficulty(int index)
    {
        switch (index)
        {
            case 1:
                return "[Normal]";

            case 2:
                return "[Hard]";

            default:
                return "[Ultimate]";
        }
    }

    private void GetTrackEntryObject(TrackEventEntry entry)
    {
        GameObject entryObject = Instantiate(Resources.Load<GameObject>("Database_Server/TrackEntry"), contentBoard.transform);
        Texture coverImage = null;

        try { coverImage = Resources.Load<Texture>("Database_CoverImage/CoverImage_" + entry.cover); } catch { coverImage = null; }
        if (coverImage) entryObject.transform.GetChild(0).GetComponent<RawImage>().texture = coverImage;

        entryObject.transform.GetChild(1).GetComponent<Text>().text = entry.title + "\n" + GetDifficulty(entry.difficulty) + " Lv. " + entry.level;
        entryObject.transform.GetChild(2).GetComponent<Text>().text = "Score: " + entry.score;
        entryObject.transform.GetChild(3).GetComponent<Text>().text = entry.point.ToString();
    }
    #endregion

    #region MAIN
    public void GetListTrack(int index)
    {
        ClearChartList();
        StartCoroutine(GetTrackRouter(index));
    }

    public void GetRateContribution(int index, TrackEventEntry entry)
    {
        bool currentEntry = false;
        string text = string.Empty;

        list.Clear();
        GetChartUnSorted(index);

        foreach (TrackEventEntry e in list.ToArray())
        {
            if (CheckDuplicateEntry(entry, e))
            {
                currentEntry = true;

                if (entry.point > e.point || entry.score > e.score) { currentEntry = false; list.Remove(e); }
                else text += JsonUtility.ToJson(e) + "/t";
            }
            
            else
                text += JsonUtility.ToJson(e) + "/t";
        }

        if (!currentEntry) text += JsonUtility.ToJson(entry) + "/t";
        SaveJsonFile(index, text);
    }

    public void ClearCacheRate(int index, TrackEventEntry entry)
    {
        string currentList = string.Empty;

        for (int num = 0; num < ChartNameArray.Length; num++)
        {
            if (index - 1 != num)
            {
                list.Clear();
                currentList = string.Empty;
                GetChartUnSorted(num + 1);

                foreach (TrackEventEntry e in list.ToArray())
                {
                    if (e.title == entry.title && e.difficulty == entry.difficulty) list.Remove(e);
                    else currentList += JsonUtility.ToJson(e) + "/t";
                }

                SaveJsonFile(num + 1, currentList);
            }
        }
    }

    private bool CheckDuplicateEntry(TrackEventEntry entry, TrackEventEntry entryList)
    {
        return entry.title == entryList.title && entry.difficulty == entryList.difficulty;
    }

    public int CalcuateTotalRatePoint()
    {
        int total = 0;
        list.Clear();

        GetChartListing(1, 20);
        GetChartListing(2, 15);
        GetChartListing(3, 15);

        list.Sort((left, right) => left.point.CompareTo(right.point));
        list.Reverse();

        foreach (TrackEventEntry point in list.ToArray())
            total += point.point;

        return total;
    }
    #endregion

    #region COMPONENT
    private IEnumerator GetTrackRouter(int index)
    {
        yield return new WaitForSeconds(0.5f);
        GetChartListing(index, (index == 1 ? 20 : index == 2 ? 15 : 15));

        CreateChartList();
        GetChartOption(index);
        listNo.text = list.ToArray().Length + "/" + (index == 1 ? 20 : index == 2 ? 15 : 15);
    }

    private void ClearChartList()
    {
        list.Clear();
        temp.Clear();

        for (int i = 0; i < contentBoard.transform.childCount; i++)
            contentBoard.transform.GetChild(i).GetComponent<RatePointBoard>().RemoveEntry();
    }

    private void CreateChartList()
    {
        emptyList.SetActive(list.ToArray().Length == 0);
        list.Sort((left, right) => left.point.CompareTo(right.point));
        list.Reverse();

        foreach (TrackEventEntry entry in list.ToArray())
            GetTrackEntryObject(entry);
    }

    private void GetChartOption(int option)
    {
        for (int choice = 0; choice < ChartOptions.Length; choice++)
        {
            if (option == choice + 1)
                ChartOptions[choice].color = Color.green;
            else
                ChartOptions[choice].color = Color.white;
        }
    }
    #endregion

    #region CHART_GENERATOR
    private void GetChartListing(int index, int limit)
    {
        string[] encode_list = LoadJsonFile(index) != string.Empty ? LoadJsonFile(index).Split("/t") : null;
        temp.Clear();

        if (encode_list != null)
        {
            for (int i = 0; i < encode_list.Length; i++)
            {
                TrackEventEntry entry = null;
                try { entry = JsonUtility.FromJson<TrackEventEntry>(encode_list[i]); } catch { }

                if (entry != null) temp.Add(entry);
                else break;
            }

            temp.Sort((left, right) => left.point.CompareTo(right.point));
            temp.Reverse();

            for (int score = 0; score < temp.ToArray().Length; score++)
                if (score < limit) list.Add(temp[score]);
        }
    }

    private void GetChartUnSorted(int index)
    {
        string[] encode_list = LoadJsonFile(index) != string.Empty ? LoadJsonFile(index).Split("/t") : null;

        if (encode_list != null)
        {
            for (int i = 0; i < encode_list.Length; i++)
            {
                TrackEventEntry entry = null;
                try { entry = JsonUtility.FromJson<TrackEventEntry>(encode_list[i]); } catch { }

                if (entry != null) list.Add(entry);
                else break;
            }
        }
    }
    #endregion

    #region JSON
    private string LoadJsonFile(int index)
    {
        string directory = (Application.isEditor ? "Assets/" : "MeloMelo_Data/");
        string path = directory + "StreamingAssets/LocalData/MeloMelo_LocalSave_ChartList/" +
            GetDestinationFile(LoginPage_Script.thisPage.portNumber == (int)MeloMelo_GameSettings.LoginType.TempPass, index);

        if (File.Exists(path))
        {
            string text = string.Empty;
            StreamReader reader = new StreamReader(path);
            text = reader.ReadToEnd();
            reader.Close();

            return text;
        }

        return string.Empty;
    }

    private void SaveJsonFile(int index, string data)
    {
        if (LoginPage_Script.thisPage.portNumber == (int)MeloMelo_GameSettings.LoginType.GuestLogin)
        {
            string directory = (Application.isEditor ? "Assets/" : "MeloMelo_Data/");
            string path = directory + "StreamingAssets/LocalData/MeloMelo_LocalSave_ChartList/" + LoginPage_Script.thisPage.GetUserPortOutput() + "_" + ChartNameArray[index - 1] + ".json";

            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine(data);

            writer.Close();
        }
    }
    #endregion

    #region NETWORK
    private string GetDestinationFile(bool isNetworkOpen, int chart_ID)
    {
        if (isNetworkOpen) return "TempPass_ChartData_" + chart_ID + ".json";
        else return LoginPage_Script.thisPage.GetUserPortOutput() + "_" + ChartNameArray[chart_ID - 1] + ".json";
    }
    #endregion
}
