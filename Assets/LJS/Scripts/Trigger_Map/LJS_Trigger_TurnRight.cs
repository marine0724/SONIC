using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_Trigger_TurnRight : MonoBehaviour
{


    // Ʈ���ŷ� ������, ĳ������ �����ΰ��� Vector �� ���� ���̰�
    // �� ���� ������ ���� �ø���.
    // ī�޶��� ������ ĳ���͸� �ٶ󺸰� �ؾ���.

    // �ʿ�Ӽ� : Player �� ����


    bool isTurn;

    private void OnTriggerEnter(Collider other)
    {
        // hAxis ���� �ø���.

        // ���� ī�޶��� ���⵵ �����غ��� ��.


        // Trigger �� gameobject �� �÷��̾���
        if (other.gameObject == LJS_Player.Instance.gameObject)
        {
            print("Ʈ���� �ۿ�");

            // ī�޶� �׳� ���������� �ɰŰ�����?!
            // ī�޶� �ٶ󺸴� ������  Player �� �����̴ϱ�!
            isTurn = true;


            // ī�޶� ������ ����, �÷��̾���  hAxis �� �� �׿������� 
            LJS_Player.Instance.dir.x = 0;
            LJS_Player.Instance.subDir = new Vector3(0,0,0);

            // �״��� �ӵ��� �÷��ָ� ���ε�?!
            LJS_Player.Instance.speed += 30;


        }


    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTurn == true)
        {
            LJS_CamRotate.Instance.x = Mathf.Lerp(LJS_CamRotate.Instance.x, 100, 15 * Time.deltaTime) ;

            if (LJS_CamRotate.Instance.x > 95)
            {
                isTurn = false;
                print("�ۿ�");
            }
        }
    }
}
