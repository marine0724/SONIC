using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LJS_GoalRing : MonoBehaviour
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
        transform.eulerAngles += new Vector3(0, rotSpeed * Time.deltaTime, 0 );

    }

}
