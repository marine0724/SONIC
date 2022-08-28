using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_Enemy : MonoBehaviour
{
    // �÷��̾�� �����Ÿ��� ������ ���� �Ǹ�, �÷��̾�� �޷�����.
    // ���¸ӽ�����?

    // ����: ���, ����, ��ȸ?


    enum EnemyState
    { 
        Idle,
        Move,
        Attack,
        Damage,
        Die
    }

    EnemyState m_State = EnemyState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damage:
                Damage();
                break;
            case EnemyState.Die:
                Die();
                break;             
        }
    }

    private void Idle()
    {
        // �����Ÿ� �̳��� �÷��̾ ������, Move���·� ��ȯ �ϰ� �ʹ�.
        float distance = (transform.position - LJS_Player.Instance.transform.position).magnitude;

        // �����Ÿ� �̳��� ������
        if (distance < 10)
        {
            //Move ���·� ��ȯ
            m_State = EnemyState.Move;
        
        }
       
    }


    private void Move()
    {
        throw new NotImplementedException();
    }


    private void Attack()
    {
        throw new NotImplementedException();
    }


    private void Damage()
    {
        throw new NotImplementedException();
    }


    private void Die()
    {
        throw new NotImplementedException();
    }


}
