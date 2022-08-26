using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LJS_Ring : MonoBehaviour
{
    // ��� ���ư�
    // �÷��̾ ������ ������鼭 �Ҹ���
    
    float rotSpeed = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //��� ���ư���
        transform.eulerAngles += new Vector3(0, 0, rotSpeed * Time.deltaTime);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == LJS_Player.Instance.gameObject)
        {
            print("Ʈ���� ����");

            LJS_UI_Score.Instance.Score += 10;

            Destroy(gameObject);

        }
    }
}
