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

        // Ʈ���Ű� �۵� �Ǹ� �����ϰ�ʹ�.

        if (other.gameObject == LJS_Player.Instance.gameObject)
        {
            LJS_Player.Instance.isAttackOn = false;
            LJS_Player.Instance.yVelocity = 30;

            print("����!");
            springJump.Stop();
            springJump.Play();


        }
    }
}
