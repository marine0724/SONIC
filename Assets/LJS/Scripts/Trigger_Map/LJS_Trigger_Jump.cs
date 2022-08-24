using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_Trigger_Jump : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        // 트리거가 작동 되면 점프하고싶다.

        if (other.gameObject == LJS_Player.Instance.gameObject)
        {
            print("점프!");
            LJS_Player.Instance.yVelocity = 30;

        }
    }
}
