using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLite : MonoBehaviour
{
    private float speed = 3;
    private bool isActive = false;
    private bool jump = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "note")
        {
            switch (other.GetComponent<Note_Script>().note_index)
            {
                case 1:
                    if (isActive)
                    {
                        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Music/SE/EnemyHit"), new Vector3(0, 0, -10f));
                        NoteHitting_Object(other);
                    }
                    break;

                case 2:
                    AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Music/SE/Trap"), new Vector3(0, 0, -10f));
                    Obstacle_Traps_Detection(other);
                    break;

                case 3:
                    PickUp_Item(other, "Item");
                    break;

                case 4:
                    PickUp_Item(other, "Jump");
                    break;

                case 5:
                    if (isActive) { NoteHitting_Object(other); }
                    break;

                case 6:
                    if (jump && isActive)
                    {
                        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Music/SE/EnemyHit"), new Vector3(0, 0, -10f));
                        NoteHitting_Object(other);
                    }
                    break;

                case 7:
                    if (jump) { Obstacle_Traps_Detection(other); }
                    break;
            }
        }
    }

    void Update()
    {
        // Movement Control
        Vector3 moveX = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        transform.Translate(moveX * speed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -TutorialManager.thisManager.get_limitBorder, TutorialManager.thisManager.get_limitBorder), transform.position.y, transform.position.z);

        if (Input.GetAxis("Horizontal") != 0) { GetComponent<Animator>().SetBool("Move", true); }
        else { GetComponent<Animator>().SetBool("Move", false); }

        // Hit Active
        if (!isActive && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            GetComponent<Animator>().SetTrigger("Attack");
            isActive = true;
            Invoke("CancelActive", 1);
        }

        // Jump Function
        if (!jump && Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(Jump());
        }
    }

    // Character Jump: Function
    private IEnumerator Jump()
    {
        jump = true;
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);

        yield return new WaitForSeconds(1);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        jump = false;
    }

    // Note Hits: Function
    protected void NoteHitting_Object(Collider other)
    {
        if (true)
        {
            TutorialManager.thisManager.UpdateNoteStatus("Perfect");
            if (other.GetComponent<Note_Script>().note_index == 1 || other.GetComponent<Note_Script>().note_index == 6)
            { TutorialManager.thisManager.UpdateBattle_Progress(0); }

            if (other.GetComponent<Note_Script>().note_index != 5) { Destroy(other.gameObject); }
            else { other.GetComponent<Note_Script>().Reflect_MoveEffect(); }
        }
        else
        {
            TutorialManager.thisManager.UpdateNoteStatus("Bad");
            if (other.GetComponent<Note_Script>().note_index == 1 || other.GetComponent<Note_Script>().note_index == 6)
            { TutorialManager.thisManager.UpdateBattle_Progress(0); }

            if (other.GetComponent<Note_Script>().note_index != 5) { Destroy(other.gameObject); }
            else { other.GetComponent<Note_Script>().Reflect_MoveEffect(); }
        }

        QuickActiveCancel();
    }

    protected void PickUp_Item(Collider other, string index)
    {
        switch (index)
        {
            case "Jump":
                if (jump && !isActive)
                {
                    AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Music/SE/Item2"), new Vector3(0, 0, -10f));
                    NoteHitting_Object(other);
                }
                break;

            case "Trap":
                NoteHitting_Object(other);
                break;

            default:
                if (!isActive)
                {
                    AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Music/SE/Item"), new Vector3(0, 0, -10f));
                    NoteHitting_Object(other);
                }
                break;
        }
    }

    protected void Obstacle_Traps_Detection(Collider other)
    {
        GameManager.thisManager.UpdateNoteStatus("Miss");
        Destroy(other.gameObject);
    }

    // Hit Cancel: Function
    protected void CancelActive()
    {
        isActive = false;
    }

    protected void QuickActiveCancel()
    {
        isActive = false;
        CancelInvoke();
    }
}
