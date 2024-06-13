using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteTick_Script : MonoBehaviour
{
    private float note_speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystem>().Play();
        note_speed = BeatConductor.thisBeat.get_noteSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        ObstacleEnd_Check(0.1f);
    }

    #region MISC
    protected void ObstacleEnd_Check(float offset)
    {
        if (transform.position.z <= GameObject.Find("Judgement Line").transform.position.z + offset)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.Find("Judgement Line").transform.position.z + offset);
            Destroy(gameObject);
        }
        else
        {
            try { transform.Translate(Vector3.back * BeatConductor.thisBeat.get_BPM_Calcuate * note_speed * Time.deltaTime); }
            catch { transform.Translate(Vector3.back * note_speed * Time.deltaTime); }
        }
    }
    #endregion
}
