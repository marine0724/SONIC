using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Linq;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 싱글톤 구현
    //------------------------------//
    public static Player Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region 속도, 점프, 방향 관련 변수
    [Header("속도")]
    public float speed = 20;

    [Header("점프관련 변수")]
    public float jumpPower = 10f;
    public float yVelocity;
    public float gravity = -30f;

    // Player 의 2단점프를 위한 변수
    int jumpCount = 0;
    int jumpCountMax = 2;

    // 방향 값
    float hAxis;
    float vAxis;
    #endregion



    //---------------------------------------------------------------------------------------------//
    //플레이어 방향
    public Vector3 dir;

    // 방향 가속
    public Vector3 subDir;

    //캐릭터컨트롤러 선언
    CharacterController cc;
    //---------------------------------------------------------------------------------------------//

    // 대쉬 줌인 줌아웃 카메라
    float curTime = 0f;
    float dashTime = 1f;

    Animator anim;


    [Header("bool 형 변수")]
    public bool isDash;
    public bool isRader;
    public bool isBuzz;
    bool isJump = false;
    bool isAttackOn;

    bool isJumpFall;

    #region 레이더 관련 변수
    [Header("타겟 캔버스")]
    public GameObject Target_Canvas;

    float curTime_2;
    float raderTime = 0.2f;

    bool isRaderReady;
    #endregion

    // 이펙트 효과
    public GameObject WeaknessEffect;
    public ParticleSystem WeaknessEffect_particle;
    public ParticleSystem Test;
    public ParticleSystem Test2;

    // 사운드효과
    public AudioSource playersfx;
    public AudioClip Weak_Attackfx;

    void Start()
    {
        //캐릭터컨트롤러 컴포넌트 가져오기
        cc = GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animator>();

    }


    string TargetOfRader = "TargetOfRader";

    void Update()
    {
        // 조작 및 방향 설정 
        InputAxis();
        SetDirection();
        SetSubDirection();
        // 움직임 관련 함수
        Move();
        Jump();
        Dash();
        AnimationController();

        // 속도가 커질 때 화면 흔들리게 하는 함수
        CameraBuzzOnOff();

        // 가까운적 찾아서 조준점 설정하는 코드
        FindNearestObjectByTag(TargetOfRader);
        RaderOnOff();
        CrossHairOnOff();

        Attack();
        WhenAttack_GameSlowdown();
    }

    private void AnimationController()
    {
        // 떨어질 때
        if (yVelocity < 0)
        {
            isJumpFall = true;
        }

        // 애니메이션에서 쓰는 변수 이름이랑, 이름이 같음
        anim.SetBool("isJumpFall", isJumpFall);

        anim.SetFloat("Boost", speed);
    }



    private void InputAxis()
    {
        #region [INPUT.GETAXIS]

        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        float speed_sum = Mathf.Abs(hAxis) + Mathf.Abs(vAxis);

        //키를 하나라도 누르기만 하면, 양수가 나
        anim.SetFloat("Speed", speed_sum);
        #endregion
    }



    private void SetDirection()
    { 
        //---------------------------------------------------------------------------------------------------------------
        #region [dir 설정]

        // 방향은 인풋의 입력에 따라서 결정
        dir = hAxis * Vector3.right + vAxis * Vector3.forward;
        // dir 정규화
        dir.Normalize();


        #endregion
        //---------------------------------------------------------------------------------------------------------------

        #region    [카메라가 보는 방향을 앞방향으로 하는 코드]


        if(!isTurnCircle)
        {
            // 카메라의 y 회전만 구해온다.
            Quaternion v3Rotation = Quaternion.Euler(0f, LHS_CamRotate.Instance.gameObject.transform.eulerAngles.y, 0f);

            //이동할 벡터를 돌린다.
            dir = v3Rotation * dir;
            //subDir = v3Rotation * subDir;

        }

        #endregion
        //---------------------------------------------------------------------------------------------------------------

        #region [방향키를 눌렀을 때, 플레이어가 그 방향을 바라보게 하는 코드]

        // 움직일 때
        if (!(hAxis == 0 && vAxis == 0))
        {
            // 지금 보고 있는 값 부터 움직이는 방향으로 바라보는 방향 값 까지 Lerp 로 회전
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 20.0f);
        }

        #endregion
        //---------------------------------------------------------------------------------------------------------------
    }

    private void SetSubDirection()
    {
        // 이 서브방향은 dir 에 영향을 미치면 안되는 서브 방향으로 만든다.

        // 구현해야하는 느낌
        // 카메라를 회전 시키면, 지금 보이는 방향으로 계속 힘을 주는 느낌이다.
        // 즉, 계속 앞으로 가다가 카메라 방향을 바꾸면, 그 방향으로 힘을 주기때문에 관성이 있는것처럼 보일 수 박에 없음

        // 그렇다면 subDir 을 축적이 아니라,  dir 의 Lerp 로 가져간다면?
        // 카메라를 보는 방향에 따라서 달라질 수도?!

        // 그러면 subDir 을 Lerp 로 dir 크기의 2배가 되게 한다면?
        // dir 에 어느정도 종속 당하면서도, 힘을 주는것 같이 보이지 않을까

        #region 서브벡터를 원래 벡터의 Lerp로

        subDir = Vector3.Lerp(subDir, 3 * dir, Time.deltaTime);

        #endregion
    }

    private void Move() //Player 움직이기
    {

        // speed 를 나눈건, 점프값이 너무 높아지지 않기 위해서 이다.
        // [dir 설정] 에 넣으면, 점프할때 player 가 위를 보게 됨.

        #region [이동]

            dir.y =  5 * yVelocity / speed;

            // 공격상태가 아닐때는 이렇게 움직이게 한다!
            if(!isAttackOn)
            cc.Move( (subDir + dir) * speed * Time.deltaTime);      
        
        #endregion
    }

    private void Jump()
    {
        // 땅에 닿아 있거나 isAttackOn 이 켜져있을 때
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            //WeaknessEffect.SetActive(false);

            isWeaknessAttack = false;
            yVelocity = 0;
            jumpCount = 0;
            isJump = false;
            isJumpFall = false;

        }
        // 공격중일 때
        else if (isAttackOn)
        {
            yVelocity = 0;
        }

        // 점프 중일때
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && jumpCount < jumpCountMax && !isRaderReady)
        {
            anim.Play("Jump");
            print(jumpCount);
            isJump = true;
            isJumpFall = false;
            yVelocity = jumpPower;
            jumpCount++;
            
        }

    }

    public void Dash()
    {
        // LeftShift 누르면 스피드 50 증가

        // 속도가 10이 넘으면 계속 감속하게 해야함.
        if (speed > 20)
        {
            speed -= 30 * Time.deltaTime;
        }
        else
        {
            speed = 20;
        }

        // leftShift 누르면 
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // 대쉬 켜져있는 상태.
            isDash = true;

            // 카메라 흔들림
            // 0.2 의 세기로 1초만큼
            LHS_CamRotate.Instance.OnShakeCamera(0.4f, 0.5f);
            //CamRotate.Instance.OnShakeCamera_Rot(0.1f, 2f);

            speed += 50;
        }

        //대쉬가 true 이면
        if (isDash == true)
        {
            // 시간이 흐르게
            curTime += Time.deltaTime;

            // 1초가 지나거나, 속도가 100보다 작다면
            if (curTime > dashTime && speed < 80)
            {
                // isDash 는 false 로
                isDash = false;
                curTime = 0f;
            }
        }
    }

    private void CameraBuzzOnOff()
    {    
        // 속도가 100보다 커지면, 화면 흔들리게 하는 코드
         // 직접적인 흔들림은 CamShake.cs 에 있다.
        if (speed > 80)
        {
            isBuzz = true;
        }
        else
        {
            isBuzz = false;
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------------


    // TargetOfRader 의 태그로 된 오브젝트 중 가장 가까이에 있는 오브젝트를 찾는 코드
    private GameObject FindNearestObjectByTag(string tag)
    {
        // 탐색할 오브젝트 목록을 List 에 저장
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ 메소드를 이용해 가장 가까운 적을 찾는다.
        var neareastObject = objects.OrderBy(obj =>
        {
            return Vector3.Distance(transform.position, obj.transform.position);
        }).FirstOrDefault();

        return neareastObject;

    }

    private void RaderOnOff()
    {
        // 적을 감지 하는 레이더가 필요함
        #region 레이더 만들기

        // 적과의 거리가 2보다 작고 isRader 가 false 이고,
        // 점프하고있는 중이고,
        // Player 와 마주보고 있는 상태라면 ( (오브젝트에서 플레이어까지의 벡터)의 크기 > (오브젝트에서 플레이어까지의 벡터의 크기 + 플레이어의 forward 벡터)의 크기)
        if (Vector3.Distance(transform.position, FindNearestObjectByTag(TargetOfRader).transform.position) < 60 && isJump == true 
            && (transform.position - FindNearestObjectByTag(TargetOfRader).transform.position).magnitude >
            (transform.position - FindNearestObjectByTag(TargetOfRader).transform.position + transform.forward).magnitude)
        {
            isRader = true;
        }
        else
            isRader = false;

        #endregion
    }


    private void CrossHairOnOff()
    {
        // isRader 가 켜지면, 가장 가까운 오브젝트에게 TargetCanvas 를 키게 하고싶다.aaaaaaaaaaaaadddd
        if (isRader == true)
        {
            if (LHS_Enemy.Instance.m_State == LHS_Enemy.EnemyState.Weakness)
            {
                //조준점을 키고
                
                Target_Canvas.SetActive(true);

                //나한테 가져다 놓기
                Target_Canvas.transform.position = FindNearestObjectByTag(TargetOfRader).transform.position + (transform.position - FindNearestObjectByTag(TargetOfRader).transform.position).normalized * 5;
                //계속 플레이어를 바라보게
                Target_Canvas.transform.forward = transform.position - FindNearestObjectByTag(TargetOfRader).transform.position;

                // 0.2초 있다가 Ready 되게 하고싶다.
                curTime_2 += Time.deltaTime;
                if (curTime_2 > raderTime)
                {
                    // 이미지의 색을 빨강으로 바꾸고
                    //Target_Canvas.GetComponent<Image>().color = Color.red;
                    // 레디 온
                    
                    isRaderReady = true;
                    //Target_Canvas.SetActive(false);
                }
            }
        }

        // isRader 가 true 가 아니면 그냥 끄자
        else
        {
            //Target_Canvas.GetComponent<Image>().color = Color.blue;
            curTime_2 = 0;
            isRaderReady = false;
            Target_Canvas.SetActive(false);
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------------
    // 조준점이 시간에 따라서, isRaderReady 까지 구현되는건 완료함!
    // 근데 가까이에 있는 적이 바뀐다면 isRaderReady 를 다시 초기화 시켜줘야 하는데 이 부분이 아직 안됨!
    //--------------------------------------------------------------------------------------------------------------------------------------------------



    // 키를 누르면 어택온이 뜨고,
    // AttackOn 상태 일때는, 충돌 할때까지 박치기를 하고싶다.
    // 이 AttackOn 상태 일때는 gravity 에 영향을 받지 않게 한다.
    // 박치기를 하고나면 AttackOn 이 풀리게 되고, 점프값을 주어 점프시킨다.

    private void Attack()
    {
        // isRaderReady 가 켜져있을 때 점프 키를 누르면
        // 켜진 오브젝트에게 박치기 하고싶다.

        // 타겟까지의 방향
        Vector3 dir = (FindNearestObjectByTag(TargetOfRader).transform.position - transform.position).normalized;

        // RaderReady 가 준비되어있는 상태에서
        if (isRaderReady == true)
        { 
            // 키를 누르면
            if(Input.GetKeyDown(KeyCode.Space))
            {
                // 어택할 수 있는 상태가 됨
                isAttackOn = true;
            }

        }
        // 여기서 어택 할 수 있는 상태가 되면
        if (isAttackOn == true)
        {
            // 어택 방향으로 박치기
            cc.Move(dir * 400 * Time.deltaTime);
        }

    }

    private void WhenAttack_GameSlowdown()
    {
        // 공격 성공하면, slowFinishTime 동안 게임이 0.3만큼 느려짐
        if (isAttackSucces == true)
        {
            Time.timeScale = 0.2f;

            curTime_3 += Time.deltaTime;
            if (curTime_3 > slowFinishTime)
            {
                Time.timeScale = 1;
                curTime_3 = 0;
                isAttackSucces = false;
            }

        }
    }


    //----------------------------------------------------------------------------------------------------------------------------------------------------


    public bool isTurnCircle;


    float curTime_3;
    float slowFinishTime = 0.03f;

    bool isAttackSucces;

    bool isWeaknessAttack;
    bool isWeakness_effect;


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 만약 맞은 물체의 태그가 TargetOfRader 라면
        // isAttackOn 을 끈다.
        if (hit.gameObject.tag == "TargetOfRader")
        {
            // 약점 상태라면 이펙트 효과들 / 카메라 흔들림 점프 / 땅에 닿으면 사운드 재생
            if (LHS_Enemy.Instance.m_State == LHS_Enemy.EnemyState.Weakness)
            {
                Test.Play();
                Test2.Play();
                WeaknessEffect.SetActive(true);
                WeaknessEffect_particle.Stop();
                WeaknessEffect_particle.Play();
                WeaknessEffect.transform.position = transform.position + new Vector3(0, 0, 0.5f);

                //카메라 흔들고
                LHS_CamRotate.Instance.OnShakeCamera(0.2f, 0.3f);

                // 시간 느려지게 하는것 때문에 함
                isAttackSucces = true;

                // 점프 한번 주고
                yVelocity = jumpPower;


                // isAttackOn 을 끈다
                isAttackOn = false;
                //Destroy(hit.gameObject);

                // 만약에 weakness 상태일때 attack을 당하지 않았다면
                // 만약에 enemy가 WEAKNESS 상ㅊㅌ아 미안미안


                // 플레이어가 땅에 닿았을 때
                if (!isWeaknessAttack)
                {
                    playersfx.PlayOneShot(Weak_Attackfx);
                    LHS_EnemyHP.Instance.HP -= 10;
                    isWeaknessAttack = true;
                }
            }
        }
    }
}
