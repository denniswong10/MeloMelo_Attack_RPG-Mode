using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

namespace Tests
{
    // Unit Test: Check all GameObject are present

    // Unit: Result
    public class _11_Result
    {
        [Test]
        public void BGM_Output()
        {
            GameObject[] BGM = GameObject.FindGameObjectsWithTag("BGM");
            Assert.AreEqual(0, BGM.Length);
        }

        [Test]
        public void ResultInfo()
        {
            bool checklist()
            {
                GameObject[] obj =
                {
                    GameObject.Find("CoverImage"),
                    GameObject.Find("HeadTitle"),
                    GameObject.Find("Artist"),
                    GameObject.Find("Title"),
                    GameObject.Find("Perfect"),
                    GameObject.Find("Bad"),
                    GameObject.Find("Miss"),
                    GameObject.Find("MaxCombo"),
                    GameObject.Find("Rank"),
                    GameObject.Find("DifficultyMeter"),
                    GameObject.Find("Score")
                };

                foreach (GameObject i in obj) { if (i == null) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Found!");
        }

        [Test]
        public void TransitionBtn_Output()
        {
            bool checklist()
            {
                GameObject[] obj =
                {
                    GameObject.Find("ViewBattleResult"),
                    GameObject.Find("CharacterStatus"),
                    GameObject.Find("NextBtn")
                };

                foreach (GameObject i in obj) { if (i == null || !i.GetComponent<Button>().interactable) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Found!");
        }
    }

    // Unit: CharacterSelection
    public class _9_StageTransition
    {
        [Test]
        public void BGM_Output()
        {
            GameObject[] BGM = GameObject.FindGameObjectsWithTag("BGM");
            Assert.AreEqual(0, BGM.Length);
        }

        [Test]
        public void MusicInfo()
        {
            bool checklist()
            {
                GameObject[] obj =
                {
                    GameObject.Find("Artist"),
                    GameObject.Find("Title"),
                    GameObject.Find("CoverBorder"),
                    GameObject.Find("HeadText"),
                    GameObject.Find("Designer"),
                    GameObject.Find("DifficultyMeter")
                };

                foreach (GameObject i in obj) { if (i == null) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Found!");
        }
    }

    // Unit: CharacterSelection
    public class _8_CharacterSelection
    {
        [Test]
        public void BGM_Output()
        {
            GameObject[] BGM = GameObject.FindGameObjectsWithTag("BGM");
            Assert.AreEqual(0, BGM.Length);
        }

        [Test]
        public void CharacterSlot_Output()
        {
            Texture[] BG = Resources.LoadAll<Texture>("Character_Data");
            GameObject[] BGM = GameObject.FindGameObjectsWithTag("Slot");
            Assert.AreEqual(BG.Length, BGM.Length - 1);
        }
    }

    // Unit: AreaSelection
    public class _7_BattleSetUp
    {
        [Test]
        public void BGM_Output()
        {
            GameObject[] BGM = GameObject.FindGameObjectsWithTag("BGM");
            Assert.AreEqual(0, BGM.Length);
        }

        [Test]
        public void Process_And_Reserve_Input()
        {
            bool checklist()
            {
                GameObject[] obj =
                {
                    GameObject.Find("StartBtn"),
                    GameObject.Find("BackBtn")
                };

                foreach (GameObject i in obj) { if (i == null || !i.GetComponent<Button>().interactable) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Found!");
        }

        [Test]
        public void UnitFormation_Output()
        {
            bool checklist()
            {
                GameObject[] obj =
                {
                    GameObject.Find("Slot1"),
                    GameObject.Find("Slot2"),
                    GameObject.Find("Slot3")
                };

                GameObject[] obj2 = 
                {
                    GameObject.Find("UNIT STATUS"),
                    GameObject.Find("Title")
                };

                foreach (GameObject i in obj) { if (i == null || !i.GetComponent<Button>().interactable) { return false; } }
                foreach (GameObject i in obj2) { if (i == null) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Found!");
        }

        [Test]
        public void GameplaySetting_Output()
        {
            bool checklist()
            {
                GameObject[] obj =
                {
                    GameObject.Find("Title2"),
                    GameObject.Find("NoteSpeed_Option"),
                    GameObject.Find("MV_Option")
                };

                foreach (GameObject i in obj) { if (i == null) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Found!");
        }
    }

    // Unit: AreaSelection
    public class _6_MusicSelection
    {
        [Test]
        public void BGM_Output()
        {
            GameObject[] BGM = GameObject.FindGameObjectsWithTag("BGM");
            Assert.AreEqual(1, BGM.Length);
        }

        [Test]
        public void NagivationBtn_Input()
        {
            bool checklist()
            {
                GameObject[] obj =
                {
                    GameObject.Find("LeftNav"),
                    GameObject.Find("RightNav")
                };

                foreach (GameObject i in obj) { if (i == null || !i.GetComponent<Button>().interactable) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Found!");
        }

        [Test]
        public void MusicSelection_Information()
        {
            bool checklist()
            {
                GameObject[] obj =
                {
                    GameObject.Find("MusicInformation_Txt"),
                    GameObject.Find("ScrollBar"),
                    GameObject.Find("Selection Btn")
                };

                foreach (GameObject i in obj) { if (i == null || !i.GetComponent<Button>().interactable) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Found!");
        }

        [Test]
        public void Achievement_Information()
        {
            bool checklist()
            {
                GameObject[] obj =
                {
                    GameObject.Find("AchievementStats"),
                    GameObject.Find("AchievementStats2"),
                    GameObject.Find("AS_Remark")
                };

                foreach (GameObject i in obj) { if (i == null) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Found!");
        }
    }

    // Unit: AreaSelection
    public class _5_AreaSelection
    {
        [Test]
        public void BGM_Output()
        {
            GameObject[] BGM = GameObject.FindGameObjectsWithTag("BGM");
            Assert.AreEqual(1, BGM.Length);
        }

        [Test]
        public void NagivationBtn_Input()
        {
            bool checklist()
            {
                GameObject[] obj =
                {
                    GameObject.Find("LeftNav"),
                    GameObject.Find("RightNav")
                };

                foreach(GameObject i in obj) { if (i == null || !i.GetComponent<Button>().interactable) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Found!");
        }

        [Test]
        public void AreaSelectionDisplay_Component()
        {
            bool checklist()
            {
                GameObject[] obj =
                {
                    GameObject.Find("AreaSelection"),
                    GameObject.Find("Border_AS"),
                    GameObject.Find("AchievementBoard"),
                    GameObject.Find("Text_AS")
                };

                foreach (GameObject i in obj) { if (i == null) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Found!");
        }

        [Test]
        public void AreaSelectionInfo_Component()
        {
            bool checklist()
            {
                GameObject[] obj =
                {
                    GameObject.Find("AreaTitle"),
                    GameObject.Find("FileInfo"),
                    GameObject.Find("Text_AT"),
                    GameObject.Find("Text_FI")
                };

                foreach (GameObject i in obj) { if (i == null) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Found!");
        }
    }

    // Unit: Options
    public class _3_Options
    {

    }

    // Unit: PreSelection
    public class _4_PreSelection
    {
        [Test]
        public void BattleModeBtn_Input()
        {
            Assert.IsTrue(GameObject.Find("Icon4").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void ShopBtn_Input()
        {
            Assert.IsTrue(GameObject.Find("Icon3").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void TutorialBtn_Input()
        {
            Assert.IsTrue(GameObject.Find("Icon2").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void StoryModeBtn_Input()
        {
            Assert.IsTrue(GameObject.Find("Icon1").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void BGM_Output()
        {
            GameObject[] BGM = GameObject.FindGameObjectsWithTag("BGM");
            Assert.AreEqual(1, BGM.Length);
        }
    }

    // Unit: Menu
    public class _2_Menu
    {
        [Test]
        public void StartBtn_Input()
        {
            Assert.IsTrue(GameObject.Find("Start").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void OptionBtn_Input()
        {
            Assert.IsTrue(GameObject.Find("Options").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void QuitBtn_Input()
        {
            Assert.IsTrue(GameObject.Find("Quit").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void Profile_Display()
        {
            bool checklist()
            {
                GameObject[] Main =
                {
                    GameObject.Find("Profile"),
                    GameObject.Find("NamePlate"),
                    GameObject.Find("TitleBorder"),
                    GameObject.Find("Count"),
                    GameObject.Find("ProfileBorder")
                };

                foreach (GameObject i in Main) { if (!i.activeInHierarchy) { return false; } }
                return true;
            }

            Assert.IsTrue(checklist(), "Not Correctly Displayed");
        }

        [Test]
        public void BGM_Output()
        {
            GameObject[] BGM = GameObject.FindGameObjectsWithTag("BGM");
            Assert.AreEqual(1, BGM.Length);
        }
    }

    // Unit: Login Page
    public class _1_LoginPage
    {
        [Test]
        public void UserInput_Input()
        {
            Assert.IsTrue(GameObject.Find("UserInput").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void PassInput_Input()
        {
            Assert.IsTrue(GameObject.Find("PassInput").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void LoginBtn_Input()
        {
            bool LoginCheck()
            {
                GameObject Btn = GameObject.Find("LoginBtn");
                if (Btn.activeInHierarchy && Btn.GetComponent<Button>().interactable) { return true; }
                else { return false; }
            }

            Assert.IsTrue(LoginCheck(), "Not Found or can't interact!");
        }

        [Test]
        public void LoginGuestBtn_Input()
        {
            bool LoginCheck()
            {
                GameObject Btn = GameObject.Find("GuestLoginBtn");
                if (Btn.activeInHierarchy && Btn.GetComponent<Button>().interactable) { return true; }
                else { return false; }
            }

            Assert.IsTrue(LoginCheck(), "Not Found!");
        }

        [Test]
        public void Logo_Display()
        {
            Assert.IsTrue(GameObject.Find("Logo").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void BGM_Output()
        {
            GameObject[] BGM = GameObject.FindGameObjectsWithTag("BGM");
            Assert.AreEqual(1, BGM.Length);
        }
    }

    // Unit: Gameplay
    public class _10_Gameplay
    {
        [Test]
        public void MusicInfo_Display()
        {
            Assert.IsTrue(GameObject.Find("SideUI_MusicInfo").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void MV_Display()
        {
            Assert.IsTrue(GameObject.Find("MV").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void Judgement_Line_Display()
        {
            Assert.IsTrue(GameObject.Find("Judgement Line").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void SpawnPoint_Display()
        {
            Assert.IsTrue(GameObject.Find("SpawnPoint").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void Character_MainSlot()
        {
            Assert.IsTrue(GameObject.Find("Character").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void Character_SideSlot_Left()
        {
            Assert.IsTrue(GameObject.Find("Character2").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void Character_SideSlot_Right()
        {
            Assert.IsTrue(GameObject.Find("Character3").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void PlayArea_Display()
        {
            Assert.IsTrue(GameObject.Find("PlayArea").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void Enemy_MainSlot()
        {
            Assert.IsTrue(GameObject.Find("Boss").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void ScoreBoard_Display()
        {
            Assert.IsTrue(GameObject.Find("BottomUI").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void BattleProgress_Display()
        {
            Assert.IsTrue(GameObject.Find("SideUI-BattleProgress").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void UnitStatus_Display()
        {
            Assert.IsTrue(GameObject.FindGameObjectWithTag("PartyStatus").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void RetreatBG_Display()
        {
            Assert.IsTrue(GameObject.Find("RetreatBG").activeInHierarchy, "Not Found!");
        }

        [Test]
        public void BossStatus_Display()
        {
            Assert.IsTrue(GameObject.Find("BossStatus").activeInHierarchy, "Not Found!");
        }
    }
}
