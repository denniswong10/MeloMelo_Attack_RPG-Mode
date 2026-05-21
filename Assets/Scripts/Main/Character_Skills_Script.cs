using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Skills_Script : MonoBehaviour
{
    [SerializeField] private Text Skill_Point_Indicator;
    [SerializeField] private Text All_Point_Indicator;
    [SerializeField] private RawImage[] skill_caterogy;
    [SerializeField] private GameObject PromptMessage;

    // Start is called before the first frame update
    void Start()
    {
        ToggleSkillType(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region MAIN
    public void ToggleSkillType(int index)
    {
        for (int i = 0; i < skill_caterogy.Length; i++)
        {
            if (i == index)
            {
                skill_caterogy[i].color = Color.green;
                GetSkillInfoByIndex(i);
            }
            else
                skill_caterogy[i].color = Color.white;
        }
    }

    public void ToggleALLPointPanel(bool visible)
    {

    }
    #endregion

    #region COMPONENT
    private void GetSkillInfoByIndex(int index)
    {
        switch (index)
        {
            case 1:
                break;

            case 2:
                break;

            default:
                break;
        }
    }

    private void GetPrompt(string message, bool isVisible)
    {
        PromptMessage.SetActive(isVisible);
        PromptMessage.GetComponentInChildren<Text>().text = message;
    }
    #endregion
}
