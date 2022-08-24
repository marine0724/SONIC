using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSLerp : MonoBehaviour
{
    //�����ؾ��ϴ� Ÿ��
    public Transform target;
    //���� ������ ����
    bool isJump;
    
    float currTime;

    //���� ���� ����
    Vector3 startPos;
    //���� ����(startPos)�� Ÿ���� ��ġ�� �߽�
    Vector3 centerPivot;
    
    //ó�� ���ǵ�
    float speed = 1;
    //�����ö� ���ӵǴ� ��ġ
    public float accel = 0.06f;

    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && isJump == false)
        {
            //������ �ʱ�ȭ
            speed = 1;
            isJump = true;
            startPos = transform.position;
            currTime = 0;

            //���� ����(startPos)�� Ÿ���� ��ġ�� �߽� ���ϱ�
            centerPivot = (startPos + target.position) * 0.5f;
            //�߽��� y���� 1��ŭ ������
            centerPivot.y -= 1;
        }

        //���� ��ġ�� ���ư���
        if(Input.GetKeyDown(KeyCode.Alpha2) && isJump == false)
        {
            transform.position = startPos;
        }

        if (isJump)
        {
            
            currTime += Time.deltaTime * speed;
            //2�� �ȿ� �������� �����ϰ� ��(2�� �ٲٸ� �ش� �ʿ� ����)
            float ratio = currTime / 2;

            //Slerp�� �̿��� �̵�
            transform.position = Vector3.Slerp(startPos - centerPivot, target.position - centerPivot, ratio) + centerPivot;

            //�ְ� ���̿� ���� ���Ŀ��� �����ϱ�
            if(ratio >= 0.5f)
            {
                speed += accel;
            }

            if(ratio >= 1)
            {
                isJump = false;                
            }
        }
    }
}
