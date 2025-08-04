using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Notation_Instance
{
    public Notation_Motion_Script motion_script { get; private set; }
    public Note_Script mainScript { get; private set; }

    public Notation_Instance(Notation_Motion_Script motion, Note_Script main)
    {
        motion_script = motion;
        mainScript = main;
    }
}

public class NotationManager : MonoBehaviour
{
    private List<Notation_Instance> note_listing;
    private List<WhiteTick_Script> note_margin_listing;

    // Start is called before the first frame update
    void Start()
    {
        note_listing = new List<Notation_Instance>();
        note_margin_listing = new List<WhiteTick_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        NotationLiveHandler();
        MarginLiveHandler();
    }

    #region MAIN
    public void AddNote(GameObject newObject)
    {
        if (note_listing != null)
        {
            Notation_Motion_Script script = newObject.GetComponent<Notation_Motion_Script>();
            Note_Script mainScript = newObject.GetComponent<Note_Script>();
            note_listing.Add(new Notation_Instance(script, mainScript));
        }
    }

    public void AddMargin(GameObject marginObject)
    {
        if (note_margin_listing != null)
        {
            WhiteTick_Script script = marginObject.GetComponent<WhiteTick_Script>();
            note_margin_listing.Add(script);
        }
    }
    #endregion

    #region COMPONENT
    private void NotationLiveHandler()
    {
        if (note_listing != null)
        {
            for (int i = note_listing.Count - 1; i >= 0; i--)
            {
                var note = note_listing[i];

                if (note == null || note.mainScript == null)
                {
                    note_listing.RemoveAt(i);
                    continue;
                }

                if (note.motion_script != null) note_listing[i].motion_script.UniversalMotionTracker();
                if (note.mainScript != null) note_listing[i].mainScript.GetTimeOutNotation();
            }
        }
    }

    private void MarginLiveHandler()
    {
        if (note_margin_listing != null)
        {
            for (int i = note_margin_listing.Count - 1; i >= 0; i--)
            {
                var margin = note_margin_listing[i];

                if (margin == null)
                {
                    note_margin_listing.RemoveAt(i);
                    continue;
                }

                if (margin != null) margin.UniservalMarginTracker();
            }
        }
    }
    #endregion
}
