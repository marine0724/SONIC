using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LHS_Enemy : MonoBehaviour
{
    #region jumpAttack 속성
    [Header("점프력")]
    public float jumpPower = 10;

    // 랜덤점프
    // 필요속성 : jumpPonts 배열을 담을 변수 / 이동속도 / 랜덤을 담을 변수
    [Header("점프2")]
    public Transform[] target;
    public int num;

    // 플레이어와 가까운 위치 점프
    // 필요속성 : jumpPoints 배열을 담을 List / 이동 jumpPoint / 짧은 거리 (Player - jumpPoint)
    [Header("점프3")]
    public List<GameObject> jumpPoints;
    public GameObject jumpPoint;

    float shortDistance;

    // 필요속성 : 중력변수 / 수직속력변수 / 점프를 위한 속도 / 이동을 위한 방향
    [Header("점프1")]
    float gravity = -20f;
    float yVelocity = 0;
    float speed_jump = 10f;
    //float speed_dir = 5f;

    Vector3 dir;
    #endregion

    #region BulletAttack 속성
    // 필요속성 : 총알 / 발사 위치
    [Header("총알 발사")]
    public GameObject Bullet;
    public Transform firePos;

    public GameObject bulletGO;

    // 총을 발사하고 딜레이를 주기위한 변수들 
    float nextFire = 0.0f;
    readonly float fireRate = 0.1f;

    // 현재 탄수
    private int currentBullet = 10;
    #endregion

    #region 랜덤 변수 
    int randAttack;
    int randWeak;
    #endregion;

    #region 회전 속성
    // 필요속성 : Turn 속도 / swing 속도
    [Header("회전 속도")]
    public float rotateSpeed = 5;
    public float swingSpeed = 20;
    #endregion

    #region 딜레이 시간 속성
    [Header("딜레이시간")]
    // 상태 실행 전 
    public float delayTime = 2.0f;
    // 돌아가는 시간
    public float swingTime = 1.0f;

    //float jump1DelayTime = 1.5f;
    float jump2DelayTime = 1.0f;
    float jump3DelayTime = 0.5f;

    #endregion

    #region 플레이어 거리 속성
    // (플레이어 - 나) / (플레이어 - 점프2 target) / (플레이어 - 점프3 point)
    [Header("플레이어와의 거리")]
    public float distance;
    public float jump2Distance; //jumpDistance
    public float jump3Distance; //highjumpDistance
    #endregion

    #region 상태여부 속성
    [Header("상태 여부")]
    // 한바퀴 돌기 or 플레이어를 보기
    public bool isSwingAttack = false;
    // ? 중력값을 위해 ? => 고민이 필요함
    public bool isJump = false;
    // 대기를 위한
    public bool isWaiting = false;
    #endregion

    #region Idle Time 속성
    // 현재 시간
    float currentTime;
    // Idle상태에서 랜덤 시간
    float createTime = 1.5f;

    //float minTime = 1.0f;
    //float maxTime = 1.5f;
    #endregion

    CharacterController cc;
    Transform player;

    // 자식 스크립트
    LHS_Gun gunPosition;
    LHS_WNcolor wnColor;
    LHS_WNcolor2 wnColor2;

    //사운드 효과
    public AudioSource mysfx;
    public AudioClip Weaknessfx;
    public AudioClip Jump2fx;
    public AudioClip Jump3fx;
    public AudioClip Bulletfx;

    // 이펙트 효과
    public ParticleSystem JumpEffect;
    public ParticleSystem BulletEffect;


    public enum EnemyState
    {
        Idle,
        Attack,
        Jump,
        Weakness,
        Die
    }

    public enum EnemyAttackState
    {
        BulletAttack,
        //SwingAttack,
        //HammerAttack
    }

    public enum EnemyJumpState
    {
        Jump1Attack,
        Jump2Attack,
        Jump3Attack
    }

    // 에너미 상태 변수
    public EnemyState m_State;
    public EnemyAttackState m_attackSM;
    public EnemyJumpState m_JumpSM;

    public static LHS_Enemy Instance;

    Animator anim;

    private void Awake()
    {
        Instance = this;

        cc = GetComponent<CharacterController>();

        player = GameObject.Find("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponentInChildren<Animator>();

        // 최초의 에너미 상태 초기값 설정
        m_State = EnemyState.Idle;

        // 자식의 스크립트를 가져온다.
        gunPosition = GetComponentInChildren<LHS_Gun>();
        wnColor = GetComponentInChildren<LHS_WNcolor>();
        wnColor2 = GetComponentInChildren<LHS_WNcolor2>();

        // 점프2 -> 랜덤 위치 값
        num = Random.Range(0, 9);

        // 점프3 -> 점프 포인트 배열에 담기 (한꺼번에 관리하기 위해)
        // 거리의 초기값을 설정 -> 나중에 비교하기 위해서 사용
        jumpPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("JumpPoint"));
        shortDistance = 500f;
        jumpPoint = jumpPoints[0];

    }

    // 플레이어 바라보기
    private void FixedUpdate()
    {
        Turn();
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어와의 거리체크
        DistanceCheck();
        JumpGravity();

        //현재 상태를 체크해 해당 상태별로 정해진 기능을 수행하게 하고 싶다.
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Jump:
                Jump();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Weakness:
                Weakness();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    //************************** 상태 함수 ************************//

    // 시간이 흐르다가
    // 만약 현재시간이 일정시간이 되면 (랜덤시간)
    // 점프로 이동한다
    void Idle()
    {
        currentTime += Time.deltaTime;

        if (currentTime > createTime)
        {
            ResetWeakness();

            //점프를 위한 초기화
            speed = 3;
            currentJumpTime = 0;

            //-------------------------------------------------------------------------------------------
            startPos = transform.position;

            endPos = new Vector3(target[num].position.x, transform.position.y, target[num].position.z);

            center = (startPos + endPos) * 0.5f;

            center.y -= 3;
            isJumpAttack = true;

            //-------------------------------------------------------------------------------------------

            // (플레이어 - 점프포인트) 가까운 포인트 구하기
            SortJumpPoint();

            startPos2 = transform.position;

            endPos2 = new Vector3(jumpPoint.transform.position.x, transform.position.y, jumpPoint.transform.position.z);

            center2 = (startPos2 + endPos2) * 0.5f;

            center2.y -= 3;
            isJumpAttack2 = true;

            //---------------------------------------------------------------------------------------------------

            // Idle -> 공격
            m_State = EnemyState.Jump;

            currentTime = 0;
        }
    }

    // HP로 나눔
    void Jump()
    {
        if (LHS_EnemyHP.Instance.HP >= 80)
        {
            // 공격으로 간다.
            m_JumpSM = EnemyJumpState.Jump1Attack;
        }

        else if (LHS_EnemyHP.Instance.HP >= 40 && LHS_EnemyHP.Instance.HP < 80)
        {
            anim.SetTrigger("Jump1");
            m_JumpSM = EnemyJumpState.Jump2Attack;

        }

        else if (LHS_EnemyHP.Instance.HP >= 0 && LHS_EnemyHP.Instance.HP < 40)
        {
            anim.SetTrigger("Jump2");
            m_JumpSM = EnemyJumpState.Jump3Attack;
        }

        switch (m_JumpSM)
        {
            case EnemyJumpState.Jump1Attack:
                Jump1Attack();
                break;
            case EnemyJumpState.Jump2Attack:
                Jump2Attack();
                break;
            case EnemyJumpState.Jump3Attack:
                Jump3Attack();
                break;
        }
    }

    void Jump1Attack()
    {
        if (!isWaiting)
        {
            m_State = EnemyState.Attack;
        }
    }

    // 체력이 30 ~ 59 일 때 // 시간이 흐르다가 일정시간이 되면 // 랜덤한 위치로 점프하며 이동하고 싶다. (Start 부분에서 랜덤 값 설정)
    void Jump2Attack()
    {
        if (!isWaiting)
        {
            
            //anim.SetTrigger("Jump1");
            Parabola1();

            //내 거리와 목적지 거리가 1.5보다 작으면 상태 전의하고 싶다.
            //필요속성: 목적지와 나의 거리
            jump2Distance = Vector3.Distance(transform.position, target[num].position);

            if (jump2Distance < 2f)
            {
                isWaiting = true;
            }
        }

        else
        {
            currentTime += Time.deltaTime;

            if (currentTime > jump2DelayTime)
            {
                ResetWaiting();

                num = Random.Range(0, 9);

                m_State = EnemyState.Attack;
            }
        }
    }

    // 체력이 29보다 작을 때 // 시간이 흐르다가 일정시간이 되면 // 플레이어와 점프포인트 거리 중 가까운 포인트로 점프하며 이동하고 싶다.
    void Jump3Attack()
    {
        if (!isWaiting)
        {
            
            //anim.SetTrigger("Jump2");
            Parabola2();

            //내 거리와 목적지 거리가 1.5보다 작으면 상태 전의하고 싶다.
            //필요속성: 목적지와 나의 거리
            jump3Distance = Vector3.Distance(transform.position, jumpPoint.transform.position);

            if (jump3Distance < 2f)
            {
                isWaiting = true;
            }
        }

        else
        {
            currentTime += Time.deltaTime;

            if (currentTime > jump3DelayTime)
            {
                ResetWaiting();

                // 초기 설정해주기
                shortDistance = 500f;
                jumpPoint = jumpPoints[0];

                m_State = EnemyState.Attack;
            }
        }
    }

    // HP마다 공격패턴 다르게하고 싶다 // 1단계 & 2단계 : 총알 & 휘두르기 / 3단계 : 총알 & 휘두르기 & 망치 (랜덤 값)
    void Attack()
    {

        if (LHS_EnemyHP.Instance.HP >= 80)
        {

            //anim.SetTrigger("Bullet");
            m_attackSM = EnemyAttackState.BulletAttack;

        }

        else if (LHS_EnemyHP.Instance.HP >= 40 && LHS_EnemyHP.Instance.HP < 80)
        {
            //anim.SetTrigger("Bullet");
            m_attackSM = EnemyAttackState.BulletAttack;
        }

        else if (LHS_EnemyHP.Instance.HP < 40)
        {
            //anim.SetTrigger("Bullet");
            m_attackSM = EnemyAttackState.BulletAttack;
        }

        switch (m_attackSM)
        {
            case EnemyAttackState.BulletAttack:
                BulletAttack();
                break;
        }
    }

    // 원거리 공격 (20보다 크다면) - 거리에 들지 않는다면 // 시간이 흐르다 딜레이 aaaaaaad시간이 되면 // 플레이어 방향으로 총알 10발 발사하고 싶다
    void BulletAttack()
    {
        if (!isWaiting)
        {
            if (currentBullet > 0)
            {

                anim.SetTrigger("Bullet");
                Fire();
            }

            else
            {
                isWaiting = true;
            }
        }

        else
        {
            // 시간이 흐르다 딜레이 시간이 되면
            currentTime += Time.deltaTime;

            if (currentTime > delayTime)
            {
                currentBullet = 10;
                ResetWaiting();

                anim.SetTrigger("Weakness");
                mysfx.PlayOneShot(Weaknessfx);
                m_State = EnemyState.Weakness;
            }
        }
    }

    void SwingAttack()
    {
        //if (!isWaiting)
        //{
        //    // 스윙이 true라면 (swing 들어오기 전 켜줘야함 /turn은 false이기 때문에 다시 꺼줘야함)
        //    if (isSwingAttack == true)
        //    {
        //        currentTime += Time.deltaTime;

        //        // 스윙 시간만큼 회전을 하고 싶다.
        //        if (currentTime < swingTime)
        //        {

        //            transform.Rotate(new Vector3(0, 45, 0) * swingSpeed * Time.deltaTime);
        //            // transform.rotation = Quaternion.Lerp(transform.rotation, swing, swingSpeed * Time.deltaTime);

        //            // 스윙할 때는 GunPosition 꺼준다.
        //            gunPosition.enabled = false;
        //        }

        //        else
        //        {
        //            isWaiting = true;
        //        }
        //    }
        //}

        //else
        //{
        //    gunPosition.enabled = true;
        //    isSwingAttack = false;

        //    ResetWaiting();

        //    randAttack = Random.Range(0, 10);

        //    anim.SetTrigger("Weakness");
        //    m_State = EnemyState.Weakness;
        //}

        // 시간이 흐르다 딜레이 시간이 되면
        //currentTime += Time.deltaTime;

        //if (currentTime > delayTime)
        //{
        //    currentBullet = 10;
        //    //ResetWaiting();

        //    randAttack = Random.Range(0, 10);

        //    anim.SetTrigger("Weakness");
        //    m_State = EnemyState.Weakness;
        //}

    }

    void HammerAttack()
    {
        //currentTime += Time.deltaTime;

        //if (currentTime > delayTime)
        //{
        //    anim.SetTrigger("Weakness");
        //    m_State = EnemyState.Weakness;

        //    currentTime = 0;
        //}

        //if (distance < 30)
        //{
        //    currentTime += Time.deltaTime;

        //    if (currentTime > delayTime)
        //    {
        //        // 랜덤값으로 상태를 전의한다.
        //        // 다시 랜덤값을 준다
        //        if (randWeak >= 6)
        //        {
        //            anim.SetTrigger("Weakness");
        //            m_State = EnemyState.Weakness;

        //            currentTime = 0;
        //            randAttack = Random.Range(0, 10);
        //        }

        //        // 조건 고민 중
        //        else
        //        {
        //            m_State = EnemyState.Jump;

        //            currentTime = 0;
        //            randAttack = Random.Range(0, 10);
        //        }
        //    }
        //else
        //{
        //    m_State = EnemyState.Idle;
        //}
    }

    //약점 : HP가 따라 나눔
    void Weakness()
    {
        //// 시간이 흐르다 // 현재시간이 약점시간때까지만 약점시스템을 작동하고싶다. // hp가 40이하일때는 3초 아니면 2초 // 플레이어가 닿는다면 hp를 깎는다
        if (!isWaiting)
        {

            currentTime += Time.deltaTime;

            if (currentTime < 3.0f)
            {
                wnColor.enabled = true;
            }

            else
            {
                isWaiting = true;
            }
        }

        else
        {
            ResetWaiting();

            anim.SetTrigger("Idle");
            wnColor2.enabled = true;
            m_State = EnemyState.Idle;
        }
    }




    void Die()
    {
        // 도는거 멈춤!
        if (isSwingAttack == false)
        {
            anim.SetTrigger("Die");
            isSwingAttack = true;
            wnColor2.enabled = true;
            wnColor.enabled = false;
            LHS_SceneManager.Instance.isDieOn = true;
        }
    }

    //************************** 필요 함수 *************************//

    // Waiting 초기화 부분
    void ResetWaiting()
    {
        isWaiting = false;
        currentTime = 0;
    }

    // 약점 초기화 부분
    void ResetWeakness()
    {
        wnColor.enabled = false;
        wnColor.enabled = false;
    }

    // 스윙공격을 하지 않는다면 // 계속 플레이어 쪽을 바라보고 싶다
    void Turn()
    {
        if (isSwingAttack == false)
        {
            // 플레이어를 보는 방향이 필요
            Vector3 dir = player.position - transform.position;

            dir.y = 0;
            dir.Normalize();

            // Quaternion.Slerp 이용하여 Player를 자연스럽게 바라보게 함 
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);
        }
    }

    // (플레이어 - 나) 거리 계산
    void DistanceCheck()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
    }

    void JumpGravity()
    {
        //땅이면 중력 없음
        if (cc.isGrounded)
        {
            yVelocity = 0;

            //isJump = false;


        }

        // 땅이 아니면 중력값을 더해짐
        else if (!cc.isGrounded)
        {
            yVelocity += gravity * Time.deltaTime;
        }

        //speed 의 종류에 따라서 점프 or 움직임 을 구사할 수 있음.
        //중력가속도를 계속 받고있는상태 (점프할때만)
        cc.Move(Vector3.up * yVelocity * speed_jump * Time.deltaTime);
        //움직여야할때는  speed_dir 을 0이 아니라, 2f 로 바꿔줌
        //cc.Move(dir * speed_dir * Time.deltaTime);
    }

    // 점프포인트와 플레이어의 거리를 구하고 // 거리가 가장 가까운 곳으로 이동하고 싶다.
    void SortJumpPoint()
    {
        // 점프포인트 배열 수 만큼 반복한다
        for (int i = 0; i < jumpPoints.Count; i++)
        {
            // (점프포인트 - 플레이어) 거리
            float pointDistance = Vector3.Distance(jumpPoints[i].transform.position, player.position);

            // 만약 초기설정한 값보다 작다면
            if (pointDistance < shortDistance)
            {
                // 점프포인트들의 i번째 포인트를 점프포인트로 지정
                jumpPoint = jumpPoints[i];
                // 가장 가까운 거리를 구하기 위해
                shortDistance = pointDistance;
            }
        }
    }

    // 총알 발사
    void Fire()
    {
        // Time.time 은 스크립트가 실행됐을 때부터 흘러가는 시간
        // nextFire에는 Time.time + 딜레이 시간이 들어가게 됨
        if (Time.time >= nextFire)
        {
            BulletEffect.Play();
            
            // 총알을 가져와서 총구 위치에 놓고 싶다.
            GameObject _bullet = Instantiate(Bullet, firePos.position, firePos.rotation);

            //GameObject bulletGameObject = Instantiate(bulletGO, firePos.position, firePos.rotation);

            // 총알을 하나씩 없애준다
            currentBullet--;
            print("currentBullet" + currentBullet);

            // 랜덤한 딜레이를 주면 좀 더 리얼한 느낌을 줄 수 있음
            nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);

            if (currentBullet == 9)
            {
                mysfx.PlayOneShot(Bulletfx);
            }
        }
    }

    // 점프의 가속도를 받기 위한 변수
    Vector3 startPos, endPos;
    Vector3 startPos2, endPos2;
    // 처음 스피드
    float speed = 3;
    // 내려올때 가속되는 수치
    public float accel = 0.06f;
    // 상태 변환 변수
    public bool isJumpAttack;
    public bool isJumpAttack2;
    // 점프 현재 시간
    float currentJumpTime = 0;
    // 센터 위치 
    Vector3 center;
    Vector3 center2;

    // 점프를 위한 함수 (포물선을 그리며 점프를 하고 싶다)
    void Parabola1()
    {
        // 점프공격 상태라면
        if (isJumpAttack)
        {
            // 시간이 흐르다가 * 스피드를 곱해준다
            currentJumpTime += Time.deltaTime * speed;
            // Slerp의 도착을 위한 0-1 을 시간을 나눠준다 -> 2초가 되면 1 이 되는 거임 
            float ratio = currentJumpTime / 2;

            // 현재지점에서 목표지점으로 이동 + 센터값을 빼주고 더해줌
            transform.position = Vector3.Slerp(startPos - center, endPos - center, ratio) + center;
            // 카메라의 흔들림을 준다 -> 강력해보이기 위해
            LHS_CamRotate.Instance.OnShakeCamera(0.5f, 0.99f);

            // 중간지점에 갔다면 가속되는 수치를 더해준다
            if (ratio >= 0.5f)
            {
                speed += accel;
            }

            // 도착했다면
            if (ratio >= 1)
            {
                JumpEffect.Play();
                mysfx.PlayOneShot(Jump2fx);

                //점프 공격을 멈춘다.
                isJumpAttack = false;
            }

        }

    }

    void Parabola2()
    {
        if (isJumpAttack2)
        {
            currentJumpTime += Time.deltaTime * speed;
            float ratio = currentJumpTime / 2;

            transform.position = Vector3.Slerp(startPos2 - center2, endPos2 - center2, ratio) + center2;
            LHS_CamRotate.Instance.OnShakeCamera(0.5f, 0.9f);

            if (ratio >= 0.5f)
            {
                speed += accel;
            }

            if (ratio >= 1)
            {
                JumpEffect.Play();
                mysfx.PlayOneShot(Jump3fx);
                isJumpAttack2 = false;
            }
        }
    }

    //************************** 추가할 함수 *************************//

    // 데미지 실행 함수
    public void OnDamageProcess(int hitPower)
    {
        // 플레이어의 공격력만큼 에너미의 체력을 감소시킨다
        LHS_EnemyHP.Instance.HP -= hitPower;

    }

    private void OnTriggerEnter(Collider other)
    {
        // 닿인 대상이 플레이어이고
        if (other.gameObject.name.Contains("Player"))
        {
            // 나의 상태가 약점 상태가 아니라면
            if (m_State != EnemyState.Weakness)
            {
                print("플레이어-10");
                // 플레이어의 피를 깎는다.
                LHS_PlayerHP.Instance.HP -= 10;
            }
        }
    }
}






