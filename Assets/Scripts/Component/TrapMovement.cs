using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMovement : MonoBehaviour
{
    enum MoveDirection { left, right };
    private MoveDirection moveDir = MoveDirection.left;

    // Update is called once per frame
    void Update()
    {
        switch(moveDir)
        {
            case MoveDirection.left:
                transform.Translate(Vector3.left * 2 * Time.deltaTime, Space.World);
                if (transform.position.x <= -GameManager.thisManager.get_playField.get_limitBorder) { moveDir = MoveDirection.right; }
                break;

            case MoveDirection.right:
                transform.Translate(Vector3.right * 2 * Time.deltaTime, Space.World);
                if (transform.position.x >= GameManager.thisManager.get_playField.get_limitBorder) { moveDir = MoveDirection.left; }
                break;
        }
    }
}
