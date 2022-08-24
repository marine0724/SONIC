using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_IsTurnCircle : MonoBehaviour
{
    // 플레이어가 여기에 닿으면, player 의 IsTurnCircle 을 true, false 로 만들고 싶다
    public bool IsTurnCircle_StartPoint;
    public bool IsTurnCircle_EndPoint;


    private void OnTriggerEnter(Collider other)
    {
        // 스타트 포인트에서는,
        // 플레이어가 스피드가 50 이상일때 플레이어의 IsTurnCircle 을 true 로 만들어줌,

        if (IsTurnCircle_StartPoint)
        {
            if (LJS_Player.Instance.speed > 50)
            {
                LJS_Player.Instance.isTurnCircle = true;
            }       
        }

        //엔드 포인트 에서는
        // 플레이어의 IsTurnCircle 을 false 로 만듦
        if (IsTurnCircle_EndPoint)
        {
            LJS_Player.Instance.isTurnCircle = false;
        }
    }



}
