using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator_FloatingText : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float duration = 1f;
    public Vector3 moveDirection = new Vector3(0, 1, 0);

    private float timer;
    private Vector3 startScale;
    private Vector3 targetScale = Vector3.zero;

    public void ResetFloatText(float time)
    {
        startScale = transform.localScale;
        timer = Time.deltaTime;
        duration = time;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Move upward
        transform.position += moveDirection * floatSpeed * Time.deltaTime;

        // Scale from big to small
        transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
    }
}
