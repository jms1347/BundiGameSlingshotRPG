using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollController : MonoBehaviour
{
    private PlayerController player;
    private float damage;

    [SerializeField] private float damageHitCoefficient;
    [SerializeField] private float invincibilityTime;
    bool isinvincibility = false;

    [SerializeField] private Color32 oriColor;
    [SerializeField] private Color32 invincibilityColor;


    private void Awake()
    {
        player = this.GetComponent<PlayerController>();
    }
    public float GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!this.name.Contains("Player"))
        {
            return;
        }
        // �浹�� ������Ʈ�� �ڽ����� Ȯ��
        if (this.transform.IsChildOf(coll.transform))
        {
            Debug.Log( " �ڽ� : "+this.gameObject.name);                                                                                               
            return; // �ڽ� �ݶ��̴��� ����
        }

    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (!this.name.Contains("Player"))
        {
            return;
        }
        if (this.transform.IsChildOf(coll.transform))
        {
            Debug.Log(" �ڽ� : " + this.gameObject.name);
            return; // �ڽ� �ݶ��̴��� ����
        }        
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Ground"))
        {
           // Debug.Log("���� ���");
            //player.IsGroundColl = false;
        }
    }


    //private void OnCollisionStay2D(Collision2D coll)
    //{
    //    if (coll.gameObject.tag == "Damage" && this.name.Equals("Cat"))
    //    {
    //        if (isinvincibility) return;

    //        damage = 1;

    //        //ƨ��� �ڵ��ε�
    //        //int dirc = player.transform.position.x - coll.transform.position.x > 0 ? 1 : -1;
    //        //Vector3 dirV = new Vector2(dirc, 1);
    //        //player.SetGimmickMoveStats(dirV, damageHitCoefficient);

    //        //���� ���������� �����ϴ� �Լ�
    //        //Invincibility();
    //        //Invoke(nameof(CallBackInvincibilityTime), invincibilityTime);
    //        //player.ChangeState(PlayerStateContext.PlayerState.DAMAGEHIT);

    //        // �Ѹ�� �ڵ�
    //        player.ChangeState(PlayerStateContext.PlayerState.Death);
    //    }

    //    if (coll.gameObject.tag == "Boss" && this.name.Equals("Cat"))
    //    {
    //        if (!player.IsCurrentState(PlayerStateContext.PlayerState.DROP))
    //        {
    //            if (isinvincibility) return;

    //            damage = 1;

    //            int dirc = player.transform.position.x - coll.transform.position.x > 0 ? 1 : -1;
    //            Vector3 dirV = new Vector2(dirc, 1);

    //            player.SetGimmickMoveStats(dirV, damageHitCoefficient);

    //            Invincibility();
    //            Invoke(nameof(CallBackInvincibilityTime), invincibilityTime);
    //            player.ChangeState(PlayerStateContext.PlayerState.DAMAGEHIT);
    //        }
    //        else
    //        {
    //            int dirc = player.transform.position.x - coll.transform.position.x > 0 ? 1 : -1;
    //            Vector3 dirV = new Vector2(dirc, 1);

    //            player.SetGimmickMoveStats(dirV, damageHitCoefficient * 10);
    //        }
    //    }
    //}

    public void Invincibility()
    {
        isinvincibility = true;

        this.GetComponent<SpriteRenderer>().color = invincibilityColor;

    }
    public void CallBackInvincibilityTime()
    {
        isinvincibility = false;

        this.GetComponent<SpriteRenderer>().color = oriColor;
    }
}
