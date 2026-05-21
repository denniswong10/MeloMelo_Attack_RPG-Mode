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
        if (other.gameObject.name == "Note5(Clone)" && 
            other.gameObject.GetComponent<Notation_Motion_Script>().get_direction_type == Notation_Motion_Script.Direction.Backward) 
        { 
            //GameManager.thisManager.UpdateEnemy_Health(-(PlayerPrefs.GetInt("Character_OverallDamage", 0) * 2), false);
            //GameManager.thisManager.SpawnDamageIndicator(transform.position, 2, -(PlayerPrefs.GetInt("Character_OverallDamage", 0) * 2));

            int finalDamageCount = PlayerPrefs.GetInt("Character_OverallDamage", 0) + MeloMelo_ExtraStats_Settings.GetBonusDamage();
            if (finalDamageCount <= 0) { finalDamageCount = Random.Range(0, 10) > 5 ? -1 : 0; }

            float randomCriticalValue = Random.Range(0, 100);

            if (randomCriticalValue >= (100f - PlayerPrefs.GetFloat("Extra_Stats_1", 0)))
            {
                int finalCriticalValue = (int)(finalDamageCount + (finalDamageCount * 0.01f * randomCriticalValue));
                GameManager.thisManager.UpdateEnemy_Health(-finalCriticalValue * 2, false);
                GameManager.thisManager.SpawnDamageIndicator(transform.position, 2, -finalCriticalValue * 2, true);
            }
            else
            {
                GameManager.thisManager.UpdateEnemy_Health(-finalDamageCount * 2, false);
                GameManager.thisManager.SpawnDamageIndicator(transform.position, 2, -finalDamageCount * 2);
            }
        }
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
                transform.Rotate(Vector3.up * -100 * Time.deltaTime, Space.World);
                try { if (transform.position.x <= -GameManager.thisManager.get_playField.get_limitBorder) { moveDir = MoveDirection.right; } }
                catch { if (transform.position.x <= -TutorialManager.thisManager.get_limitBorder) { moveDir = MoveDirection.right; } }
                break;

            case MoveDirection.right:
                transform.Translate(Vector3.right * 2 * Time.deltaTime, Space.World);
                transform.Rotate(Vector3.up * 100 * Time.deltaTime, Space.World);
                try { if (transform.position.x >= GameManager.thisManager.get_playField.get_limitBorder) { moveDir = MoveDirection.left; } }
                catch { if (transform.position.x >= TutorialManager.thisManager.get_limitBorder) { moveDir = MoveDirection.left; } }
                break;
        }
    }
}
