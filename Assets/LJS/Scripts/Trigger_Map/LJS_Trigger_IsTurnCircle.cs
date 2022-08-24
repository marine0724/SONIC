using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_IsTurnCircle : MonoBehaviour
{
    // �÷��̾ ���⿡ ������, player �� IsTurnCircle �� true, false �� ����� �ʹ�
    public bool IsTurnCircle_StartPoint;
    public bool IsTurnCircle_EndPoint;


    private void OnTriggerEnter(Collider other)
    {
        // ��ŸƮ ����Ʈ������,
        // �÷��̾ ���ǵ尡 50 �̻��϶� �÷��̾��� IsTurnCircle �� true �� �������,

        if (IsTurnCircle_StartPoint)
        {
            if (LJS_Player.Instance.speed > 50)
            {
                LJS_Player.Instance.isTurnCircle = true;
            }       
        }

        //���� ����Ʈ ������
        // �÷��̾��� IsTurnCircle �� false �� ����
        if (IsTurnCircle_EndPoint)
        {
            LJS_Player.Instance.isTurnCircle = false;
        }
    }



}
