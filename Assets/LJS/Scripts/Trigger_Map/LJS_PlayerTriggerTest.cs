using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_TriggerTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == LJS_Player.Instance.gameObject)
        {
            print("ĳ���� ��Ʈ�ѷ��� Ʈ���� �浹�ϱ�");
        }
    }
}
