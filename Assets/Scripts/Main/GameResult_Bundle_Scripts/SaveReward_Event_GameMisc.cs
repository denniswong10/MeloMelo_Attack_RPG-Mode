using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_Local;

namespace MeloMelo_EventBuilder
{
    public abstract class InGameEventManagement
    {
        public string get_event_prompted_message { get; private set; }

        protected abstract void ReceiveOfEventInfo();
        protected abstract void RetrieveEventInfo();

        #region MAIN
        public void GetUpdateOfEvent()
        {

        }
        #endregion
    }

    public class PlayEventManagement
    {

    }

    public class PlayMarathonAward : InGameEventManagement
    {
        private const int minRewardingValue = 1;
        private const int rewardingMultipler = 2;

        #region COMPONENT
        protected override void RetrieveEventInfo()
        {
            LocalSave_DataManagement data = new LocalSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
                "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

            //ItemData item = Resources.Load<ItemData>("Database_Item/#12");
            //int maxAmount = IsTrackListCleared() ? 1 : 0;
            //maxAmount += IsTrackListCleared() && PlayerPrefs.GetInt("Marathon_Quest_Result", 0) == 1 ? 1 : 0;

            //if (item && maxAmount > 0)
            //{
            //    mainScript.PromptMessage.SetActive(true);
            //    mainScript.PromptMessage.transform.GetChild(0).GetComponent<Text>().text = "Item Obtained: " +
            //        item.itemName + " ( x" + maxAmount + " )";

            //    data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);
            //    data.SaveVirtualItemFromPlayer(item.itemName, maxAmount, true);
            //}

            //yield return new WaitForSeconds(1);
            //mainScript.PromptMessage.SetActive(false);
            //postProcessing.CompletedProcess();
        }

        protected override void ReceiveOfEventInfo()
        {

        }
        #endregion

        #region MISC
        private bool IsTrackListCleared()
        {
            bool isCleared = false;
            int clearedOfLength = PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty) != "CustomList" ?
                Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty)).trackList.Length :
                    MeloMelo_ExtensionContent_Settings.LoadMarathonDetail(PlayerPrefs.GetInt("MarathonInstanceNumber", 0)).track_difficulty.Length;

            for (int track = 0; track < clearedOfLength; track++)
            {
                if (PlayerPrefs.HasKey("TrackListRecord_Score" + track)) isCleared = true;
                else isCleared = false;
            }

            return isCleared;
        }
        #endregion 
    }
}
