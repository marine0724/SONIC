using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_Trigger_Jump : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        // Ʈ���Ű� �۵� �Ǹ� �����ϰ�ʹ�.

        if (other.gameObject == LJS_Player.Instance.gameObject)
        {
            print("����!");
            LJS_Player.Instance.yVelocity = 30;

        }
    }
}
