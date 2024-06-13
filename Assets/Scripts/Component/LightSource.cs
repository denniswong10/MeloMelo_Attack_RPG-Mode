using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    private float rotateValue = 0.1f;
    private float angle = 0;
    private bool invert = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (invert)
        {
            angle -= rotateValue;

            if (angle < 1) { invert = false; }
        }
        else
        {
            angle += rotateValue;

            if (angle > 24) { invert = true; }
        }

        transform.eulerAngles = new Vector3(angle, 0, 0);
    }
}
