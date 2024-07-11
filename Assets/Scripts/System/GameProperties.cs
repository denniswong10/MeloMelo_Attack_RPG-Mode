using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MeloMelo_PlayerManagement
{
    public class UserControlSettings
    {
        private KeyCode[] leftMove = { KeyCode.A, KeyCode.LeftArrow };
        private KeyCode[] rightMove = { KeyCode.D, KeyCode.RightArrow };
        private bool anlogMovement = false;

        private KeyCode[] jumpKey = { KeyCode.W, KeyCode.UpArrow };
        private KeyCode[] attackKey = { KeyCode.O, KeyCode.P };
        private int[] mouseInputKey = { 0, 1 };

        private KeyCode[] espacePlay = { KeyCode.Escape };

        public KeyCode[] getInput_Left { get { return leftMove; } }
        public KeyCode[] getInput_Right { get { return rightMove; } }
        public KeyCode[] getInput_Jump { get { return jumpKey; } }
        public KeyCode[] getInput_Attack { get { return attackKey; } }
        public int[] get_mouseInputKey { get { return mouseInputKey; } }
        public bool get_anlogMovement { get { return anlogMovement; } }

        #region MODIFY
        public void LoadSettingsOfAnlogMovement(bool enable)
        {
            anlogMovement = enable;
        }
        #endregion

        #region COMPONENT
        private bool GetInputKeyTest(KeyCode[] keys)
        {
            foreach (KeyCode key in keys) if (Input.GetKey(key)) return true;
            return false;
        }

        private bool GetInputKeyDownTest(KeyCode[] keys)
        {
            foreach (KeyCode key in keys) if (Input.GetKeyDown(key)) return true;
            return false;
        }

        private bool GetInputMouseDownTest(int[] keys)
        {
            foreach (int key in keys) if (Input.GetMouseButtonDown(key)) return true;
            return false;
        }
        #endregion

        #region TEST
        public bool GetForInputLeftMove()
        {
            return GetInputKeyTest(leftMove);
        }

        public bool GetForInputRightMove()
        {
            return GetInputKeyTest(rightMove);
        }

        public bool GetForInputAttackKey()
        {
            return GetInputKeyDownTest(attackKey);
        }

        public bool GetForMouseInputAttack()
        {
            return GetInputMouseDownTest(mouseInputKey);
        }

        public bool GetForInputJumpKey()
        {
            return GetInputKeyDownTest(jumpKey);
        }

        public bool GetForInputExitPlay()
        {
            return GetInputKeyTest(espacePlay);
        }
        #endregion
    }

    public interface CharacterSettings_Properties
    {
        void Move();
        void Jump();
        void Hits();

        Vector3 BoundarySettings(float min, float max, Vector3 target);
        void LiveMotionSettings(CharacterSettings.STATE state);
        void ResetAttackKey(int key);
        void ResetJumpKey();

        string LiveMotionGetState(CharacterSettings.STATE _state);
        bool LiveMotionPlayBack(CharacterSettings.STATE state);
        string LiveMotionGetNPlayCurrent(CharacterSettings.STATE _state);

        void SetAutoPlayField(bool enable);
        void AutoPlayField();
    }

    public class JudgeBundle
    {
        int judge_status;
        int fastNLate_status;

        public void SetMain(int status) { judge_status = status; }
        public void SetOffbeat(int offTime) { fastNLate_status = offTime; }

        public int get_judge { get { return judge_status; } }
        public int get_fastLate { get { return fastNLate_status; } }
    }

    public class CharacterSettings : CharacterSettings_Properties
    {
        private UserControlSettings input = new UserControlSettings();
        public UserControlSettings getInput { get { return input; } }

        public enum STATE { Idle, Move, Attack };
        private STATE currentState = STATE.Idle;
        private string[] state = { "Idle", "Move", "Attack" };

        public enum PICKUP_TYPE { NONE, JUMP, ITEM, ITEM3, TRAP };
        public enum TRAPS_TYPE { NONE, GROUND, AIR };

        public enum action_state { normal, fade, rest, none };
        public enum timing_state { fast, perfect, late };

        private action_state attack = action_state.none;
        private float offsetBeat_timer = 0;
        private float offsetBeat_timer_critcal = 0;

        private bool[] attack_active = { false, false };
        public bool[] get_attackActive { get { return attack_active; } }

        private action_state jump = action_state.none;

        private bool jump_active = false;
        public bool get_jumpActive { get { return jump_active; } }

        private action_state move = action_state.none;
        private bool move_active = false;
        public bool get_moveActive { get { return move_active; } }

        private const float speed = 3;
        private Vector3 position;
        public Vector3 getPosition { get { return position; } }

        private bool autoDetect = false;
        public bool get_AutoFieldDetect { get { return autoDetect; } }

        #region MAIN
        public void Move()
        {
            if (input.get_anlogMovement)
            {
                Vector3 moveX = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
                position = moveX * speed * Time.deltaTime;
            }
            else
            {
                MoveKeyCheckerSetting(Vector3.left, input.getInput_Left);
                MoveKeyCheckerSetting(Vector3.right, input.getInput_Right);
            }
        }

        public void Jump()
        {
            JumpKeyCheckerSetting(input.getInput_Jump);
        }

        public void Hits()
        {
            HitKeyCheckerSetting(input.getInput_Attack);
            HitKeyCheckerMouseSetting(input.get_mouseInputKey);
        }
        #endregion

        #region COMPONENT
        private void MoveKeyCheckerSetting(Vector3 target, KeyCode[] keys)
        {
            foreach (KeyCode key in keys)
            {
                if (currentState == STATE.Move && Input.GetKey(key)) position = target * speed * Time.deltaTime;
                else if (currentState == STATE.Idle && !Input.GetKey(key)) position = Vector3.zero;
            }
        }

        // Attack: Modified
        #region ATTACK
        private void HitKeyCheckerSetting(KeyCode[] keys)
        {
            for (int key = 0; key < keys.Length; key++)
            {
                if (Input.GetKeyDown(keys[key]) && !attack_active[key])
                {
                    attack_active[key] = true;

                    // Add Code
                }
            }
        }

        private void HitKeyCheckerMouseSetting(int[] keys)
        {
            for (int key = 0; key < keys.Length; key++)
            {
                if (Input.GetMouseButtonDown(keys[key]) && !attack_active[key])
                {
                    attack_active[key] = true;

                    // Add Code
                }
            }
        }

        public void SwitchToNextAttackState()
        {
            switch (attack)
            {
                case action_state.normal:
                    attack = action_state.fade;
                    break;

                case action_state.fade:
                    attack = action_state.rest;
                    break;

                case action_state.rest:
                    attack = action_state.none;
                    break;

                case action_state.none:
                    attack = action_state.normal;
                    break;
            }
        }

        public void ResetAttackState()
        {
            attack = action_state.none;
        }

        public bool GetAttackPerfectTune()
        {
            return attack == action_state.normal;
        }
        #endregion

        // Jump: Modified
        #region JUMP
        private void JumpKeyCheckerSetting(KeyCode[] keys)
        {
            foreach (KeyCode key in keys)
            {
                if (Input.GetKeyDown(key) && !jump_active)
                {
                    jump_active = true;

                    // Add Code
                }
            }
        }

        public void SwitchToNextJumpState()
        {
            switch (jump)
            {
                case action_state.normal:
                    jump = action_state.fade;
                    break;

                case action_state.fade:
                    jump = action_state.rest;
                    break;

                case action_state.rest:
                    jump = action_state.none;
                    break;

                case action_state.none:
                    jump = action_state.normal;
                    break;
            }
        }

        public void ResetJumpState()
        {
            jump = action_state.none;
        }

        public bool GetJumpPerfectTune()
        {
            return jump == action_state.normal;
        }
        #endregion
        #endregion

        #region MODIFY
        public Vector3 BoundarySettings(float min, float max, Vector3 target)
        {
            return new Vector3(Mathf.Clamp(target.x, min, max), target.y, target.z);
        }

        public void LiveMotionSettings(STATE state)
        {
            currentState = state;
        }

        public void ResetAttackKey(int key)
        {
            attack_active[key] = false;
        }

        public void ResetJumpKey()
        {
            jump_active = false;
        }
        #endregion

        #region MISC
        public string LiveMotionGetState(STATE _state)
        {
            return state[(int)_state];
        }

        public bool LiveMotionPlayBack(STATE state)
        {
            return (int)currentState == (int)state;
        }

        public string LiveMotionGetNPlayCurrent(STATE _state)
        {
            if (currentState == _state) return state[(int)_state];
            return string.Empty;
        }

        public float GetAttackInternalTime(float startTime)
        {
            switch (attack)
            {
                case action_state.normal:
                    return startTime * 0.15f; // 0.25f

                case action_state.fade:
                    return startTime * 0.20f; // 0.5f

                case action_state.rest:
                    return startTime * 0.65f; // 0.5f
            }

            return (startTime * 0.5f) + (startTime * 0.1f);
        }

        public float GetJumpInternalTime(float startTime)
        {
            switch (jump)
            {
                case action_state.normal:
                    return startTime * 0.2f;

                case action_state.fade:
                    return startTime * 0.25f;

                case action_state.rest:
                    return startTime * 0.55f;
            }

            return 1;
        }

        public Vector3 GetJumpPosition(Vector3 target)
        {
            if (jump != action_state.none) return new Vector3(target.x, 1, target.z);
            return new Vector3(target.x, 0, target.z);
        }

        public bool IsJumpFinished()
        {
            if (jump == action_state.none) return true;
            return false;
        }

        public JudgeBundle GetJumpStatusById()
        {
            JudgeBundle bundle = new JudgeBundle();
            bundle.SetMain(!autoDetect ? 1 : (int)jump + 1);
            bundle.SetOffbeat(bundle.get_judge == 1 ? GetCritcalJudgeTiming() : GetJudgeTimingState());

            ResetJumpState();
            return bundle;
        }

        public bool IsAttackFinished()
        {
            if (attack == action_state.none) return true;
            return false;
        }

        public JudgeBundle GetPickUpStatusById()
        {
            JudgeBundle bundle = new JudgeBundle();

            switch (attack)
            {
                case action_state.normal:
                    bundle.SetMain(3);
                    break;

                case action_state.fade:
                case action_state.rest:
                    bundle.SetMain(2);
                    break;

                default:
                    bundle.SetMain(1);
                    break;
            }

            bundle.SetOffbeat(bundle.get_judge == 1 ? 1 : GetJudgeTimingState());
            return bundle;
        }

        public JudgeBundle GetAttackStatusById()
        {
            JudgeBundle bundle = new JudgeBundle();
            bundle.SetMain((int)attack + 1);
            bundle.SetOffbeat(bundle.get_judge == 1 ? GetCritcalJudgeTiming() : GetJudgeTimingState());

            ResetAttackState();
            return bundle;
        }

        public JudgeBundle GetSpeicalItemStatusById()
        {
            JudgeBundle bundle = new JudgeBundle();
            bundle.SetMain(jump != action_state.none ? 2 : 3);
            bundle.SetOffbeat(bundle.get_judge == 1 ? 1 : GetJudgeTimingState());
            return bundle;
        }

        private JudgeBundle GetAirTrapStatusById()
        {
            JudgeBundle bundle = new JudgeBundle();
            if (!autoDetect) bundle.SetMain((int)jump > (int)action_state.fade ? 2 : 4);
            else bundle.SetMain(1);

            return bundle;
        }

        private JudgeBundle GetGroundTrapStatusById()
        {
            JudgeBundle bundle = new JudgeBundle();
            if (!autoDetect) bundle.SetMain(attack != action_state.none ? 2 : 4);
            else bundle.SetMain(1);

            return bundle;
        }

        public JudgeBundle GetTrapsStatusById(TRAPS_TYPE note_type_status)
        {
            if (note_type_status == TRAPS_TYPE.GROUND)
                return GetGroundTrapStatusById();
            else
                return GetAirTrapStatusById();
        }
        #endregion

        #region EXTRA
        public void SetAutoPlayField(bool enable)
        {
            autoDetect = enable;
        }

        public void AutoPlayField()
        {
            attack_active[0] = true;
            jump_active = true;
        }

        public void SetOffsetBeatTime(float time)
        {
            const float halfCutTime = 0.5f;
            const float quarterCutTime = halfCutTime + 0.25f;

            offsetBeat_timer = Time.time + time * halfCutTime;
            offsetBeat_timer_critcal = Time.time + time * quarterCutTime;
        }

        private int GetJudgeTimingState()
        {
            if (Time.time < offsetBeat_timer) return (int)timing_state.fast;
            else if (Time.time > offsetBeat_timer) return (int)timing_state.late;
            else return (int)timing_state.late;
        }

        private int GetCritcalJudgeTiming()
        {
            if (Time.time < offsetBeat_timer) return (int)timing_state.perfect;
            else if (Time.time > offsetBeat_timer && Time.time < offsetBeat_timer_critcal) return (int)timing_state.fast;
            else if (Time.time > offsetBeat_timer_critcal) return (int)timing_state.late;
            else return (int)timing_state.late;
        }
        #endregion
    }
}

namespace MeloMelo_GameProperties
{
    public class BattleGroundFieldComponent
    {
        public enum PlayArea { mirco, mini, small, medium, large };
        private PlayArea myArea = PlayArea.large;
        private string currentPlaySize = string.Empty;
        private Color playMatStatus = Color.black;
        public Color get_playMatStatus { get { return playMatStatus; } } 

        private string[] playAreaDefine = { "X", "Mini", "Small", "Medium", "Large" };
        public string[] get_playAreaDefine { get { return playAreaDefine; } }

        private float[] limitBorderRef = { 0.1f, 0.25f, 0.53f, 0.81f, 1.21f };
        private float[] playBorderRef = { 0.05f, 0.10f, 0.16f, 0.21f, 0.29f };

        private float limitBorder = 1.21f;
        public float get_limitBorder { get { return limitBorder; } }
        private float playBorder = 0.28f;
        public float get_playBorder { get { return playBorder; } }

        #region MAIN
        public void SetPlayField(string area)
        {
            switch (area)
            {
                case "X":
                    myArea = PlayArea.mirco;
                    break;

                case "Mini":
                    myArea = PlayArea.mini;
                    break;

                case "Small":
                    myArea = PlayArea.small;
                    break;

                case "Medium":
                    myArea = PlayArea.medium;
                    break;

                case "Large":
                    myArea = PlayArea.large;
                    break;
            }

            currentPlaySize = area;
        }

        public bool IsDrawAreaPossible()
        {
            for (int area = 0; area < playAreaDefine.Length; area++)
            {
                if (currentPlaySize == playAreaDefine[area])
                {
                    if (area > 0) return false;
                }
            }

            return true;
        }

        public void SwitchPlayFieldMat(bool reserve)
        {
            if (reserve)
            {
                switch (myArea)
                {
                    case PlayArea.mirco:
                        myArea = PlayArea.mini;
                        break;

                    case PlayArea.mini:
                        myArea = PlayArea.small;
                        break;

                    case PlayArea.small:
                        myArea = PlayArea.large;
                        break;

                    case PlayArea.medium:
                        myArea = PlayArea.small;
                        break;

                    case PlayArea.large:
                        myArea = PlayArea.medium;
                        break;
                }
            }

            else
            {
                switch (myArea)
                {
                    case PlayArea.mirco:
                        myArea = PlayArea.large;

                        break;
                    case PlayArea.mini:
                        myArea = PlayArea.mirco;
                        break;

                    case PlayArea.small:
                        myArea = PlayArea.medium;
                        break;

                    case PlayArea.medium:
                        myArea = PlayArea.large;
                        break;

                    case PlayArea.large:
                        myArea = PlayArea.small;
                        break;
                }
            }
        }

        public void ModifyPlayBorder(float amount)
        {
            if ((amount < 0 && playBorder - amount < playBorderRef[(int)myArea]) || (amount > 0 && playBorder + amount > playBorderRef[(int)myArea]))
            { playBorder = playBorderRef[(int)myArea]; }
            else if (playBorder != playBorderRef[(int)myArea]) { playBorder += amount; }
        }

        public void ModifyLimitBorder(float amount)
        {
            if ((amount < 0 && limitBorder - amount < limitBorderRef[(int)myArea]) || (amount > 0 && limitBorder + amount > limitBorderRef[(int)myArea]))
            { limitBorder = limitBorderRef[(int)myArea]; }
            else if (limitBorder != limitBorderRef[(int)myArea]) { limitBorder += amount; }
        }

        public bool CheckCompleteFieldDrawOut()
        {
            bool condition = Equals(playBorder, playBorderRef[(int)myArea]) && Equals(limitBorder, limitBorderRef[(int)myArea]);
            playMatStatus = condition ? Color.white : new Color(0.6f, 0.55f, 0.05f);

            return condition;
        }

        public bool CheckCompleteFieldEraseOut()
        {
            bool condition = Equals(playBorder, playBorderRef[(int)myArea]) && Equals(limitBorder, limitBorderRef[(int)myArea]);
            playMatStatus = condition ? Color.white : new Color(0.6f, 0.55f, 0.05f);

            return condition;
        }

        public void FinishedStageField(int status)
        {
            Color stageColor = Color.white;

            switch (status)
            {
                case 1:
                    stageColor = Color.magenta;
                    break;

                case 2:
                    stageColor = Color.blue;
                    break;

                case 3:
                    stageColor = new Color(0.2f, 1, 0.95f);
                    break;

                case 4:
                    stageColor = Color.green;
                    break;

                case 5:
                    stageColor = Color.black;
                    break;
            }

            GameObject.Find("PlayArea").transform.GetComponent<Renderer>().material.color = stageColor;
        }
        #endregion
    }

    public class BattleProgressMeter
    {
        private const float clearedValue = 80;

        private const string unplayed_remark = "DEFEATED!";
        private const string played_remark = "CLEARED!";

        private string[] status = { "LEVEL CLEARED!", "LEVEL FAILED!" };
        private string[] special_remark = { "PERFECT!", "ALL ELIMINATE!", "MISSLESS!" };

        private string currentStatus = string.Empty;

        private Color cleared = new Color32(36, 137, 5, 255);
        public Color get_cleared { get { return cleared; } }

        private Color failed = new Color32(255, 0, 0, 255);
        public Color get_failed { get { return failed; } }

        private float battleProgress = 0;
        public float get_battleProgress { get { return battleProgress; } }

        private bool warningAlert = false;

        #region BASIC
        public void Modify_BattleProgress(float amount)
        {
            battleProgress += amount;
        }

        public Color GetBattleProgressBorder()
        {
            return (int.Parse(battleProgress.ToString("0")) >= clearedValue) ? cleared : failed;
        }

        public bool MaxOutBattleProgress()
        {
            return battleProgress.ToString("0") == "100";
        }

        public bool ClearedBattleProgress()
        {
            return int.Parse(battleProgress.ToString("0")) >= clearedValue;
        }
        #endregion

        #region INTERMIDATE
        public bool ProgressDangerAlert(bool condition)
        {
            if (!warningAlert && condition) warningAlert = true;
            else if (battleProgress >= clearedValue && warningAlert) return false;

            return warningAlert;
        }

        public string GetProgressPassNFailStatus()
        {
            if (int.Parse(battleProgress.ToString("0")) >= clearedValue) return status[0];
            else return status[1];
        }

        public string GetProgressSpecialStatus(int index)
        {
            return special_remark[index - 1];
        }

        public string GetProgressUnplayedStatus()
        {
            return unplayed_remark;
        }

        public string GetProgressPlayedStatus()
        {
            return played_remark;
        }

        public void SetProgressRemark(int value)
        {
            currentStatus = value > 0 && value < special_remark.Length + 1 ? 
                special_remark[value - 1] : 
                value == 4 ? played_remark : 
                unplayed_remark;
        }

        private int ProgressRemarkIndex(string remark)
        {
            int count = 0;

            foreach (string status in special_remark)
            {
                count++;
                if (status == remark) return count;
            }

            count++;
            if (played_remark == remark) return count;

            count++;
            if (unplayed_remark == remark) return count;

            count++;
            return count;
        }

        public bool ProgressRemarkChecker(string remark)
        {
            int currentStatusIndex = ProgressRemarkIndex(currentStatus);
            int statusIndex = ProgressRemarkIndex(remark);
            return (currentStatusIndex > statusIndex);
        }
        #endregion
    }

    public class UnitStatusComponent
    {
        private int health = 0;
        private int maxhealth = 0;
        private int knockOutValue = 0;

        public int get_health { get { return health; } }
        public int get_maxHealth { get { return maxhealth; } }
        public int get_knockOutValue { get { return knockOutValue; } }

        public void ModifyHealth(int amount)
        {
            if ((health + amount) >= maxhealth) health = maxhealth;
            else if ((health + amount) > 0) health += amount;
            else if ((health + amount) < 0) ModifyKnockOutValue(amount);
        }

        public void SetMaxHealth(int value)
        {
            maxhealth = value;
        }

        private void ModifyKnockOutValue(int amount)
        {
            if (health > 0) health = 0;
            knockOutValue += amount;
        }
    }

    public class JudgeWindowComponent
    {
        private int perfect2 = 0;
        private int perfect = 0;
        private int bad = 0;
        private int miss = 0;

        public int get_perfect2 { get { return perfect2; } }
        public int get_perfect { get { return perfect; } }
        public int get_bad { get { return bad; } }
        public int get_miss { get { return miss; } }

        private int combo = 0;
        private int maxCombo = 0;
        private int overallCombo = 0;

        public int getCombo { get { return combo; } }
        public int getMaxCombo { get { return maxCombo; } }
        public int getOverallCombo { get { return overallCombo; } }

        public void UpdateJudgeStatus(int index)
        {
            switch (index)
            {
                case 1:
                    perfect2++;
                    AddCombo();
                    break;

                case 2:
                    perfect++;
                    AddCombo();
                    break;

                case 3:
                    bad++;
                    AddCombo();
                    break;

                default:
                    miss++;
                    ResetCombo();
                    break;
            }
        }

        #region BASIC
        public void AddTotalCombo()
        {
            overallCombo++;
        }

        private void AddCombo()
        {
            combo++;
            if (combo >= maxCombo) { AddMaxCombo(); }
        }

        private void ResetCombo()
        {
            combo = 0;
        }

        private void AddMaxCombo()
        {
            maxCombo++;
        }
        #endregion

        #region INTERMIDATE
        public bool LevelPlayCleared()
        {
            return (perfect2 + perfect + bad + miss) >= overallCombo;
        }

        public bool FullComboPlay()
        {
            if (maxCombo >= overallCombo) return true;
            else return false;
        }

        public bool AllPerfectPlus()
        {
            if (perfect2 >= overallCombo) return true;
            else return false;
        }

        public bool AllPerfectPlay()
        {
            if ((perfect2 + perfect) >= overallCombo) return true;
            else return false;
        }

        public bool AllEliminatePlay(bool condition)
        {
            if (condition && miss == 0) return true;
            else return false;
        }
        #endregion

        #region CUSTOM
        public int TotalJudgeCounted()
        {
            return perfect2 + perfect + bad + miss;
        }

        public bool IsPerfectChained()
        {
            if (bad == 0 && miss == 0) return true;
            else return false;
        }
        #endregion
    }

    public class GameplayObjectComponent
    {
        private int totalEnemy = 0;
        private int totalTraps = 0;

        public int getTotalEnemy { get { return totalEnemy; } }
        public int getTotalTraps { get { return totalTraps; } }

        public void AddEnemyCount()
        {
            totalEnemy++;
        }

        public void AddTrapsCount()
        {
            totalTraps++;
        }
    }

    public class GameSystem_Score
    {
        private float score = 0;
        private float maxScore = 0;

        public float get_score { get { return score; } }
        public float get_maxScore { get { return maxScore; } }

        #region BASIC
        public void ModifyScore(int amount)
        {
            if (maxScore != 0 && (score + amount) >= maxScore) score = maxScore;
            else score += amount;
        }

        public void ResetScore()
        {
            score = 0;
        }

        public void SetMaxScore(int value)
        {
            maxScore = value;
        }
        #endregion

        #region INTERMIDATE
        public bool MaxOutScore()
        {
            if (score >= maxScore) return true;
            else return false;
        }
        #endregion
    }
}
