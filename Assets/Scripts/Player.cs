using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
{
    //싱글톤 구현
    //------------------------------//
    public static Player Instance;

    private void Awake()
    {
        Instance = this;
    }
    //------------------------------//


    [Header("--------------------------")]
    [Header("속도         ")]
    //public float speed = 20f;
    [Header("--------------------------")]


    [Header("가속도")]
    public float accel = 0;  
    public float maxAccel = 200f;
    public float avgAccel = 60f;
    public float minAccel = 0f;


    [Header("제로백")]
    public float zeroTohundred = 5f;
    [Header("--------------------------")]


    [Header("플레이어 점프력")]
    public float jumpPower = 10f;
    public float yVelocity;

    // 점프상태 확인
    bool isJump = false;

    // Player 의 2단점프를 위한 변수
    int jumpCount = 0;
    int jumpCountMax = 2;

    // 중력값
    public float gravity = -20f;


    float hAxis;
    float vAxis;

    //---------------------------------------------------------------------------------------------//

    //플레이어 방향
    Vector3 dir;

    //캐릭터컨트롤러 선언
    CharacterController cc;
    //---------------------------------------------------------------------------------------------//

    // 대쉬 줌인 줌아웃 카메라
    float curTime = 0f;
    float dashTime = 1f;

    public bool isDash;


    PostProcessProfile myProfile;
    LensDistortion myLD;


    void Start()
    {
        //캐릭터컨트롤러 컴포넌트 가져오기
        cc = GetComponent<CharacterController>();

        // 자식한테 PostProcessVolume 컴포넌트에 있는
        // 프로파일 가져오기 (만든 postprocess)
        myProfile = GetComponentInChildren<PostProcessVolume>().profile;

        // 프로파일에 렌즈 디스토션 가져오기
        myProfile.TryGetSettings<LensDistortion>(out myLD);


        //myLD.enabled.value = true;
    }


    void Update()
    {
        Jump();
        Dash();
        //pp_LensDistortion(); 렌즈 구부리기
        CameraDetail();
        Rader();
        Attack();
    }


    private void FixedUpdate()
    {
        Move();
    }

    public bool isSensitive = false;

    private void CameraDetail()
    {
        if (accel > 60)
        {
            isSensitive = true;
        }
        else
        {
            isSensitive = false;
        }
    }


    // 강도를 속도에 따라 만들면 될듯
    // 근데 속도의 크기가 60이하일 때는 value = 0
    // 속도의 크기가 60보다 커지면 커진 값만큼 value에 -값이 붙음
    // 근데 value 는 -70 까지만

    private void pp_LensDistortion()
    {
        float abs_accel = Mathf.Abs(accel);
        //강도 낮춤

        // 근데 속도의 크기가 60이하일 때는 value = 0
        if (abs_accel <= 60)
        {
            abs_accel = 0;
        }
        // 속도의 크기가 60보다 커지면
        else if(abs_accel > 60)
        {
            abs_accel = abs_accel - 60;

            if (abs_accel > 100)
                abs_accel = 70;
        }

        //myLD.intensity.value = -abs_accel;
        myLD.intensity.value = Mathf.Lerp(myLD.intensity.value, -abs_accel, Time.deltaTime);
    }


    public bool isRader = false;

    private void Rader()
    {

        // 적을 감지 하는 레이더가 필요함
        #region 레이더 만들기


        // 거리 2
        float attackDistance = 2f;

        print("isRader :" + isRader);
        print("isJumpt :" + isJump);

        //적과의 거리가 2보다 작고 isRader 가 false 이고, isJump 가 true 인 경우에
        if (Vector3.Distance(transform.position, Enemy.Instance.transform.position) < 30 &&  isJump == true)
        {
            // 내가 보는 방향의 -30 ~ 30 도 사이에 있다면
            // 벡터끼리의 각도를 재면 됨
           // if(Vector3.Angle(Enemy.Instance.transform.position - transform.position,   )
            // isRader 를 true 로 만들자
            isRader = true;
        }
        else
            isRader = false;

        #endregion
        // 레이더 안에서 가장 가까운 한놈한테만 Target_Canvas 가 뜸



        // Enemy 는 레이더에 걸리면 bool 함수가 true 로 발동하고
        // 하나가 발동되면, 
        // 발동하면 플레이어가 공격 할 수 있는 상태가 됨

    }


    Rigidbody rigid;

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {

            Vector3 dir = Enemy.Instance.transform.position - transform.position;
            dir.Normalize();

            // 레이더가 true 이면
            if (isRader == true)
            {
                cc.Move(dir * accel);
            }
        }        
    }

    private void Move() //Player 움직이기
    {     
        
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        //------------------------------------------------------------------------------------------------ //Character Controller 로 땅에 닿았을때만 중력값 0이게 만들기.
        //y속도값이 0보다 크거나 땅에 닿지 않았을 경우에는 yVelocity 를 중력값 적용
        if (yVelocity > 0 || !cc.isGrounded)
        {
            isJump = true;
            yVelocity += gravity * Time.deltaTime;
        }

        //y속도값이 땅에 닿고 속도가 0 인경우에는 yVelocity = 0
        else if (yVelocity <= 0 && cc.isGrounded)
        {
            isJump = false;
            yVelocity = 0f;
            jumpCount = 0;
        }
        //------------------------------------------------------------------------------------------------



        // 방향 설정
        dir = new Vector3(hAxis, 0, vAxis).normalized;

         
        # region    카메라가 보는 방향을 앞방향으로 하는 코드
        // 카메라의 y 회전만 구해온다.
        Quaternion v3Rotation = Quaternion.Euler(0f, CamRotate.Instance.gameObject.transform.eulerAngles.y, 0f);

            //이동할 벡터를 돌린다.
            dir = v3Rotation * dir;
        #endregion

        
        #region 방향키를 눌렀을 때, 그 방향을 바라보게 하는 코드
        // 움직일 때
        if (!(hAxis == 0 && vAxis == 0))
            {
                // 지금 보고 있는 값 부터 움직이는 방향으로 바라보는 방향 값 까지 Lerp 로 회전
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 20.0f);
            }

        #endregion



        //------------------------------------------------------------------------------------------------ 여기 뜯어서 다시 봐야함

        
        #region 가속 최댓값 최솟값 설정
        // 앞방향키를 계속 누르고 있을때
        if (vAxis > 0)
            {
                // 만약 avgAccel 보다 작은데
                if (accel < avgAccel)
                {
                    // 만약 accel 값이 음수면, 초당 200 가속
                    if (accel < 0)
                    {
                        accel += 200 * Time.deltaTime;
                    }

                    // 그게 아니라면 제로백만큼 가속 됨
                    accel += (100 / zeroTohundred) * Time.deltaTime;
                }
                // 가속값이 일반 가속값보다 크다면 초당 10만큼 감속
                else if (accel > avgAccel)
                {
                    accel -= 10f * Time.deltaTime;
                }
                // 가속값 최대로 만들기
                //만약 가속값이 최대가속값 이상이라면
                else if (accel >= maxAccel)
                {
                    // 가속은 최대가속값 고정
                    accel = maxAccel;
                }
            }


            // 앞방향키를 계속 누르고 있을때
            else if (hAxis > 0)
            {
                // 그냥 앞방향키 누르고만 있으면 40만큼만 가속됨

                // 만약 avgAccel 보다 작은데
                if (accel < avgAccel)
                {
                    // 만약 accel 값이 음수면, 초당 200 가속
                    if (accel < 0)
                    {
                        accel += 200 * Time.deltaTime;
                    }

                    // 그게 아니라면 제로백만큼 가속 됨
                    accel += (100 / zeroTohundred) * Time.deltaTime;
                }


                // 가속값이 일반 가속값보다 크다면 초당 10만큼 감속
                else if (accel > avgAccel)
                {
                    accel -= 10f * Time.deltaTime;
                }

                // 가속값 최대로 만들기
                //만약 가속값이 최대가속값 이상이라면
                else if (accel >= maxAccel)
                {
                    // 가속은 최대가속값 고정
                    accel = maxAccel;
                }

            }

            // 뒷 방향키를 누른다면,
            // 감속 엄청나게 시키다가
            // 만약 
            else if (vAxis < 0)
            {              
                // 가속도 값이 -avgAccel 보다 크다면
                if (accel > -avgAccel)
                {
                    //근데 만약 그게 양수라면
                    if (accel > 0)
                    {
                        //초당 200만큼 감속
                        accel -= 200 * Time.deltaTime;
                    }

                    // 음수인데 -avgAccel 까지 안되면 제로백만큼 가속 됨
                    accel -= (100 / zeroTohundred) * Time.deltaTime;
                }

                // 가속값 최대로 만들기
                //만약 가속값이 최대가속값 이상이라면
                else if (accel <= -maxAccel)
                {
                    // 가속은 최대가속값 고정
                    accel = -maxAccel;
                }
            }

            else if (hAxis < 0)
            {
                // 가속도 값이 -avgAccel 보다 크다면
                if (accel > -avgAccel)
                {
                    //근데 만약 그게 양수라면
                    if (accel > 0)
                    {
                        //초당 200만큼 감속
                        accel -= 200 * Time.deltaTime;
                    }

                    // 음수인데 -avgAccel 까지 안되면 제로백만큼 가속 됨
                    accel -= (100 / zeroTohundred) * Time.deltaTime;
                }

                // 가속값 최대로 만들기
                //만약 가속값이 최대가속값 이상이라면
                else if (accel <= -maxAccel)
                {
                    // 가속은 최대가속값 고정
                    accel = -maxAccel;
                }
            }


            // 앞 방향키를 누르고 있지 않다면
            else if (vAxis == 0)
            {              
                accel -= 200 * Time.deltaTime;


                //만약 가속값이 최소가속값 이하라면
                if (accel <= minAccel)
                {
                    // 가속은 최소가속값 고정
                    accel = minAccel;
                }
            }
        #endregion
        //------------------------------------------------------------------------------------------------


        //------------------------------------------------------------------------------------------------ characterController 로 점프 구현
        //Speed => 속력 + 가속. 즉 최종속력
        float Speed = accel;

        // 방향의 y 값은 yVelocity
        // Speed 로 나눠주지 않으면 가속도값이 너무 커서, 점프하면 하늘높이 날아감
        // 그리고 5를 곱해주지 않으면, RigidBody 에서 점프한것처럼 붕 뜨는 느낌임

        // [이동]     

        if (accel > 0 && vAxis > 0)
        {
            dir.y = 5 * yVelocity / Speed;
            cc.Move(dir * Speed * Time.deltaTime);
        }
        else if (accel < 0 && vAxis < 0)
        {
            dir.y = -5 * yVelocity / Speed;
            cc.Move(dir * (-Speed) * Time.deltaTime);
        }
        else if (accel > 0 && vAxis < 0)
        {
            dir.y = -5 * yVelocity / Speed;
            cc.Move(dir * (-Speed) * Time.deltaTime);
        }
        //속도는 - 인데, 앞을 보는경우
        else if (accel < 0 && vAxis > 0)
        {
            dir.y = 5 * yVelocity / Speed;
            cc.Move(dir * Speed * Time.deltaTime);
        }
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------

        else if (accel > 0 && hAxis > 0 && vAxis == 0)
        {
            dir.y = 5 * yVelocity / Speed;
            cc.Move(dir * Speed * Time.deltaTime);
        }
        else if (accel < 0 && hAxis < 0 && vAxis == 0)
        {
            dir.y = -5 * yVelocity / Speed;
            cc.Move(dir * (-Speed) * Time.deltaTime);
        }
        else if (accel > 0 && hAxis < 0 && vAxis == 0)
        {
            dir.y = -5 * yVelocity / Speed;
            cc.Move(dir * (-Speed) * Time.deltaTime);
        }
        else if (accel < 0 && hAxis > 0 && vAxis == 0)
        {
            dir.y = 5 * yVelocity / Speed;
            cc.Move(dir * Speed * Time.deltaTime);
        }
    }


    //문제

    // 앞, 뒷 키를 눌렀을때 움직임 => 괜찮음

    // 앞 키를 눌렀을 때 양_옆 움직임 => 괜찮음

    // 뒷 키를 눌렀을 때 왼쪽의 움직임은 => 괜찮음
    //============================================================

    // 앞 키를 누르고있다가 때고 속도 떨어지기 전에 왼쪽 키 누르면 => 반대로가다가 왼쪽으로 감


    // 양, 옆 키를 눌렀을 때 움직임 괜찮음 => 근데 대쉬가 안됨

    // 뒷 키를 눌렀을 때 오른쪽의 움직임 => 반대로 감

    private void Jump()
    {
        if (jumpCount < jumpCountMax &&Input.GetButtonDown("Jump"))
        {

            jumpCount++;
            yVelocity = jumpPower;
        }
    }


    private void Dash()
    {
        // LeftShift 누르면 스피드 50 증가

        // leftShift 누르면 
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // 대쉬 켜져있는 상태.
            isDash = true;

            // 카메라 흔들림
            // 0.2 의 세기로 1초만큼
            CamRotate.Instance.OnShakeCamera(0.4f, 0.5f);
            //CamRotate.Instance.OnShakeCamera_Rot(0.1f, 2f);

            if (vAxis > 0)
            {
                //스피드 50 증가
                accel += 50;
            }
            else if (vAxis < 0)
            {
                accel -= 50;
            }
        }

        //대쉬가 true 이면
        if (isDash == true)
        {
            // 시간이 흐르게
            curTime += Time.deltaTime;

            // 1초가 지나거나, 속도가 100보다 작다면
            if (curTime > dashTime && accel < 150)
            {
                // isDash 는 false 로
                isDash = false;
                curTime = 0f;
            }
        }
    }
}
