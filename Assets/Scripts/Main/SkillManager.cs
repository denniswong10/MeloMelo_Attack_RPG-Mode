using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EffectTypeBundle
{
    public int instance;
    public string effectName;
    public string skillName;
    public int activationKey;
    public bool effectOnCondtion;
    public bool effectOnAction;
    public string valueOfTrigger;
}

[System.Serializable]
public class EffectToggleData
{
    public string skillName;
    public string description;
}

public static class SkillManager_Properties
{
    #region EFFECT DATA
    public static void SetEffectName(string effectName) { PlayerPrefs.SetString("Skill_EffectName", effectName); }
    public static void SetEffectBaseDamage(string effectKey, int baseValue) { PlayerPrefs.SetInt(effectKey + "_BaseDamage", baseValue); }
    public static void SetEffectExtraPercentage(string effectKey, int bonusValue) { PlayerPrefs.SetInt(effectKey + "_ExtraDamagePercentage", bonusValue); }
    public static void SetEffectValueOfTrigger(int instance, string effectUsage, string value) 
    { PlayerPrefs.SetString(instance + "_ValueOfTrigger_" + effectUsage, value); }

    public static string GetEffectName() { return PlayerPrefs.GetString("Skill_EffectName", string.Empty); }
    public static int GetEffectBaseDamage(string effectKey) { return PlayerPrefs.GetInt(effectKey + "_BaseDamage", 0); }
    public static int GetEffectExtraPercentage(string effectKey) { return PlayerPrefs.GetInt(effectKey + "_ExtraDamagePercentage", 0); }
    public static string GetEffectValueOfTrigger(int instance, string effectUsage) 
    { return PlayerPrefs.GetString(instance + "_ValueOfTrigger_" + effectUsage, string.Empty); }
    #endregion

    #region EFFECT PROCESSOR
    public static void SetEffectIsBusy(int phaseId, bool isBusy) { PlayerPrefs.SetInt(phaseId + "_EffectCurrentlyInProcess", isBusy ? 1 : 0); }
    public static bool GetEffectOnBusy(int phaseId) { return PlayerPrefs.GetInt(phaseId + "_EffectCurrentlyInProcess", 0) == 1 ? true : false; }

    public static void SetEffectLimit(string effectKey, int value) { PlayerPrefs.SetInt(effectKey + "_Limit", value); }
    public static void SetActiveInstance(int value) { PlayerPrefs.SetInt("Effect_Instance_Id", value); }
    public static void SetTotalActiveInstance(int phaseId, int value) { PlayerPrefs.SetInt("Effect_TotalInstance_Id_" + phaseId, value); }
    public static void SetEffectCondition(bool condition) { PlayerPrefs.SetInt("Effect_Condition_Status", condition ? 1 : 0); }

    public static int GetEffectLimit(string effectKey) { return PlayerPrefs.GetInt(effectKey + "_Limit", 1); }
    public static int GetActiveInstance() { return PlayerPrefs.GetInt("Effect_Instance_Id", 0); }
    public static int GetTotalActiveInstance(int phaseId) { return PlayerPrefs.GetInt("Effect_TotalInstance_Id_" + phaseId, 0); }
    public static bool GetEffectCondition() { return PlayerPrefs.GetInt("Effect_Condition_Status", 0) == 1 ? true : false; }
    #endregion

    #region EFFECT MODIFICATION
    public static void SetEffectIndicator(string effectKey, string indicate)
    { GameObject.Find(effectKey).transform.GetChild(0).GetComponent<Text>().text = indicate; }
    public static void SetEffectColorEffect(string effectKey, Color color_id)
    { GameObject.Find(effectKey).GetComponent<RawImage>().color = color_id; }
    #endregion
}

public class SkillManager : MonoBehaviour
{
    private List<EffectTypeBundle> onStartOfTrackEffects;
    private List<EffectTypeBundle> onEndOfTrackEffects;
    private List<EffectTypeBundle> onTrackEffects;

    [SerializeField] private GameObject allEffectIndicator;
    [SerializeField] private GameObject skillHolder;
    [SerializeField] private Texture[] skillIconNone;
    [SerializeField] private GameObject SkillIndicator;

    private List<EffectToggleData> effectToggleData;

    void Start()
    {
        if (IsSkillOnActive() && !PlayerPrefs.HasKey("MarathonPermit"))
        {
            onStartOfTrackEffects = new List<EffectTypeBundle>();
            onEndOfTrackEffects = new List<EffectTypeBundle>();
            onTrackEffects = new List<EffectTypeBundle>();
            effectToggleData = new List<EffectToggleData>();

            skillHolder.SetActive(true);
        }
    }

    void Update()
    {
        OnEffectUpdate(1, BeatConductor.thisBeat.gameObject.GetComponent<AudioSource>().isPlaying);

        if (effectToggleData != null && effectToggleData.ToArray().Length > 0)
        {
            foreach (EffectToggleData _dat in effectToggleData)
            {
                if (!SkillIndicator.activeInHierarchy)
                {
                    StartCoroutine(GetSkillActiveAlert(_dat.skillName, _dat.description));
                    effectToggleData.Remove(_dat);
                    break;
                }
            }
        }
    }

    #region SETUP (Effect Manipluate)
    public void ExtractSkill(SkillContainer skill, ClassBase caster)
    {
        // Extract skill and convert into effect type
        if (skill.customEffectData != null)
        {
            // Get the current phase where effect is needed to be added
            for (int phaseCount = 0; phaseCount < (int)EffectDataSettings.ActivePhase.OnEnd + 1; phaseCount++)
            {
                // From the current phase add everything to the list which contain the condition then the action
                for (int unloadAndModify = 0; unloadAndModify < skill.customEffectData.Length; unloadAndModify++)
                {
                    if ((int)skill.customEffectData[unloadAndModify].effectActivationPhase == phaseCount)
                    {
                        EffectTypeBundle effectBundle = new EffectTypeBundle();
                        effectBundle.instance = SkillManager_Properties.GetActiveInstance() + 1;
                        effectBundle.effectName = skill.customEffectData[unloadAndModify].effectName;
                        effectBundle.activationKey = phaseCount;

                        // Write effect information for condition
                        foreach (EffectConditionData conditionData in skill.customEffectData[unloadAndModify].effectOnCondition)
                        {
                            EffectTypeBundle effectBundle_onCondition = new EffectTypeBundle();
                            effectBundle_onCondition.instance = effectBundle.instance;
                            effectBundle_onCondition.effectName = effectBundle.effectName;
                            effectBundle_onCondition.activationKey = effectBundle.activationKey;
                            effectBundle_onCondition.effectOnCondtion = true;
                            effectBundle_onCondition.effectOnAction = false;
                            effectBundle_onCondition.valueOfTrigger = conditionData.effectCondition + "," + conditionData.valueOfTrigger;

                            // Add following condition type to list
                            AddingEffectToList(effectBundle_onCondition.activationKey, effectBundle_onCondition);
                        }

                        // Write effect information for action
                        int actionArray = 0;
                        foreach (EffectActionData actionData in skill.customEffectData[unloadAndModify].effectOnAction)
                        {
                            EffectTypeBundle effectBundle_onAction = new EffectTypeBundle();
                            ElemetStartingStats baseStats = MeloMelo_GameSettings.GetStatsWithElementBonus(
                                caster.elementType == ClassBase.ElementStats.Earth ? "Earth" :
                                caster.elementType == ClassBase.ElementStats.Light ? "Light" :
                                caster.elementType == ClassBase.ElementStats.Dark ? "Dark" : "None");

                            effectBundle_onAction.instance = effectBundle.instance;
                            effectBundle_onAction.effectName = effectBundle.effectName;
                            effectBundle_onAction.skillName = skill.skillName;
                            effectBundle_onAction.activationKey = effectBundle.activationKey;
                            effectBundle_onAction.effectOnCondtion = false;
                            effectBundle_onAction.effectOnAction = true;
                            effectBundle_onAction.valueOfTrigger = actionData.effectActionName + "," + actionData.baseValue + "," + actionData.extraPercentage + "," +

                                ((skill.customEffectData[unloadAndModify].effectOnAction[actionArray].effectMainStats == EffectActionData.EffectActionStats.STR ?
                                caster.strength * baseStats.strength :
                                skill.customEffectData[unloadAndModify].effectOnAction[actionArray].effectMainStats == EffectActionData.EffectActionStats.VIT ?
                                caster.vitality * baseStats.vitality : caster.magic * baseStats.magic) + baseStats.strength)

                                 + "," + (MeloMelo_SkillData_Settings.CheckSkillGrade(skill.skillName) > 0 ? 
                                 MeloMelo_SkillData_Settings.CheckSkillGrade(skill.skillName) : 1);

                            // Add following condition and action type to list
                            actionArray++;
                            AddingEffectToList(effectBundle_onAction.activationKey, effectBundle_onAction);
                        }

                        // Raise instance by 1 indicate the effect is done
                        SkillManager_Properties.SetActiveInstance(effectBundle.instance + 1);
                    }
                }

                // Reset instance and store total instance count
                SkillManager_Properties.SetTotalActiveInstance(phaseCount, SkillManager_Properties.GetActiveInstance());
                Debug.Log("Total Active Effect (" + phaseCount + "): " + SkillManager_Properties.GetTotalActiveInstance(phaseCount));
                SkillManager_Properties.SetActiveInstance(0);
            }        
        }

        // Reset all existing effect data
        if (skill.customEffectData != null)
        {
            OnResetStatusEffect(onStartOfTrackEffects.ToArray());
            OnResetStatusEffect(onEndOfTrackEffects.ToArray());
            OnResetStatusEffect(onTrackEffects.ToArray());
        }
    }

    private void AddingEffectToList(int phaseId, EffectTypeBundle data)
    {
        switch (phaseId)
        {
            case 1:
                onTrackEffects.Add(data);
                break;

            case 2:
                onEndOfTrackEffects.Add(data);
                break;

            default:
                onStartOfTrackEffects.Add(data);
                break;
        }

        //Debug.Log("Added format (" + (data.effectOnCondtion ? "Condition" : data.effectOnAction ? "Action" : "???") + "): " + 
            //JsonUtility.ToJson(data));
    }

    private EffectTypeBundle[] GetEffectFromList(int id)
    {
        switch (id)
        {
            case 1:
                if (onTrackEffects != null) return onTrackEffects.ToArray();
                break;

            case 2:
                if (onEndOfTrackEffects != null) return onEndOfTrackEffects.ToArray();
                break;

            default:
                if (onStartOfTrackEffects != null) return onStartOfTrackEffects.ToArray();
                break;
        }

        return null;
    }
    #endregion

    #region SETUP
    public void RegisterForSkillUsage(SkillContainer skill, bool isPrimary)
    {
        // Update skill icon for displaying
        skillHolder.transform.GetChild(isPrimary ? 1 : 2).GetComponent<RawImage>().texture = skill.skillIcon;
        Debug.Log((isPrimary ? 1 : 2) + ": " + skill.skillName + " --> Register Completed!");
    }

    private void OnResetStatusEffect(EffectTypeBundle[] effectForReset)
    {
        foreach (EffectTypeBundle effect in effectForReset)
        {
            PlayerPrefs.DeleteKey(effect.effectName + "_Limit");
            ClearBuffEffect(effect.effectName);
        }
    }
    #endregion

    #region MAIN
    public void OnEffectUpdate(int phase_index, bool inCondition)
    {
        if (GetEffectFromList(phase_index) != null && GetEffectFromList(phase_index).Length > 0)
        {
            if (!SkillManager_Properties.GetEffectOnBusy(phase_index))
            {
                SkillManager_Properties.SetEffectIsBusy(phase_index, true);

                // Update this effect only when the game is still on-going
                if (inCondition)
                {
                    for (int instance = 1; instance < GetTotalNumberOfInstance(phase_index) + 1; instance++)
                    {
                        foreach (EffectTypeBundle effect in onTrackEffects)
                        {
                            if (effect.effectOnCondtion && !effect.effectOnAction && effect.instance == instance)
                            {
                                // Format: ConditionName, Value
                                string[] conditionOfData = effect.valueOfTrigger.Split(",");
                                SkillManager_Properties.SetEffectName(effect.effectName);
                                SkillManager_Properties.SetActiveInstance(instance);
                                SkillManager_Properties.SetEffectValueOfTrigger(instance, "Condition", effect.valueOfTrigger);

                                CreateBuffEffect(effect.effectName);
                                Invoke(conditionOfData[0], 0);
                                //Debug.Log("Instance: " + instance + " | OnCondition: " + conditionOfData[0] + " | ByEffect: " + effect.effectName);
                            }
                        }

                        foreach (EffectTypeBundle effect in onTrackEffects)
                        {
                            if (instance == effect.instance && effect.effectOnAction && SkillManager_Properties.GetEffectCondition())
                            {
                                // Format: ActionName, Base, Pecentage, MainStats
                                string[] actionOfData = effect.valueOfTrigger.Split(",");
                                SkillManager_Properties.SetEffectValueOfTrigger(instance, "Action", effect.valueOfTrigger);
                                PlayerPrefs.SetString("CurrentSkill_Active", effect.skillName);
                                Invoke(actionOfData[0], 0);
                                //Debug.Log("Instance: " + instance + " | OnAction: " + actionOfData[0] + " | ByEffect: " + effect.effectName);
                            }
                        }

                        //Debug.Log((phase_index == 1 ? "On-Track Play" : phase_index == 0 ? "Start of Track" : "End of Track") +
                            //" [End of Instance]: " + instance);
                    }
                }

                // Otherwise this effect will not be active again
                else
                {
                    for (int instance = 0; instance < GetTotalNumberOfInstance(phase_index); instance++)
                    {
                        foreach (EffectTypeBundle effect in onTrackEffects)
                        {
                            ClearBuffEffect(effect.effectName);
                            break;
                        }
                    }
                }

                SkillManager_Properties.SetEffectIsBusy(phase_index, false);
            }
        }
        //else
            //Debug.Log((phase_index == 1 ? "On-Track Play" : phase_index == 0 ? "Start of Track" : "End of Track") + " [Effect]: Not Active");
    }
    #endregion

    #region COMPONENT (CREATE BUFF INSTANCE)
    private void CreateBuffEffect(string effectName)
    {
        if (GetInstantEffectIcon(effectName) != null && !PlayerPrefs.HasKey(effectName + "_Buffer"))
        {
            PlayerPrefs.SetInt(effectName + "_Buffer", 1);
            RawImage icon = Instantiate(GetInstantEffectIcon(effectName));
            icon.name = effectName;
            icon.transform.SetParent(allEffectIndicator.transform);
        }
    }

    private void ClearBuffEffect(string effectName)
    {
        // Remove buff effect icon
        PlayerPrefs.DeleteKey(effectName + "_Buffer");
        Destroy(GameObject.Find(effectName));
    }

    private RawImage GetInstantEffectIcon(string effectName)
    {
        RawImage isBuffIconEnable = Resources.Load<RawImage>("Database_Buffs_Effect/" + effectName);
        if (isBuffIconEnable) return isBuffIconEnable;
        else return null;
    }
    #endregion

    #region SKILL EFFECT (PRIMARY)
    //private void OnComboDamage()
    //{
    //    const string effectKey = "OnComboDamage";
    //    int countReachLimit = GameManager.thisManager.getJudgeWindow.getCombo == 0 ? 1 : SkillManager_Properties.GetEffectLimit(effectKey);
    //    SkillManager_Properties.SetEffectLimit(effectKey, countReachLimit);
    //    SkillManager_Properties.SetEffectIndicator(effectKey, (SkillManager_Properties.GetEffectLimit(effectKey) - 1).ToString());

    //    // On Activation upon reached a certain combo
    //    if (GameManager.thisManager.getJudgeWindow.getCombo >= countReachLimit * int.Parse(SkillManager_Properties.GetEffectValueOfTrigger(effectKey)))
    //    {
    //        ModifyOfEnemyHealth(-SkillManager_Properties.GetEffectBaseDamage(effectKey), -SkillManager_Properties.GetEffectExtraPercentage(effectKey),
    //            effectKey
    //            );

    //        PromptDamageIndicator(countReachLimit, effectKey);
    //    }
    //}

    //private void OnCountdownShield()
    //{
    //    const string effectKey = "OnCountdownShield";
    //    if (!PlayerPrefs.HasKey(effectKey + "_Limit"))
    //        SkillManager_Properties.SetEffectLimit(effectKey, int.Parse(SkillManager_Properties.GetEffectValueOfTrigger(effectKey)));
    //    SkillManager_Properties.SetEffectIndicator(effectKey, Time.time >= SkillManager_Properties.GetEffectLimit(effectKey) ? string.Empty :
    //         (SkillManager_Properties.GetEffectLimit(effectKey) - Time.time).ToString("0"));

    //    // On Activation upon character do not have damage resistance in every given seconds
    //    if (Time.time >= SkillManager_Properties.GetEffectLimit(effectKey))
    //    {
    //        // Giving out damage resistance and set timer
    //        SkillManager_Properties.SetEffectLimit(effectKey, (int)(Time.time + int.Parse(SkillManager_Properties.GetEffectValueOfTrigger(effectKey))));
    //        GivingOutCharacterDamageResistance(SkillManager_Properties.GetEffectBaseDamage(effectKey), SkillManager_Properties.GetEffectExtraPercentage(effectKey));
    //        PromptDamageResistanceIndicator((int)Time.time, effectKey);
    //    }

    //    else
    //    {
    //        if (PlayerPrefs.GetInt("MISC_Character_DamageResist", 0) > 0) SkillManager_Properties.SetEffectColorEffect(effectKey, Color.white);
    //        else SkillManager_Properties.SetEffectColorEffect(effectKey, Color.grey);
    //    }
    //}

    //private void OnItemCountForDamage()
    //{
    //    const string effectKey = "OnItemCountForDamage";
    //    if (!PlayerPrefs.HasKey(effectKey + "_Limit")) MeloMelo_UnitData_Settings.SetSuccessPickItem(0);

    //    int countReachLimit = SkillManager_Properties.GetEffectLimit(effectKey);
    //    SkillManager_Properties.SetEffectIndicator(effectKey, MeloMelo_UnitData_Settings.GetSuccessPickItem().ToString());

    //    // On Activation upon reached a certain item picked
    //    if (MeloMelo_UnitData_Settings.GetSuccessPickItem() > int.Parse(SkillManager_Properties.GetEffectValueOfTrigger(effectKey)))
    //    {
    //        MeloMelo_UnitData_Settings.SetSuccessPickItem(0);
    //        ModifyOfEnemyHealth(-SkillManager_Properties.GetEffectBaseDamage(effectKey), -SkillManager_Properties.GetEffectExtraPercentage(effectKey),
    //                effectKey);
    //        PromptDamageIndicator(countReachLimit, effectKey);
    //    }

    //    // On Active: Charged!
    //    if (MeloMelo_UnitData_Settings.GetSuccessPickItem() >= int.Parse(SkillManager_Properties.GetEffectValueOfTrigger(effectKey))) 
    //        SkillManager_Properties.SetEffectColorEffect(effectKey, Color.white);
    //    else 
    //        SkillManager_Properties.SetEffectColorEffect(effectKey, Color.grey);
    //}

    //private void OnSucessHitBonusDamage()
    //{
    //    const string effectKey = "OnSucessHitBonusDamage";
    //    if (!PlayerPrefs.HasKey(effectKey + "_Limit")) OnResetTargetCounted();

    //    int countReachLimit = SkillManager_Properties.GetEffectLimit(effectKey);
    //    int targetCounted = MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget(1) + MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget(6) +
    //        MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget(5);
    //    SkillManager_Properties.SetEffectIndicator(effectKey, targetCounted.ToString());

    //    if (targetCounted >= countReachLimit * int.Parse(SkillManager_Properties.GetEffectValueOfTrigger(effectKey)))
    //    {
    //        int currentBaseDamage = MeloMelo_ExtraStats_Settings.GetBonusDamage();
    //        GivingOutCharacterBonusBaseDamage(SkillManager_Properties.GetEffectBaseDamage(effectKey), currentBaseDamage + 
    //            SkillManager_Properties.GetEffectExtraPercentage(effectKey), effectKey);

    //        int currentHitStack = MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget();
    //        MeloMelo_UnitData_Settings.SetSuccessHitOfAllEnemyTarget(currentHitStack + 1);
    //    }
    //}

    //private void OnStackSuccessDamage()
    //{
    //    const string effectKey = "OnStackSuccessDamage";
    //    if (!PlayerPrefs.HasKey(effectKey + "_Limit")) MeloMelo_UnitData_Settings.SetSuccessHitOfAllEnemyTarget(0);
    //    SkillManager_Properties.SetEffectLimit(effectKey, 1);
    //    SkillManager_Properties.SetEffectIndicator(effectKey, MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget().ToString());

    //    // On Active: Charged!
    //    if (MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget() > int.Parse(SkillManager_Properties.GetEffectValueOfTrigger(effectKey)))
    //    {
    //        MeloMelo_UnitData_Settings.SetSuccessHitOfAllEnemyTarget(0);
    //        ModifyOfEnemyHealth(-SkillManager_Properties.GetEffectBaseDamage(effectKey), -SkillManager_Properties.GetEffectExtraPercentage(effectKey), effectKey);
    //        PromptDamageIndicator(-3, effectKey);
    //    }

    //    SkillManager_Properties.SetEffectColorEffect(effectKey,
    //        MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget() >= int.Parse(SkillManager_Properties.GetEffectValueOfTrigger(effectKey)) ?
    //        Color.white : Color.grey);
    //}
    #endregion

    #region SKILL EFFECT (SECONDARY)
    private void OnScoreIncreaseBonusTech()
    {
        // On Activation upon reached a certain score
        if (GameManager.thisManager.get_score1.get_score >= int.Parse(PlayerPrefs.GetString("OnScoreIncreaseBonusTech_ValueOfTrigger", string.Empty)))
            // Adding bonus tech score
            GameManager.thisManager.get_score2.ModifyScore(PlayerPrefs.GetInt("OnScoreIncreaseBonusTech_BaseDamage", 0) +
                PlayerPrefs.GetInt("OnScoreIncreaseBonusTech_ExtraDamagePercentage", 0));
    }

    private void OnScoreDamageEnemyHealth()
    {
        // On Activation upon reached a certain score
        if (GameManager.thisManager.get_score1.get_score >= int.Parse(PlayerPrefs.GetString("OnScoreDamageEnemyHealth_ValueOfTrigger", string.Empty)))
            // Dealing damage to enemy health
            GameManager.thisManager.UpdateEnemy_Health(PlayerPrefs.GetInt("OnScoreDamageEnemyHealth_BaseDamage", 0) +
                PlayerPrefs.GetInt("OnScoreDamageEnemyHealth_ExtraDamagePercentage", 0), false);
    }

    private void OnSuddenDamageEnemyHealth()
    {
        // On Activation upon randomize a number
        if (Random.Range(0, 100) >= int.Parse(PlayerPrefs.GetString("OnSuddenDamageEnemyHealth_ValueOfTrigger", string.Empty)))
            // Dealing damage to enemy health
            GameManager.thisManager.UpdateEnemy_Health(PlayerPrefs.GetInt("OnSuddenDamageEnemyHealth_BaseDamage", 0) +
               PlayerPrefs.GetInt("OnSuddenDamageEnemyHealth_ExtraDamagePercentage", 0), false);
    }

    private void OnJudgeForBaseDamageBuff()
    {
        // On Activation upon achieve a certain judge timing

    }

    private void OnResistOfDamageTaken()
    {

    }

    private void GivingOutInstantSheildProctector()
    {

    }
    #endregion

    #region SKILL EFFECT (DEBUFF)
    private void OnDebuffBonusDamage()
    {
        // Reset the bonus damage to 0
        MeloMelo_ExtraStats_Settings.GiveOutBonusBaseDamage(0);
    }

    private void OnResetTargetCounted()
    {
        MeloMelo_UnitData_Settings.SetSuccessHitOfAllEnemyTarget(0, 1);
        MeloMelo_UnitData_Settings.SetSuccessHitOfAllEnemyTarget(0, 2);
        MeloMelo_UnitData_Settings.SetSuccessHitOfAllEnemyTarget(0, 3);
        MeloMelo_UnitData_Settings.SetSuccessHitOfAllEnemyTarget(0, -1);
    }

    private void OnResetTrapBeenTrigger()
    {
        PlayerPrefs.DeleteKey("MISC_OnTrapsTrigger");
    }

    private void OnBuffInstantSheild()
    {
        CreateInstanceOfSkillInfo(PlayerPrefs.GetString("CurrentSkill_Active", string.Empty),
            "Characater gain shield");
    }
    #endregion

    #region COMPONENT (CONDITION LIST)
    private void OnComboCount()
    {
        // Format: ConditionName, Number Of Combo
        string[] onComboTriggerData = SkillManager_Properties.GetEffectValueOfTrigger(SkillManager_Properties.GetActiveInstance(), "Condition").Split(",");
        int countLimitBreak = SkillManager_Properties.GetEffectLimit(SkillManager_Properties.GetEffectName());
        Text indicator = GameObject.Find(SkillManager_Properties.GetEffectName()).transform.GetChild(0).GetComponent<Text>();

        // Display: Raise value by every number of combo
        if (indicator)
        {
            indicator.text = (countLimitBreak - 1).ToString();

            SkillManager_Properties.SetEffectCondition(GameManager.thisManager.getJudgeWindow.getCombo >= countLimitBreak * int.Parse(onComboTriggerData[1]));
            if (countLimitBreak > 0 && GameManager.thisManager.getJudgeWindow.getCombo == 0)
                SkillManager_Properties.SetEffectLimit(SkillManager_Properties.GetEffectName(), 1);

            if (GameManager.thisManager.getJudgeWindow.getCombo >= countLimitBreak * int.Parse(onComboTriggerData[1]))
                SkillManager_Properties.SetEffectLimit(SkillManager_Properties.GetEffectName(), countLimitBreak + 1);
        }
    }

    private void OnTargetCount()
    {
        // Format: ConditionName, Number Of Hits
        string[] onTargetCountedTriggerData = SkillManager_Properties.GetEffectValueOfTrigger(SkillManager_Properties.GetActiveInstance(), "Condition").Split(",");
        int countLimitBreak = SkillManager_Properties.GetEffectLimit(SkillManager_Properties.GetEffectName());
        Text indicator = GameObject.Find(SkillManager_Properties.GetEffectName()).transform.GetChild(0).GetComponent<Text>();

        // Display: All Target Hit Counted
        if (indicator)
        {
            indicator.text = MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget().ToString();

            SkillManager_Properties.SetEffectCondition(MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget() >=
                countLimitBreak * int.Parse(onTargetCountedTriggerData[1]));
            if (MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget() >= countLimitBreak * int.Parse(onTargetCountedTriggerData[1]))
                SkillManager_Properties.SetEffectLimit(SkillManager_Properties.GetEffectName(), countLimitBreak + 1);
        }
    }

    private void OnCountStackTarget()
    {
        // Format: ConditionName, NumberOfTarget, TargetType
        string[] onTargetStackTriggerData = SkillManager_Properties.GetEffectValueOfTrigger(SkillManager_Properties.GetActiveInstance(), "Condition").Split(",");
        string[] typeOfTarget = onTargetStackTriggerData[2].Split("^");
        GameObject indicator = GameObject.Find(SkillManager_Properties.GetEffectName());

        // Display: Total Target Hit Counted
        if (indicator)
        {
            // Total up all targets
            int totalTargetCount = 0;
            foreach (string target in typeOfTarget) totalTargetCount += MeloMelo_UnitData_Settings.GetSuccessHitOfAllEnemyTarget(int.Parse(target));
            indicator.transform.GetChild(0).GetComponent<Text>().text = totalTargetCount.ToString();

            // Enable icon to show clear in this condition
            if (totalTargetCount > int.Parse(onTargetStackTriggerData[1])) indicator.GetComponent<RawImage>().color = Color.white;
            else indicator.GetComponent<RawImage>().color = Color.grey;

            // Allow to perform action
            SkillManager_Properties.SetEffectCondition(totalTargetCount > int.Parse(onTargetStackTriggerData[1]));
            if (totalTargetCount >= int.Parse(onTargetStackTriggerData[1])) 
                foreach (string target in typeOfTarget) MeloMelo_UnitData_Settings.SetSuccessHitOfAllEnemyTarget(0, int.Parse(target));
        }
    }

    private void OnItemCount()
    {
        // Format: ConditionName, Number Of Items
        string[] onItemTriggerData = SkillManager_Properties.GetEffectValueOfTrigger(SkillManager_Properties.GetActiveInstance(), "Condition").Split(",");
        GameObject indicator = GameObject.Find(SkillManager_Properties.GetEffectName());

        // Display: Total count required
        if (indicator)
        {
            indicator.transform.GetChild(0).GetComponent<Text>().text = MeloMelo_UnitData_Settings.GetSuccessPickItem().ToString();
            if (MeloMelo_UnitData_Settings.GetSuccessPickItem() > int.Parse(onItemTriggerData[1])) indicator.GetComponent<RawImage>().color = Color.white;
            else indicator.GetComponent<RawImage>().color = Color.grey;

            // Allow to perform action
            SkillManager_Properties.SetEffectCondition(MeloMelo_UnitData_Settings.GetSuccessPickItem() > int.Parse(onItemTriggerData[1]));
            if (MeloMelo_UnitData_Settings.GetSuccessPickItem() >= int.Parse(onItemTriggerData[1])) MeloMelo_UnitData_Settings.SetSuccessPickItem(0);
        }
    }

    private void OnTimerCount()
    {
        // Format: ConditionName, Countdown Time
        string[] onCountdownTriggerData = SkillManager_Properties.GetEffectValueOfTrigger(SkillManager_Properties.GetActiveInstance(), "Condition").Split(",");
        if (!PlayerPrefs.HasKey(SkillManager_Properties.GetEffectName() + "_Limit")) SkillManager_Properties.SetEffectLimit(SkillManager_Properties.GetEffectName(),
            (int)Time.time + int.Parse(onCountdownTriggerData[1]));
        GameObject indicator = GameObject.Find(SkillManager_Properties.GetEffectName());

        // Display: Countdown timer
        if (indicator)
        {
            indicator.transform.GetChild(0).GetComponent<Text>().text =
                (SkillManager_Properties.GetEffectLimit(SkillManager_Properties.GetEffectName()) - Time.time).ToString("0");

            // Allow to perform action
            SkillManager_Properties.SetEffectCondition(Time.time >= SkillManager_Properties.GetEffectLimit(SkillManager_Properties.GetEffectName()));
            if (Time.time >= SkillManager_Properties.GetEffectLimit(SkillManager_Properties.GetEffectName()))
                SkillManager_Properties.SetEffectLimit(SkillManager_Properties.GetEffectName(), (int)Time.time + int.Parse(onCountdownTriggerData[1]));
        }
    }

    private void OnJudgeCount()
    {
        // Format: ConditionName, Number Of Judge, Judge Index
        string[] onJudgeCountTriggerData = SkillManager_Properties.GetEffectValueOfTrigger(SkillManager_Properties.GetActiveInstance(), "Condition").Split(",");
        int countLimit = SkillManager_Properties.GetEffectLimit(SkillManager_Properties.GetEffectName());
        Text indicator = GameObject.Find(SkillManager_Properties.GetEffectName()).transform.GetChild(0).GetComponent<Text>();
        int typeOfJudge = int.Parse(onJudgeCountTriggerData[2]) == 1 ? GameManager.thisManager.getJudgeWindow.get_perfect2 : int.Parse(onJudgeCountTriggerData[2]) == 2
            ? GameManager.thisManager.getJudgeWindow.get_perfect : int.Parse(onJudgeCountTriggerData[2]) == 3 ? GameManager.thisManager.getJudgeWindow.get_bad :
             GameManager.thisManager.getJudgeWindow.get_miss;

        // Display: Judge Counted
        if (indicator)
        {
            indicator.text = (countLimit - 1).ToString();

            // Allow to perform action
            bool isJudgeTrigger = typeOfJudge >= int.Parse(onJudgeCountTriggerData[1]) * SkillManager_Properties.GetEffectLimit(SkillManager_Properties.GetEffectName());
            SkillManager_Properties.SetEffectCondition(isJudgeTrigger);

            if (isJudgeTrigger)
                SkillManager_Properties.SetEffectLimit(SkillManager_Properties.GetEffectName(), countLimit + 1);
        }
    }

    private void OnTrapsTrigger()
    {
        SkillManager_Properties.SetEffectCondition(PlayerPrefs.HasKey("MISC_OnTrapsTrigger"));
        if (PlayerPrefs.HasKey("MISC_OnTrapsTrigger")) PlayerPrefs.DeleteKey("MISC_OnTrapsTrigger");
    }

    private void OnChanceCondition()
    {
        // Format: ConditionName, Percentage of Chance
        int randomRange = Random.Range(0, 100);
        bool cooldownTime = Time.time >= SkillManager_Properties.GetEffectLimit(SkillManager_Properties.GetEffectName());
        string[] onChanceTrigger = SkillManager_Properties.GetEffectValueOfTrigger(SkillManager_Properties.GetActiveInstance(), "Condition").Split(",");
        Text indicator = GameObject.Find(SkillManager_Properties.GetEffectName()).transform.GetChild(0).GetComponent<Text>();

        if (indicator)
        {
            // Allow to perform action
            if (cooldownTime)
            {
                indicator.text = (100 - randomRange).ToString();
                SkillManager_Properties.SetEffectCondition(100 - randomRange >= int.Parse(onChanceTrigger[1]));
                SkillManager_Properties.SetEffectLimit(SkillManager_Properties.GetEffectName(), (int)Time.time + 30);
            }
        }
    }

    private void OnEmptyConditionRun()
    {
        SkillManager_Properties.SetEffectCondition(true);
    }
    #endregion

    #region COMPONENT (ACTION LIST)
    private void OnDamageEnemyHealth()
    {
        // Deal direct damage to enemy unit
        int allDamage = GetEffectCalculatedValue(1) + GetEffectCalculatedValue(2) - PlayerPrefs.GetInt("Enemy_MagicDefense", 0);
        GameManager.thisManager.UpdateEnemy_Health(-allDamage, false);
        CreateInstanceOfSkillInfo(PlayerPrefs.GetString("CurrentSkill_Active", string.Empty), 
            allDamage + " damage taken to enemy");

        // Arrange indicator for damage check
        GameManager.thisManager.SpawnDamageIndicator(GameObject.Find("Boss").transform.position, 2, -allDamage);

        //PromptDamageIndicator(SkillManager_Properties.GetActiveInstance(), SkillManager_Properties.GetEffectName(),
            //GetEffectCalculatedValue(1), GetEffectCalculatedValue(2));
    }

    private void AttractDamageResistance()
    {
        // Giving out damage resistance to character
        int resistance = GetEffectCalculatedValue(1) + GetEffectCalculatedValue(2);
        MeloMelo_ExtraStats_Settings.GiveOutDamageResistance(resistance);

        // Toggle user alert
        CreateInstanceOfSkillInfo(PlayerPrefs.GetString("CurrentSkill_Active", string.Empty),
            "Gain " + resistance + " damage resist to character");

        //PromptDamageResistanceIndicator(SkillManager_Properties.GetActiveInstance(), SkillManager_Properties.GetEffectName(),
        //GetEffectCalculatedValue(1), GetEffectCalculatedValue(2));
    }

    private void AttractBonusBaseDamage()
    {
        // Giving out base damage to character
        int bonusDamage = GetEffectCalculatedValue(1) + GetEffectCalculatedValue(2);
        MeloMelo_ExtraStats_Settings.GiveOutBonusBaseDamage(bonusDamage + MeloMelo_ExtraStats_Settings.GetBonusDamage());

        CreateInstanceOfSkillInfo(PlayerPrefs.GetString("CurrentSkill_Active", string.Empty), 
            "Gain " + bonusDamage + " bonus damage to character basic attack");
    }

    private void AttractBonusMagicStats()
    {
        // Giving out magic stats to character
        int bonusMAGStats = GetEffectCalculatedValue(1) + GetEffectCalculatedValue(2);
        //int existingStats = MeloMelo_ExtraStats_Settings.GetExtraMagicStats();
        //MeloMelo_ExtraStats_Settings.GiveOutBonusBaseDamage(existingStats + GetEffectCalculatedValue(1) + GetEffectCalculatedValue(2));

        CreateInstanceOfSkillInfo(PlayerPrefs.GetString("CurrentSkill_Active", string.Empty), 
            "Gain " + bonusMAGStats + " MAG stats to character");
    }
    #endregion

    #region COMPONENT (ACTION LIST 2)
    private void ModifyOfEnemyHealth(int baseValue, int extraValue, string value_key)
    {
        // Damage enemy health with base damage and percentage
        GameManager.thisManager.UpdateEnemy_Health(baseValue + extraValue, false);
        int newLimit = SkillManager_Properties.GetEffectLimit(value_key);
        SkillManager_Properties.SetEffectLimit(value_key, newLimit + 1);
        PromptFinalDamageIndicator(-1, value_key);
    }

    private void ModifyOfCharacterHealth(int baseValue, int extraValue, string value_key)
    {
        // Update the character health with baseValue and extraValue
        GameManager.thisManager.UpdateCharacter_Health(baseValue + extraValue, false);
        int newLimit = SkillManager_Properties.GetEffectLimit(value_key);
        SkillManager_Properties.SetEffectLimit(value_key, newLimit + 1);
        PromptFinalDamageIndicator(-2, value_key);
    }

    private void GivingOutCharacterDamageResistance(int resistValue, int extraResist)
    {
        // Giving out damage resistance to character
        MeloMelo_ExtraStats_Settings.GiveOutDamageResistance(resistValue + extraResist);
    }

    private void GivingOutCharacterBonusBaseDamage(int damageValue, int extraValue, string value_key)
    {
        // Giving out damage resistance to character
        MeloMelo_ExtraStats_Settings.GiveOutBonusBaseDamage(damageValue + extraValue);
        int newLimit = SkillManager_Properties.GetEffectLimit(value_key);
        SkillManager_Properties.SetEffectLimit(value_key, newLimit + 1);
    }
    #endregion

    #region MISC 
    public bool IsSkillOnActive()
    {
        return PlayerPrefs.GetString("Character_Active_Skill", "T") == "T";
    }

    public void UnlockSkillSlot(int index, bool isLocked)
    {
        skillHolder.transform.GetChild(index).GetComponent<RawImage>().texture = skillIconNone[isLocked ? 0 : 1];
    }

    private int GetTotalNumberOfInstance(int activeId)
    {
        Debug.Log("Get total Instance (" + activeId + "): " + SkillManager_Properties.GetTotalActiveInstance(activeId));

        bool getTotalInstance = activeId == 1 && onTrackEffects != null ? true : activeId == 2 && onEndOfTrackEffects != null ? true :
            activeId == 0 && onStartOfTrackEffects != null ? true : false;
        return getTotalInstance ? SkillManager_Properties.GetTotalActiveInstance(activeId) : 0;
    }

    private int GetEffectCalculatedValue(int findIndexOfValue)
    {
        // Format: ActionName, baseValue, GradeValue, StatsValue, GradeLevel
        string[] data_spliter = SkillManager_Properties.GetEffectValueOfTrigger(SkillManager_Properties.GetActiveInstance(), "Action").Split(",");
        float totalValue = 0;

        switch (findIndexOfValue)
        {
            case 1:
                totalValue = int.Parse(data_spliter[1]) * 0.01f * int.Parse(data_spliter[3]);
                break;

            case 2:
                totalValue = int.Parse(data_spliter[1]) * 0.01f * (int.Parse(data_spliter[2]) * int.Parse(data_spliter[4]));
                break;

            default:
                break;
        }

        return (int)totalValue;
    }

    private IEnumerator GetSkillActiveAlert(string skillName, string description)
    {
        SkillIndicator.SetActive(true);
        SkillIndicator.transform.GetChild(0).GetComponent<Text>().text = "Skill Effect: " + skillName;
        SkillIndicator.transform.GetChild(1).GetComponent<Text>().text = description;
        yield return new WaitForSeconds(2.5f);
        SkillIndicator.SetActive(false);
    }

    private void CreateInstanceOfSkillInfo(string title, string details)
    {
        if (effectToggleData != null)
        {
            EffectToggleData temp = new EffectToggleData();
            temp.skillName = title;
            temp.description = details;
            effectToggleData.Add(temp);
        }
    }
    #endregion

    #region MISC (SKILL PROMPT)
    public void PromptDamageIndicator(int counter, string effectName, int baseDamage, int bonusDamage)
    {
        // Get damage calculated result before receiving damage to targeted units
        Debug.Log(counter + " - Get intital damage of (" + effectName + "): Base: " + baseDamage + " | Skill Grade: " + bonusDamage);
    }

    public void PromptFinalDamageIndicator(int counter, string effectName)
    {
        // Get damage result after receiving damage to targeted units
        Debug.Log(counter + " - Get final Damage of (" + effectName + "): Total: " +
                (SkillManager_Properties.GetEffectBaseDamage(effectName) + SkillManager_Properties.GetEffectExtraPercentage(effectName))
                );
    }

    public void PromptDamageResistanceIndicator(int counter, string effectName, int baseValue, int bonusValue)
    {
        Debug.Log(counter + " - Receive damage resistance from (" + effectName + "): Base: " + baseValue + " | Skill Grade: " + bonusValue);
    }

    public void PromptResultOfDamageResistance(int originalDamage, int damageAfterResist, string typeOfUnit)
    {
        // After damaging any units health (Before: Original Damage, After: Damage with resist value)
        Debug.Log(" - Final damage from (" + typeOfUnit + "): Before: " +
           originalDamage + " | After: " + damageAfterResist);
    }

    public void PromptCharacterBaseDamage(string typeOfAttack, int originalDamage, int bonusDamage)
    {
        // Auto attack for character base damage (Before: Just Base Damage, After: Base Damage added with bonus damage)
        Debug.Log("Get character bonus damage of (" + typeOfAttack + "): " +
            "Before: " + originalDamage + " | After: " + (originalDamage + bonusDamage));
    }
    #endregion
}