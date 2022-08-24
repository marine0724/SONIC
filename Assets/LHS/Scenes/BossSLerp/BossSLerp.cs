using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSLerp : MonoBehaviour
{
    //점프해야하는 타겟
    public Transform target;
    //점프 중인지 여부
    bool isJump;
    
    float currTime;

    //점프 시작 지점
    Vector3 startPos;
    //시작 지점(startPos)와 타겟의 위치의 중심
    Vector3 centerPivot;
    
    //처음 스피드
    float speed = 1;
    //내려올때 가속되는 수치
    public float accel = 0.06f;

    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && isJump == false)
        {
            //점프시 초기화
            speed = 1;
            isJump = true;
            startPos = transform.position;
            currTime = 0;

            //시작 지점(startPos)와 타겟의 위치의 중심 구하기
            centerPivot = (startPos + target.position) * 0.5f;
            //중심의 y값을 1만큼 내리기
            centerPivot.y -= 1;
        }

        //원래 위치로 돌아가기
        if(Input.GetKeyDown(KeyCode.Alpha2) && isJump == false)
        {
            transform.position = startPos;
        }

        if (isJump)
        {
            
            currTime += Time.deltaTime * speed;
            //2초 안에 목적지에 도착하게 함(2를 바꾸면 해당 초에 도착)
            float ratio = currTime / 2;

            //Slerp를 이용해 이동
            transform.position = Vector3.Slerp(startPos - centerPivot, target.position - centerPivot, ratio) + centerPivot;

            //최고 높이에 도착 이후에는 가속하기
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
