using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control_Menu : MonoBehaviour
{
    public InputField keyBindingInput;
    private bool input_key;
    private int assignIndex = -1;
    public GameObject[] ref_object;

    private char[] input_mapped_key = new char[24];

    //[Header("LeftMovement_Panel")]
    //private char Keyboard_Mapped1_LefMovement = '/';
    //private char Keyboard_Mapped2_LefMovement = '/';
    //private char Mouse_Mapped1_LefMovement = '/';
    //private char Mouse_Mapped2_LefMovement = '/';
    //private char Joystick_Mapped1_LefMovement = '/';
    //private char Joystick_Mapped2_LefMovement = '/';

    // Start is called before the first frame update
    void Start()
    {
        ResetAllInputKey();
        KeyBindingModified();
    }

    // Update is called once per frame
    void Update()
    {
        if (input_key && Input.GetKeyDown(KeyCode.Escape))
        {
            ref_object[assignIndex].transform.GetChild(0).GetComponent<Text>().text = "Not Mapped";
            PlayerPrefs.SetString("KeyBinding_" + assignIndex, "/");
            input_key = false;
        }
    }

    private void ResetAllInputKey()
    {
        input_key = false;
        for (int i = 0; i < input_mapped_key.Length; i++)
            input_mapped_key[i] = '/';
    }

    private void KeyBindingModified()
    {
        for (int i = 0; i < input_mapped_key.Length; i++)
            if (PlayerPrefs.GetString("KeyBinding_" + i, "/") != "/") ref_object[i].transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("KeyBinding_" + i, "/");
    }

    public void StartMappingKeyBind(GameObject obj)
    {
        if (!input_key)
        {
            assignIndex = int.Parse(obj.transform.GetChild(0).gameObject.name);
            ref_object[assignIndex].transform.GetChild(0).GetComponent<Text>().text = "-Press Key-";
            input_key =  true;
            keyBindingInput.Select();
        }
    }

    public void SuccessfulKeyBindChanged()
    {
        keyBindingInput.DeactivateInputField();
        ref_object[assignIndex].transform.GetChild(0).GetComponent<Text>().text = keyBindingInput.text.ToUpper();
        PlayerPrefs.SetString("KeyBinding_" + assignIndex, keyBindingInput.text.ToUpper());
        input_key = false;
    }
}
