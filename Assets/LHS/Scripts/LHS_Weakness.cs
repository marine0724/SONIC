using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_Weakness : MonoBehaviour
{
   
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    print("��������");
    //    if (hit.gameObject.name.Contains("Player"))
    //    {
    //        if (LHS_Enemy.Instance.m_State == LHS_Enemy.EnemyState.Weakness)
    //        {
    //            LHS_EnemyHP.Instance.HP--;
    //        }
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        print("��������");
        if (collision.gameObject.name.Contains("Player"))
        {
            if (LHS_Enemy.Instance.m_State == LHS_Enemy.EnemyState.Weakness)
            {
                LHS_EnemyHP.Instance.HP--;
            }
        }
    }

    



    //private void OnTriggerEnter(Collider collision)
    //{
    //    print("��������");

    //    if (LHS_Enemy.Instance.m_State == LHS_Enemy.EnemyState.Weakness)
    //    {
    //        LHS_EnemyHP.Instance.HP--;
    //    }
    //}


}
