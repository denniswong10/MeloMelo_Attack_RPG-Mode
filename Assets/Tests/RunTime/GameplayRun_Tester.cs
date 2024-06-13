using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using MeloMelo_GameProperties;
using MeloMelo_PlayerManagement;

namespace TitleMenu
{
    public class Network_Tester
    {
        bool check_login = false;

        [UnityTest]
        public IEnumerator Checking_Login()
        {
            if (!check_login)
            {
                check_login = true;
            }

            yield return new WaitForSeconds(3);
            Assert.AreNotEqual("GUEST", LoginPage_Script.thisPage.get_user);
        }
    }
}

namespace Gameplay
{
    // Unit: Gameplay
    public class BattleProgressMeter_Component
    {
        [UnityTest]
        public IEnumerator PerfectPlus()
        {
            BattleProgressMeter properties = new BattleProgressMeter();
            properties.Modify_BattleProgress(99.5f);
            BattleProgressResult(properties.get_battleProgress, "Perfect+");
            properties.SetProgressRemark(1);

            JudgeWindowComponent judgeWindow = new JudgeWindowComponent();

            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(judgeWindow.AllPerfectPlus() && properties.MaxOutBattleProgress()
                && properties.ProgressRemarkChecker(properties.GetProgressSpecialStatus(1)), true);
        }

        [UnityTest]
        public IEnumerator Perfect()
        {
            BattleProgressMeter properties = new BattleProgressMeter();
            properties.Modify_BattleProgress(99.7f);
            BattleProgressResult(properties.get_battleProgress, "Perfect");
            properties.SetProgressRemark(1);

            JudgeWindowComponent judgeWindow = new JudgeWindowComponent();

            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(judgeWindow.AllPerfectPlay() && properties.MaxOutBattleProgress()
                && properties.ProgressRemarkChecker(properties.GetProgressSpecialStatus(1)), true);
        }

        [UnityTest]
        public IEnumerator AllEliminate()
        {
            BattleProgressMeter properties = new BattleProgressMeter();
            properties.Modify_BattleProgress(99.51f);
            BattleProgressResult(properties.get_battleProgress, "AllEliminate");
            properties.SetProgressRemark(2);

            JudgeWindowComponent judgeWindow = new JudgeWindowComponent();

            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(judgeWindow.AllEliminatePlay(properties.MaxOutBattleProgress())
                && properties.ProgressRemarkChecker(properties.GetProgressSpecialStatus(2)), true);
        }

        [UnityTest]
        public IEnumerator Missless()
        {
            BattleProgressMeter properties = new BattleProgressMeter();
            properties.Modify_BattleProgress(99.52f);
            BattleProgressResult(properties.get_battleProgress, "Missless");
            properties.SetProgressRemark(3);

            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(properties.MaxOutBattleProgress() && properties.ProgressRemarkChecker(properties.GetProgressSpecialStatus(3)), true);
        }

        [UnityTest]
        public IEnumerator Cleared()
        {
            BattleProgressMeter properties = new BattleProgressMeter();

            properties.Modify_BattleProgress(79.75f);
            BattleProgressResult(properties.get_battleProgress, "Cleared!");
            properties.SetProgressRemark(4);

            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(properties.GetBattleProgressBorder() == properties.get_cleared && properties.ClearedBattleProgress()
                && properties.ProgressRemarkChecker(properties.GetProgressPlayedStatus()), true);
        }

        [UnityTest]
        public IEnumerator Failed()
        {
            BattleProgressMeter properties = new BattleProgressMeter();

            properties.Modify_BattleProgress(49.1f);
            BattleProgressResult(properties.get_battleProgress, "Failed!");
            properties.SetProgressRemark(5);

            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(properties.GetBattleProgressBorder() == properties.get_failed && !properties.ClearedBattleProgress()
                && properties.ProgressRemarkChecker(properties.GetProgressUnplayedStatus()), true);
        }

        #region COMPONENT
        private void BattleProgressResult(float value, string title)
        {
            Debug.Log("Current Result: " + title);
            Debug.Log("- Battle Progress: " + value);
        }
        #endregion
    }

    public class PerformanceScore_Component
    {
        [UnityTest]
        public IEnumerator PlayScore()
        {
            GameSystem_Score score = new GameSystem_Score();
            score.SetMaxScore(1000000);

            score.ModifyScore((int)(score.get_maxScore + Random.Range(100, 500)));

            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(score.get_score, score.get_maxScore);
        }

        [UnityTest]
        public IEnumerator ComboPenatly()
        {
            JudgeWindowComponent judge = new JudgeWindowComponent();
            for (int i = 0; i < Random.Range(1, 100); i++) judge.AddTotalCombo();
            for (int i = 0; i < 5; i++) judge.UpdateJudgeStatus(1);
            judge.UpdateJudgeStatus(4);
            judge.UpdateJudgeStatus(1);

            GameSystem_Score scorePenatly = new GameSystem_Score();
            scorePenatly.ModifyScore((judge.getCombo - judge.getMaxCombo));

            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(scorePenatly.get_score, -4);
        }
    }

    public class TechnicalScore_Component
    {
        [UnityTest]
        public IEnumerator PlayScore()
        {
            GameSystem_Score technicalScore = new GameSystem_Score();
            technicalScore.ModifyScore(200);

            yield return new WaitForSeconds(0.5f);
            Assert.Greater(technicalScore.get_score, technicalScore.get_maxScore);
        }
    }

    public class CharacterStatus_Component
    {
        [UnityTest]
        public IEnumerator Damage()
        {
            UnitStatusComponent characterStatus = new UnitStatusComponent();
            characterStatus.SetMaxHealth(50);
            characterStatus.ModifyHealth(characterStatus.get_maxHealth);
            characterStatus.ModifyHealth(Random.Range(-1, -25));

            yield return new WaitForSeconds(0.5f);
            Assert.Less(characterStatus.get_health, characterStatus.get_maxHealth);
        }

        [UnityTest]
        public IEnumerator Restore()
        {
            UnitStatusComponent characterStatus = new UnitStatusComponent();
            characterStatus.SetMaxHealth(50);
            characterStatus.ModifyHealth(characterStatus.get_maxHealth);
            characterStatus.ModifyHealth(-10);
            characterStatus.ModifyHealth(Random.Range(1, 25));

            yield return new WaitForSeconds(0.5f);
            Assert.Greater(characterStatus.get_health, characterStatus.get_health - 10);
        }

        [UnityTest]
        public IEnumerator KnockOut()
        {
            UnitStatusComponent characterStatus = new UnitStatusComponent();
            characterStatus.SetMaxHealth(50);
            characterStatus.ModifyHealth(-100);

            yield return new WaitForSeconds(0.5f);
            Assert.AreNotEqual(characterStatus.get_knockOutValue, 0);
        }

        [UnityTest]
        public IEnumerator MoveFunction()
        {
            CharacterSettings character = new CharacterSettings();
            Vector3 original = character.getPosition;
            character.Move();

            yield return new WaitForSeconds(0.1f);
            Assert.AreNotEqual(character.getPosition, original);
        }

        [UnityTest]
        public IEnumerator PlayerBoundary()
        {
            CharacterSettings character = new CharacterSettings();
            character.BoundarySettings(-1, 1, character.getPosition);

            yield return new WaitForSeconds(0.1f);
            Assert.AreNotEqual(character.getPosition, 0);
        }

        [UnityTest]
        public IEnumerator LiveMotionFunction()
        {
            bool moving = false;
            bool attacking = false;

            CharacterSettings character = new CharacterSettings();

            character.Move();
            character.LiveMotionSettings(CharacterSettings.STATE.Move);

            character.Hits();
            character.LiveMotionSettings(CharacterSettings.STATE.Attack);

            yield return new WaitUntil(() => character.LiveMotionPlayBack(CharacterSettings.STATE.Idle));
            Assert.AreEqual(!moving && !attacking, true);
        }

        [UnityTest]
        public IEnumerator HitsFunction()
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    public class EnemyStatus_Component
    {
        [UnityTest]
        public IEnumerator Damage()
        {
            UnitStatusComponent enemyStatus = new UnitStatusComponent();
            enemyStatus.SetMaxHealth(100);
            enemyStatus.ModifyHealth(enemyStatus.get_maxHealth);
            enemyStatus.ModifyHealth(Random.Range(-1, -50));

            yield return new WaitForSeconds(0.5f);
            Assert.Less(enemyStatus.get_health, enemyStatus.get_maxHealth);
        }

        [UnityTest]
        public IEnumerator KnockOut()
        {
            UnitStatusComponent enemyStatus = new UnitStatusComponent();
            enemyStatus.SetMaxHealth(100);
            enemyStatus.ModifyHealth(-1000);

            yield return new WaitForSeconds(0.5f);
            Assert.AreNotEqual(enemyStatus.get_knockOutValue, 0);
        }
    }

    public class JudgeTimingWindow
    {
        [UnityTest]
        public IEnumerator PerfectHit()
        {
            JudgeWindowComponent component = new JudgeWindowComponent();
            component.UpdateJudgeStatus(Random.Range(1, 2));
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual((component.get_perfect != 0 || component.get_perfect2 != 0), true);
        }

        [UnityTest]
        public IEnumerator ParticleHit()
        {
            JudgeWindowComponent component = new JudgeWindowComponent();
            component.UpdateJudgeStatus(3);
            yield return new WaitForSeconds(0.5f);
            Assert.Greater(component.get_bad, 0);
        }

        [UnityTest]
        public IEnumerator MissHit()
        {
            JudgeWindowComponent component = new JudgeWindowComponent();
            component.UpdateJudgeStatus(4);
            yield return new WaitForSeconds(0.5f);
            Assert.Greater(component.get_miss, 0);
        }

        [UnityTest]
        public IEnumerator RunCombo()
        {
            int comboSet = Random.Range(1, 100);
            JudgeWindowComponent component = new JudgeWindowComponent();
            for (int i = 0; i < comboSet; i++) component.AddTotalCombo();
            for (int i = 0; i < comboSet; i++) component.UpdateJudgeStatus(1);

            yield return new WaitUntil(() => component.getMaxCombo >= component.getOverallCombo);
            Assert.AreEqual(component.getMaxCombo, component.getOverallCombo);
        }
    }
}
