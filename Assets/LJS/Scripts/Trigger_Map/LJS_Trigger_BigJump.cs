using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_Trigger_BigJump : MonoBehaviour
{

    AudioSource springJump;

    private void Start()
    {
        springJump = GameObject.Find("SoundMgr_SpringJump").GetComponent<AudioSource>();

    }



    private void OnTriggerEnter(Collider other)
    {

        // 트리거가 작동 되면 점프하고싶다.

        if (other.gameObject == LJS_Player.Instance.gameObject)
        {
            LJS_Player.Instance.isAttackOn = false;
            LJS_Player.Instance.yVelocity = 30;

            print("점프!");
            springJump.Stop();
            springJump.Play();


        }
    }
}
