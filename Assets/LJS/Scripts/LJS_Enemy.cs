using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_Enemy : MonoBehaviour
{
    // 플레이어랑 일정거리에 가까이 오게 되면, 플레이어에게 달려오게.
    // 상태머신으로?

    // 상태: 대기, 공격, 배회?


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
        // 일정거리 이내에 플레이어가 들어오면, Move상태로 전환 하고 싶다.
        float distance = (transform.position - LJS_Player.Instance.transform.position).magnitude;

        // 일정거리 이내에 들어오면
        if (distance < 10)
        {
            //Move 상태로 전환
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
