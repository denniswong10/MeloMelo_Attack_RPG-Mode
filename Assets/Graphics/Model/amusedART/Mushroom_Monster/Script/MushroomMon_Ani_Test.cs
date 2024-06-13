using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMon_Ani_Test : MonoBehaviour {

	public const string IDLE	= "Idle";
	public const string RUN		= "Run";
	public const string ATTACK	= "Attack";
	public const string DAMAGE	= "Damage";
	public const string DEATH	= "Death";

	Animation anim;
    enum MoveDirection { left, right };
    private MoveDirection moveDir = MoveDirection.left;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Note5(Clone)") { GameManager.thisManager.UpdateEnemy_Health(-(PlayerPrefs.GetInt("Character_OverallDamage", 0) * 2), false); }
    }

    void Start () {
		anim = GetComponent<Animation>();
	}

	public void IdleAni (){
		anim.CrossFade (IDLE);
	}

	public void RunAni (){
		anim.CrossFade (RUN);
	}

	public void AttackAni (){
		anim.CrossFade (ATTACK);
	}

	public void DamageAni (){
		anim.CrossFade (DAMAGE);
	}

	public void DeathAni (){
		anim.CrossFade (DEATH);
	}

    void Update()
    {
        switch(moveDir)
        {
            case MoveDirection.left:
                transform.Translate(Vector3.left * 2 * Time.deltaTime, Space.World);
                try { if (transform.position.x <= -GameManager.thisManager.get_playField.get_limitBorder) { moveDir = MoveDirection.right; } }
                catch { if (transform.position.x <= -TutorialManager.thisManager.get_limitBorder) { moveDir = MoveDirection.right; } }
                break;

            case MoveDirection.right:
                transform.Translate(Vector3.right * 2 * Time.deltaTime, Space.World);
                try { if (transform.position.x >= GameManager.thisManager.get_playField.get_limitBorder) { moveDir = MoveDirection.left; } }
                catch { if (transform.position.x >= TutorialManager.thisManager.get_limitBorder) { moveDir = MoveDirection.left; } }
                break;
        }
    }
}
