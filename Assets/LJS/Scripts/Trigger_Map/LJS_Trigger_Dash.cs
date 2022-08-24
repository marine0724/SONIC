using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_Trigger_Dash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        // 트리거가 작동 되면 대쉬하고싶다.

        if (other.gameObject == LJS_Player.Instance.gameObject)
        {
            LJS_CamRotate.Instance.OnShakeCamera(0.4f, 0.5f);
            //CamRotate.Instance.OnShakeCamera_Rot(0.1f, 2f);

            LJS_Player.Instance.speed += 20;

            LJS_CreateShockWave_OnDash.Instance.ShockShock();
        }
    }
}
