using UnityEngine;
using MeloMelo_PlayerManagement;

public class Notation_Visual_Script : MonoBehaviour
{
    public ParticleSystem judgeline;
    private Note_Script mainScript;

    void Start()
    {
        mainScript = GetComponent<Note_Script>();
        if (judgeline != null && ChannelAnimatorValve()) judgeline.Play();
    }

    #region MAIN
    public void JudgeLineToggle() { if (judgeline) judgeline.Stop(); }
    #endregion

    #region COMPONENT
    private bool ChannelAnimatorValve()
    {
        if (mainScript.note_define_index == CharacterSettings.PICKUP_TYPE.JUMP)
        {
            return true;
        }

        else if (mainScript.note_define_index == CharacterSettings.PICKUP_TYPE.NONE)
        {
            switch (mainScript.note_index)
            {
                case 2:
                    return true;

                default:
                    return false;
            }
        }

        return false;
    }
    #endregion
}
