using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    private List<SkillEffectType> onStartOfTrackEffects;
    private List<SkillEffectType> onEndOfTrackEffects;
    private List<SkillEffectType> onTrackEffects;

    [SerializeField] private GameObject allEffectIndicator;
    [SerializeField] private GameObject skillHolder;

    void Start()
    {
        onStartOfTrackEffects = new List<SkillEffectType>();
        onEndOfTrackEffects = new List<SkillEffectType>();
        onTrackEffects = new List<SkillEffectType>();
    }

    void Update()
    {
        OnPlayEffect_Update(BeatConductor.thisBeat.gameObject.GetComponent<AudioSource>().isPlaying);
    }

    #region SETUP (Effect Manipluate)
    public void ExtractSkill(SkillContainer skill, ClassBase stats)
    {
        if (skill.onStartOfEffect != null)
            foreach (SkillEffectType unloadAndModify in skill.onStartOfEffect)
                onStartOfTrackEffects.Add(ModifyOfSkillEffectData(skill.skillName, unloadAndModify, stats));

        if (skill.onEndOfEffect != null)
            foreach (SkillEffectType unloadAndModify in skill.onEndOfEffect)
                onEndOfTrackEffects.Add(ModifyOfSkillEffectData(skill.skillName, unloadAndModify, stats));

        if (skill.duringPlayOfEffect != null)
            foreach (SkillEffectType unloadAndModify in skill.duringPlayOfEffect)
                onTrackEffects.Add(ModifyOfSkillEffectData(skill.skillName, unloadAndModify, stats));

        // Reset all existing effect data
        if (skill.onStartOfEffect != null) OnResetStatusEffect(onStartOfTrackEffects.ToArray());
        if (skill.onEndOfEffect != null) OnResetStatusEffect(onEndOfTrackEffects.ToArray());
        if (skill.duringPlayOfEffect != null) OnResetStatusEffect(onTrackEffects.ToArray());
    }

    private SkillEffectType ModifyOfSkillEffectData(string skillName, SkillEffectType originalCopy, ClassBase stats)
    {
        SkillEffectType completedModify = new SkillEffectType();
        completedModify.effectName = originalCopy.effectName;
        completedModify.baseDamageStats = originalCopy.baseDamageStats;
        completedModify.extraStatsPercentage = originalCopy.extraStatsPercentage;
        completedModify.valueOfTrigger = originalCopy.valueOfTrigger;

        completedModify.baseDamageStats *= originalCopy.stats == SkillEffectType.MainTypeStats.STR ? stats.strength :
            originalCopy.stats == SkillEffectType.MainTypeStats.VIT ? stats.vitality : stats.magic;

        completedModify.extraStatsPercentage *= PlayerPrefs.GetInt(skillName + "_Grade_Code", 1);
        return completedModify;
    }
    #endregion

    #region SETUP
    public void RegisterForSkillUsage(SkillContainer skill, bool isPrimary)
    {
        // Update skill icon for displaying
        skillHolder.transform.GetChild(isPrimary ? 1 : 2).GetComponent<RawImage>().texture = skill.skillIcon;
        Debug.Log((isPrimary ? 1 : 2) + ": " + skill.skillName + " --> Register Completed!");
    }

    private void OnResetStatusEffect(SkillEffectType[] effectForReset)
    {
        foreach (SkillEffectType effect in effectForReset)
        {
            PlayerPrefs.DeleteKey(effect.effectName + "_Limit");
            ClearBuffEffect(effect.effectName);
        }
    }
    #endregion

    #region MAIN
    public void OnPlayEffect_Update(bool onPlaying)
    {
        // Update this effect only when the game is still on-going
        if (onPlaying)
        {
            foreach (SkillEffectType effect in onTrackEffects)
            {
                PlayerPrefs.SetInt(effect.effectName + "_BaseDamage", effect.baseDamageStats);
                PlayerPrefs.SetInt(effect.effectName + "_ExtraDamagePercentage", effect.extraStatsPercentage);
                PlayerPrefs.SetString(effect.effectName + "_ValueOfTrigger", effect.valueOfTrigger);
                CreateBuffEffect(effect.effectName);
                Invoke(effect.effectName, 0);
            }
        }

        // Otherwise this effect will not be active again
        else
            foreach (SkillEffectType effect in onTrackEffects)
                ClearBuffEffect(effect.effectName);
    }

    public void OnStartEffect_Update()
    {
        // Update during the start of track
        foreach (SkillEffectType effect in onStartOfTrackEffects)
        {
            PlayerPrefs.SetInt(effect.effectName + "_BaseDamage", effect.baseDamageStats);
            PlayerPrefs.SetInt(effect.effectName + "_ExtraDamagePercentage", effect.extraStatsPercentage);
            PlayerPrefs.SetString(effect.effectName + "_ValueOfTrigger", effect.valueOfTrigger);
            CreateBuffEffect(effect.effectName);
            Invoke(effect.effectName, 2);
        }
    }

    public void OnEndEffect_Update()
    {
        // Update only if the track come to an end
        foreach (SkillEffectType effect in onEndOfTrackEffects)
        {
            PlayerPrefs.SetInt(effect.effectName + "_BaseDamage", effect.baseDamageStats);
            PlayerPrefs.SetInt(effect.effectName + "_ExtraDamagePercentage", effect.extraStatsPercentage);
            PlayerPrefs.SetString(effect.effectName + "_ValueOfTrigger", effect.valueOfTrigger);
            CreateBuffEffect(effect.effectName);
            Invoke(effect.effectName, 2);
        }
    }
    #endregion

    #region COMPONENT (CREATE BUFF INSTANCE)
    private void CreateBuffEffect(string effectName)
    {
        if (!PlayerPrefs.HasKey(effectName + "_Buffer"))
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
    private void OnComboDamage()
    {
        const string effectKey = "OnComboDamage";
        int countReachLimit = GameManager.thisManager.getJudgeWindow.getCombo == 0 ? 1 : PlayerPrefs.GetInt(effectKey + "_Limit", 1);
        GameObject.Find(effectKey).transform.GetChild(0).GetComponent<Text>().text = (countReachLimit - 1).ToString();

        // On Activation upon reached a certain combo
        if (GameManager.thisManager.getJudgeWindow.getCombo >= countReachLimit * int.Parse(PlayerPrefs.GetString(effectKey + "_ValueOfTrigger", string.Empty)))
        {
            ModifyOfEnemyHealth(PlayerPrefs.GetInt(effectKey + "_BaseDamage", 0), PlayerPrefs.GetInt(effectKey + "_ExtraDamagePercentage", 0),
                effectKey
                );
        }
    }

    private void OnCountdownShield()
    {
        const string effectKey = "OnCountdownShield";
        if (!PlayerPrefs.HasKey(effectKey + "_Limit"))
            PlayerPrefs.SetFloat(effectKey + "_Limit", int.Parse(PlayerPrefs.GetString(effectKey + "_ValueOfTrigger")));

        GameObject.Find(effectKey).transform.GetChild(0).GetComponent<Text>().text = Time.time >= PlayerPrefs.GetFloat(effectKey + "_Limit") ? string.Empty :
            (PlayerPrefs.GetFloat(effectKey + "_Limit") - Time.time).ToString("0");

        // On Activation upon character do not have damage resistance in every given seconds
        if (Time.time >= PlayerPrefs.GetFloat(effectKey + "_Limit"))
        {
            // Giving out damage resistance and set timer
            PlayerPrefs.SetFloat(effectKey + "_Limit", Time.time + int.Parse(PlayerPrefs.GetString(effectKey + "_ValueOfTrigger")));

            if (!PlayerPrefs.HasKey("MISC_Character_DamageResist"))
                GivingOutCharacterDamageResistance(PlayerPrefs.GetInt(effectKey + "_BaseDamage"),
                PlayerPrefs.GetInt(effectKey + "_ExtraDamagePercentage", 0));
        }

        else
            GameObject.Find(effectKey).GetComponent<RawImage>().color = !PlayerPrefs.HasKey("MISC_Character_DamageResist") ? Color.grey : Color.white;
    }

    private void OnItemCountForDamage()
    {
        const string effectKey = "OnItemCountForDamage";
        int countReachLimit = PlayerPrefs.GetInt(effectKey + "_Limit", 1);

        GameObject.Find(effectKey).transform.GetChild(0).GetComponent<Text>().text =
            PlayerPrefs.GetInt("MISC_Character_TotalOPickCount", 0).ToString();

        // On Activation upon reached a certain item picked
        if (PlayerPrefs.GetInt("MISC_Character_TotalOPickCount", 0) >= countReachLimit *
            int.Parse(PlayerPrefs.GetString(effectKey + "_ValueOfTrigger", string.Empty)))
            ModifyOfEnemyHealth(PlayerPrefs.GetInt(effectKey + "_BaseDamage", 0), PlayerPrefs.GetInt(effectKey + "_ExtraDamagePercentage", 0),
                effectKey);
    }
    #endregion

    #region SKILL EFFECT (CUSTOM)
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

    #region COMPONENT (ACTION LIST)
    private void ModifyOfEnemyHealth(int baseValue, int extraValue, string value_key)
    {
        // Damage enemy health with base damage and percentage
        GameManager.thisManager.UpdateEnemy_Health(baseValue + extraValue, false);
        PlayerPrefs.SetInt(value_key + "_Limit", PlayerPrefs.GetInt(value_key + "_Limit", 1) + 1);
    }

    private void ModifyOfCharacterHealth(int baseValue, int extraValue, string value_key)
    {
        // Update the character health with baseValue and extraValue
        GameManager.thisManager.UpdateCharacter_Health(baseValue + extraValue, false);
        PlayerPrefs.SetInt(value_key + "_Limit", PlayerPrefs.GetInt(value_key + "_Limit", 1) + 1);
    }

    private void GivingOutCharacterDamageResistance(int resistValue, int extraResist)
    {
        // Giving out damage resistance to character
        PlayerPrefs.SetInt("MISC_Character_DamageResist", resistValue + extraResist);
    }

    private void GivingOutCharacterBonusBaseDamage(int damageValue, int extraValue)
    {
        // Giving out damage resistance to character
        PlayerPrefs.SetInt("MISC_Character_ExtraBaseDamage", damageValue + extraValue);
    }
    #endregion
}