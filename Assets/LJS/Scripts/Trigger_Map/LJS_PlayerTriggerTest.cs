using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_TriggerTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == LJS_Player.Instance.gameObject)
        {
            print("캐릭터 컨트롤러로 트리거 충돌하기");
        }
    }
}
