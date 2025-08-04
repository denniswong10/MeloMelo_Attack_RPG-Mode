using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_RPGEditor;
using MeloMelo_ExtraComponent;
using UnityEngine.UI;
using MeloMelo_PlayerManagement;

public class Character : MonoBehaviour
{
    public GameObject ForceFieldMat;
    public GameObject Barrier;
    public GameObject PartyStatus;

    private CharacterSettings character;
    public CharacterSettings get_character { get { return character; } }
    public Character_StorageStats stats;

    private bool isJumping = false;
    private bool[] isAttacking = { false, false };

    private void OnTriggerEnter(Collider other)
    {
        // Detect all objects are interactable
        if (other.tag == "note" && other.GetComponent<Note_Script>().note_define_index != CharacterSettings.PICKUP_TYPE.NONE)
        {
            // Register as a traps
            if (IsObstacleDodgePossible(other.GetComponent<Note_Script>().note_define_index))
                Obstacle_Traps_Detection(other.GetComponent<Note_Script>());

            // Register as a pickable object
            else if (IsPickUpPossible(other.GetComponent<Note_Script>().note_define_index))
                PickUp_Item(other.GetComponent<Note_Script>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Detect all objects are clickable at the same time of interactable
        if (other.tag == "note" && other.GetComponent<Note_Script>().note_define_index == CharacterSettings.PICKUP_TYPE.NONE)
        {
            if (character.get_AutoFieldDetect) SimulateHitTarget();

            if (IsAttackInputReceived())
            {
                if (!other.GetComponent<Note_Script>().TriggerAsHits())
                {
                    switch (other.GetComponent<Note_Script>().note_index)
                    {
                        // Register as a enemy unit
                        case 1:
                        case 2:
                            TimingSequenceToAttack(other.GetComponent<Note_Script>(), "EnemyHit");
                            break;

                        // Register as incoming enemy attack
                        case 3:
                            TimingSequenceToAttack(other.GetComponent<Note_Script>(), "EnemyAttack2");
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }

    void Start()
    {
        if (!GameManager.thisManager.DeveloperMode)
        {
            GetComponent<Animator>().enabled = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetCharacterAnimation_ValueKey) == 1 ? true : false;

            LoadSettings();
            LoadCharacterStats();
        }
    }

    void Update()
    {
        if (!GameManager.thisManager.DeveloperMode)
        {
            if (character.get_AutoFieldDetect)
            {
                character.AutoPlayField();
                SetCharacterAutoField();
            }
            else
            {
                PlayerSettingsControl();
                PlayerAttackAction();
                PlayerJumpAction();
            }

            GameManager.thisManager.AutoPlayDisplay(character.get_AutoFieldDetect);
        }
    }

    #region SETUP
    private void LoadSettings()
    {
        stats = new Character_StorageStats("NA", 1);
        character = new CharacterSettings();
    }

    private void LoadCharacterStats()
    {
        // Caculate all party member health
        stats.Update_Character_StorageStats(PartyStatus, PlayerPrefs.GetString("CharacterFront", "NA"), PlayerPrefs.GetInt("Character_OverallHealth", 0));

        // Field Play: Properties
        character.SetAutoPlayField(BeatConductor.thisBeat.autoPlayField);
    }

    private void SetCharacterAutoField()
    {
        BoxCollider field = ForceFieldMat.GetComponent<BoxCollider>();
        GetComponent<BoxCollider>().center = field.center;
        GetComponent<BoxCollider>().size = field.size;
    }
    #endregion

    #region MAIN
    private void PlayerSettingsControl()
    {
        // Move Control: Implemented for user playing
        character.Move();
        transform.Translate(character.getPosition * (isJumping ? 1.5f : 1));
        transform.position = character.BoundarySettings(-GameManager.thisManager.get_playField.get_limitBorder, GameManager.thisManager.get_playField.get_limitBorder, transform.position);

        // Attack Control: Implemented for user playing
        character.Hits();

        // Jump Control: Implemented for user playing
        character.Jump();

        // Animator: Enchant user experience
        if (!GameManager.thisManager.DeveloperMode)
        {
            // Set animator of current state
            if (character.getInput.GetForInputLeftMove() || character.getInput.GetForInputRightMove())
                character.LiveMotionSettings(CharacterSettings.STATE.Move);

            else if (character.getInput.GetForInputAttackKey() || character.getInput.GetForMouseInputAttack())
                character.LiveMotionSettings(CharacterSettings.STATE.Attack);

            else
                character.LiveMotionSettings(CharacterSettings.STATE.Idle);

            if (GetComponent<Animator>().enabled)
            {
                // Main Action Caller: Move
                GetComponent<Animator>().SetBool(character.LiveMotionGetState(CharacterSettings.STATE.Move), character.LiveMotionPlayBack(CharacterSettings.STATE.Move));

                // Sub Action Caller: Attack
                if (character.LiveMotionPlayBack(CharacterSettings.STATE.Attack))
                {
                    GetComponent<Animator>().SetTrigger(character.LiveMotionGetState(CharacterSettings.STATE.Attack));
                }
            }
        }
    }

    private void PlayerAttackAction()
    {
        //// Keyboard Input
        for (int key = 0; key < character.getInput.getInput_Attack.Length; key++)
        {
            if (!isAttacking[key] && character.get_attackActive[key] && character.getInput.GetForInputAttackKey())
            {
                isAttacking[key] = true;
                PerformAttack(key, key);
            }
        }

        // Mouse Input
        for (int mouseKey = 0; mouseKey < character.getInput.get_mouseInputKey.Length; mouseKey++)
        {
            if (!isAttacking[mouseKey] && character.get_attackActive[mouseKey] && character.getInput.GetForMouseInputAttack())
            {
                isAttacking[mouseKey] = true;
                PerformAttack(mouseKey + character.getInput.getInput_Attack.Length, mouseKey);
            }
        }
    }

    private void PlayerJumpAction()
    {
        // Keyboard Input
        if (!isJumping && character.get_jumpActive)
        {
            isJumping = true;
            PerformJump();
        }
    }
    #endregion

    // Character Jump: Function
    #region Jump: Function
    private void PerformJump()
    {
        // Jump Output: Y-Axis
        character.SwitchToNextJumpState();

        // Suspend and get to the next available state
        if (!character.IsJumpFinished())
        {
            transform.position = character.GetJumpPosition(transform.position);

            // Receive current state note timing
            float calculateNoteTime = BeatConductor.thisBeat.get_BPM_Calcuate != 0 ?
                character.GetJumpInternalTime(BeatConductor.thisBeat.get_BPM_Calcuate) : 1;
            character.SetOffsetBeatTime(calculateNoteTime);

            StartCoroutine(CountDownToNextJump(calculateNoteTime));
        }

        else CancelJump();
    }

    private IEnumerator CountDownToNextJump(float time)
    {
        yield return new WaitForSeconds(time);
        if (isJumping) PerformJump();
    }

    private void CancelJump()
    {
        character.ResetJumpState();
        transform.position = character.GetJumpPosition(transform.position);
        character.ResetJumpKey();
        isJumping = false;
    }
    #endregion

    #region Attack: Function
    private void PerformAttack(int key_output, int key_input)
    {
        // Key Output: Display
        character.SwitchToNextAttackState();
        GameObject.Find("HotKey_Controller").transform.GetChild(key_output).GetComponent<RawImage>().color = Color.black;

        // Suspend and get to the next available state
        if (!character.IsAttackFinished())
        {
            // Receive current state note timing
            float calculateNoteTime = BeatConductor.thisBeat.get_BPM_Calcuate != 0 ?
                character.GetAttackInternalTime(BeatConductor.thisBeat.get_BPM_Calcuate) : 1;
            character.SetOffsetBeatTime(calculateNoteTime);

            StartCoroutine(CountDownToNextAttack(key_input, key_output, calculateNoteTime));
        }

        else CancelAttack(key_output, key_input);
    }

    private IEnumerator CountDownToNextAttack(int key_input, int key_output, float time)
    {
        yield return new WaitForSeconds(time);
        if (isAttacking[key_input]) PerformAttack(key_output, key_input);
    }

    private void CancelAttack(int output, int key)
    {
        GameObject.Find("HotKey_Controller").transform.GetChild(output).GetComponent<RawImage>().color = Color.white;
        character.ResetAttackState();
        character.ResetAttackKey(key);
        isAttacking[key] = false;
    }
    #endregion

    // Note Hits: Function
    #region CHARACTER UPDATE GAMEPLAY
    private void JudgeNote_MainField(Note_Script note, string sound = "")
    {
        // Gameplay: Judgement
        CancelAnyActiveAction(note.note_define_index, note.note_index);
        JudgeNote_RegisterField(note.get_preJudgeNote, sound);
        if (note.get_preJudgeNote != 4) GameManager.thisManager.ModifyFastNLateJudge(note.get_preOffBeat, note.get_preJudgeNote);

        // Gameplay: Sub Component
        IncludeExtensionField(note);

        // Clear individual from field
        note.Despawn();
    }

    private void JudgeNote_RegisterField(int mainJudge, string sound)
    {
        switch (mainJudge)
        {
            case 1: // Perfect 2
                GameManager.thisManager.UpdateNoteStatus("Perfect_2");
                GameManager.thisManager.UpdatePoint(3);
                IncludePerfromanceScore(BeatConductor.thisBeat.get_scorePerfect2);
                PlaySoundEffect(sound);
                break;

            case 2: // Perfect
                GameManager.thisManager.UpdateNoteStatus("Perfect");
                GameManager.thisManager.UpdatePoint(2);
                IncludePerfromanceScore(BeatConductor.thisBeat.get_scorePerfect);
                PlaySoundEffect(sound);
                break;

            case 3: // Bad
                GameManager.thisManager.UpdateNoteStatus("Bad");
                GameManager.thisManager.UpdatePoint(1);
                IncludePerfromanceScore(BeatConductor.thisBeat.get_scoreBad);
                PlaySoundEffect(sound);
                break;

            default: // Miss
                GameManager.thisManager.UpdateNoteStatus("Miss");
                break;
        }
    }

    private void IncludeExtensionField(Note_Script note)
    {
        float damage_Multipler = note.get_preJudgeNote == 3 ? 0.5f : 1;
        if (note.note_define_index == CharacterSettings.PICKUP_TYPE.NONE) DamagingEnemyProgress(damage_Multipler, note.note_index == 1 || note.note_index == 2);
        IncludeTechnicalScore(note);
    }
    #endregion

    #region CHARACTER HIT DETECTION
    private void PickUp_Item(Note_Script note_action)
    {
        switch (note_action.note_define_index)
        {
            case CharacterSettings.PICKUP_TYPE.JUMP:
                //if (character.get_AutoFieldDetect)
                //{
                //    float delayJump = BeatConductor.thisBeat.get_BPM_Calcuate * 0.15f;
                //    Invoke("SimulateJump", delayJump);
                //}

                if (character.get_jumpActive || character.get_AutoFieldDetect)
                {
                    IncludeCharacterHealing();
                    TimingSequenceForJump(note_action);
                }
                break;

            case CharacterSettings.PICKUP_TYPE.ITEM3:
                if (!character.get_AutoFieldDetect) TimingSequenceForSpecialItem(note_action);
                break;

            default: // Item
                if (!character.get_attackActive[0] || !character.get_attackActive[1])
                    TimingSequenceForItem(note_action, "Item_" + note_action.name);
                break;
        }
    }

    private void Obstacle_Traps_Detection(Note_Script target)
    {
        if (GetObstacleHit_Condition(target.note_index))
        {
            JudgeBundle isJudgeAsTraps = character.GetTrapsStatusById(FindAssignIndex(target.note_index));
            target.SetNoteCompleted(isJudgeAsTraps.get_judge, isJudgeAsTraps.get_fastLate);
            JudgeNote_MainField(target, "Trap");

            if (stats.get_name != "NA" && isJudgeAsTraps.get_judge == 4)
            {
                PlayerPrefs.SetInt("MISC_OnTrapsTrigger", 1);
                GameManager.thisManager.UpdateCharacter_Health(-PlayerPrefs.GetInt("Enemy_OverallDamage", 0), false);
                GameManager.thisManager.SpawnDamageIndicator(transform.position, 1, -PlayerPrefs.GetInt("Enemy_OverallDamage", 0));
            }

            if (target.note_index == (int)CharacterSettings.TRAPS_TYPE.AIR) CancelJump();
        }
    }

    private bool GetObstacleHit_Condition(int index)
    {
        if (index == (int)CharacterSettings.TRAPS_TYPE.AIR)
            return !character.IsJumpFinished() && !character.get_AutoFieldDetect;
        else
            return !character.get_AutoFieldDetect;
    }

    private CharacterSettings.TRAPS_TYPE FindAssignIndex(int index)
    {
        switch (index)
        {
            case 1:
                return CharacterSettings.TRAPS_TYPE.GROUND;

            case 2:
                return CharacterSettings.TRAPS_TYPE.AIR;

            default:
                return CharacterSettings.TRAPS_TYPE.NONE;
        }
    }
    #endregion

    #region CHARACTER BASE BEHAVIOUR
    public void CancelAnyActiveAction(CharacterSettings.PICKUP_TYPE note_define, int note_index)
    {
        // Universal: Jump Reset
        if (note_define == CharacterSettings.PICKUP_TYPE.JUMP)
            CancelJump();

        else if (note_define == CharacterSettings.PICKUP_TYPE.NONE && (note_index == 2 || note_index == 3))
            CancelJump();

        // Universal: Attack Reset
        if (note_define != CharacterSettings.PICKUP_TYPE.JUMP)
        {
            // Keyboard Input: Attack Reset
            for (int key = 0; key < character.getInput.getInput_Attack.Length; key++)
                if (character.get_attackActive[key])
                    CancelAttack(key, key);

            // Mouse Input: Attack Reset
            for (int mouseKey = 0; mouseKey < character.getInput.get_mouseInputKey.Length; mouseKey++)
                if (character.get_attackActive[mouseKey] || character.get_AutoFieldDetect) 
                    CancelAttack(mouseKey + character.getInput.getInput_Attack.Length, mouseKey);
        }
    }

    private void PlaySoundEffect(string audio)
    {
        if (audio != string.Empty && PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAudioMute_ValueKey) == 0)
        {
            float soundeffect_volume = PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetSE_ValueKey);
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/" + audio), new Vector3(0, 0, -10f), soundeffect_volume);
        }
    }
    #endregion

    #region CHARACTER JUDGE TIMING BASE
    private void TimingSequenceToAttack(Note_Script note_define, string sound)
    {
        // Judge timing based on attacking status
        JudgeBaseTimingDistribution(note_define, character.GetAttackStatusById());
        JudgeNote_MainField(note_define, sound);

        // Count all targets
        int targetCount = MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget(note_define.note_index);
        MeloMelo_UnitData_Settings.SetSuccessHitOfAllEnemyTarget(targetCount + 1, note_define.note_index);

        // Count any targets
        int anyTargetCount = MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget();
        MeloMelo_UnitData_Settings.SetSuccessHitOfAllEnemyTarget(anyTargetCount + 1);
    }

    private void TimingSequenceForItem(Note_Script note_define, string sound)
    {
        // Judge timing based on non-attacking status
        JudgeBaseTimingDistribution(note_define, character.GetPickUpStatusById());
        JudgeNote_MainField(note_define, sound);

        // Count items
        int currentCount = MeloMelo_UnitData_Settings.GetSuccessPickItem();
        MeloMelo_UnitData_Settings.SetSuccessPickItem(currentCount + 1);
    }

    private void TimingSequenceForSpecialItem(Note_Script note_define)
    {
        // Judge timing based on picking status
        JudgeBaseTimingDistribution(note_define, character.GetSpeicalItemStatusById());
        JudgeNote_MainField(note_define);
    }

    private void TimingSequenceForJump(Note_Script note_define)
    {
        // Judge timing based on juming status
        JudgeBaseTimingDistribution(note_define, character.GetJumpStatusById());
        JudgeNote_MainField(note_define, "Item2");
    }

    private void JudgeBaseTimingDistribution(Note_Script target, JudgeBundle judgePackage)
    {
        int isJudgeApplied = judgePackage.get_judge;
        target.SetNoteCompleted(isJudgeApplied, judgePackage.get_fastLate);
    }
    #endregion

    #region CHARACTER REWARDS
    private void IncludePerfromanceScore(float _score)
    {
        GameManager.thisManager.FinalScoreMultipler(_score);
        //if (PlayerPrefs.GetInt("MissCP", 0) > 0) scoreF.score_combo();
    }

    private void IncludeTechnicalScore(Note_Script target)
    {
        bool isAttackAreReflectable = target.note_define_index == CharacterSettings.PICKUP_TYPE.NONE && target.note_index == 3;
        int bonusScoring = isAttackAreReflectable ? 2 : 1;

        GameManager.thisManager.UpdateScore_Tech((PlayerPrefs.GetInt("Character_OverallPower", 0) / 3) * bonusScoring);
        if (isAttackAreReflectable) target.GetComponent<Notation_Motion_Script>().Reflect_MoveEffect();
    }

    private void DamagingEnemyProgress(float multiple, bool condition)
    {
        if (condition)
        {
            GameManager.thisManager.UpdateBattle_Progress((float)100 / GameManager.thisManager.getGameplayComponent.getTotalEnemy * multiple);
            GameManager.thisManager.UpdateEnemy_Health(-PlayerPrefs.GetInt("Character_OverallDamage", 0) - MeloMelo_ExtraStats_Settings.GetBonusDamage(), false);
            GameManager.thisManager.SpawnDamageIndicator(transform.position, 2,
                -PlayerPrefs.GetInt("Character_OverallDamage", 0) - MeloMelo_ExtraStats_Settings.GetBonusDamage());

            // Prompt basic attack information
            GameManager.thisManager.gameObject.GetComponent<SkillManager>().PromptCharacterBaseDamage("Basic Attack",
                -PlayerPrefs.GetInt("Character_OverallDamage", 0), -MeloMelo_ExtraStats_Settings.GetBonusDamage());
            stats.WEXP_input();
        }
    }

    private void IncludeCharacterHealing()
    {
        if (stats.get_name != "NA")
        {
            int healing_value = 10;
            StatsDistribution stats = new StatsDistribution();
            stats.load_Stats();

            foreach (ClassBase character in stats.slot_Stats)
            {
                int originalValue = healing_value * (character.magic + MeloMelo_ExtraStats_Settings.GetExtraMagicStats(character.name));
                healing_value += originalValue + MeloMelo_ItemUsage_Settings.GetPowerBoost(character.name) +
                    originalValue * MeloMelo_ItemUsage_Settings.GetPowerBoostByMultiply(character.name);
            }

            GameManager.thisManager.UpdateCharacter_Health(healing_value, false);
            GameManager.thisManager.SpawnDamageIndicator(transform.position, 1, healing_value);
        }
    }
    #endregion

    #region CHARACTER HIT IDENTIFICATION
    private int GetPickUp_Type(CharacterSettings.PICKUP_TYPE define)
    {
        switch (define)
        {
            case CharacterSettings.PICKUP_TYPE.ITEM:
                return (int)CharacterSettings.PICKUP_TYPE.ITEM;

            case CharacterSettings.PICKUP_TYPE.JUMP:
                return (int)CharacterSettings.PICKUP_TYPE.JUMP;

            case CharacterSettings.PICKUP_TYPE.ITEM3:
                return (int)CharacterSettings.PICKUP_TYPE.ITEM3;

            default:
                return (int)CharacterSettings.PICKUP_TYPE.NONE;
        }
    }

    private bool IsPickUpPossible(CharacterSettings.PICKUP_TYPE note_type)
    {
        if (GetPickUp_Type(note_type) > 0) return true;
        else return false;
    }

    public bool IsObstacleDodgePossible(CharacterSettings.PICKUP_TYPE note_type)
    {
        if (note_type == CharacterSettings.PICKUP_TYPE.TRAP) return true;
        else return false;
    }

    private bool IsAttackInputReceived()
    {
        if (!character.IsAttackFinished())
            return true;
        else 
            return false;
    }
    #endregion

    #region AUTO DETECT TOOLS
    private void SimulateHitTarget()
    {
        if (character.IsAttackFinished())
        {
            if (!isAttacking[0]) PerformAttack(2, 0);
            else if (!isAttacking[1]) PerformAttack(3, 1);
        }
    }

    private void SimulateJump()
    {
        if (!isJumping && character.IsJumpFinished())
        {
            isJumping = true;
            PerformJump();
        }
    }
    #endregion

    //Warrior Ability
    #region RPG: Content (NOT_IN_USE)
    public void abilitycharge()
    {
        if (PlayerPrefs.GetString("CharacterFront", "NA") == "Warrior" && stats.get_NumberofnotesHitW >= 25 && PlayerPrefs.GetString("AutoSkillOption", "F") == "T")
        {
            stats.NumberofnotesHitW_input(PartyStatus.transform.GetChild(1).GetChild(1).gameObject, true);
            //GameObject BarrierInt = Instantiate(Barrier, new Vector3(0, 0.4f, -7.8f), Quaternion.identity);
            Barrier.SetActive(true);
            Invoke("DestroyBarrier", 3);
        }
    }

    void DestroyBarrier() { Barrier.SetActive(false); }
    #endregion
}