using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LHS_Enemy : MonoBehaviour
{
    #region jumpAttack �Ӽ�
    [Header("������")]
    public float jumpPower = 10;

    // ��������
    // �ʿ�Ӽ� : jumpPonts �迭�� ���� ���� / �̵��ӵ� / ������ ���� ����
    [Header("����2")]
    public Transform[] target;
    public int num;

    // �÷��̾�� ����� ��ġ ����
    // �ʿ�Ӽ� : jumpPoints �迭�� ���� List / �̵� jumpPoint / ª�� �Ÿ� (Player - jumpPoint)
    [Header("����3")]
    public List<GameObject> jumpPoints;
    public GameObject jumpPoint;

    float shortDistance;

    // �ʿ�Ӽ� : �߷º��� / �����ӷº��� / ������ ���� �ӵ� / �̵��� ���� ����
    [Header("����1")]
    float gravity = -20f;
    float yVelocity = 0;
    float speed_jump = 10f;
    //float speed_dir = 5f;

    Vector3 dir;
    #endregion

    #region BulletAttack �Ӽ�
    // �ʿ�Ӽ� : �Ѿ� / �߻� ��ġ
    [Header("�Ѿ� �߻�")]
    public GameObject Bullet;
    public Transform firePos;

    public GameObject bulletGO;

    // ���� �߻��ϰ� �����̸� �ֱ����� ������ 
    float nextFire = 0.0f;
    readonly float fireRate = 0.1f;

    // ���� ź��
    private int currentBullet = 10;
    #endregion

    #region ���� ���� 
    int randAttack;
    int randWeak;
    #endregion;

    #region ȸ�� �Ӽ�
    // �ʿ�Ӽ� : Turn �ӵ� / swing �ӵ�
    [Header("ȸ�� �ӵ�")]
    public float rotateSpeed = 5;
    public float swingSpeed = 20;
    #endregion

    #region ������ �ð� �Ӽ�
    [Header("�����̽ð�")]
    // ���� ���� �� 
    public float delayTime = 2.0f;
    // ���ư��� �ð�
    public float swingTime = 1.0f;

    //float jump1DelayTime = 1.5f;
    float jump2DelayTime = 1.0f;
    float jump3DelayTime = 0.5f;

    #endregion

    #region �÷��̾� �Ÿ� �Ӽ�
    // (�÷��̾� - ��) / (�÷��̾� - ����2 target) / (�÷��̾� - ����3 point)
    [Header("�÷��̾���� �Ÿ�")]
    public float distance;
    public float jump2Distance; //jumpDistance
    public float jump3Distance; //highjumpDistance
    #endregion

    #region ���¿��� �Ӽ�
    [Header("���� ����")]
    // �ѹ��� ���� or �÷��̾ ����
    public bool isSwingAttack = false;
    // ? �߷°��� ���� ? => ����� �ʿ���
    public bool isJump = false;
    // ��⸦ ����
    public bool isWaiting = false;
    #endregion

    #region Idle Time �Ӽ�
    // ���� �ð�
    float currentTime;
    // Idle���¿��� ���� �ð�
    float createTime = 1.5f;

    //float minTime = 1.0f;
    //float maxTime = 1.5f;
    #endregion

    CharacterController cc;
    Transform player;

    // �ڽ� ��ũ��Ʈ
    LHS_Gun gunPosition;
    LHS_WNcolor wnColor;
    LHS_WNcolor2 wnColor2;

    //���� ȿ��
    public AudioSource mysfx;
    public AudioClip Weaknessfx;
    public AudioClip Jump2fx;
    public AudioClip Jump3fx;
    public AudioClip Bulletfx;

    // ����Ʈ ȿ��
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

    // ���ʹ� ���� ����
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

        // ������ ���ʹ� ���� �ʱⰪ ����
        m_State = EnemyState.Idle;

        // �ڽ��� ��ũ��Ʈ�� �����´�.
        gunPosition = GetComponentInChildren<LHS_Gun>();
        wnColor = GetComponentInChildren<LHS_WNcolor>();
        wnColor2 = GetComponentInChildren<LHS_WNcolor2>();

        // ����2 -> ���� ��ġ ��
        num = Random.Range(0, 9);

        // ����3 -> ���� ����Ʈ �迭�� ��� (�Ѳ����� �����ϱ� ����)
        // �Ÿ��� �ʱⰪ�� ���� -> ���߿� ���ϱ� ���ؼ� ���
        jumpPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("JumpPoint"));
        shortDistance = 500f;
        jumpPoint = jumpPoints[0];

    }

    // �÷��̾� �ٶ󺸱�
    private void FixedUpdate()
    {
        Turn();
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾���� �Ÿ�üũ
        DistanceCheck();
        JumpGravity();

        //���� ���¸� üũ�� �ش� ���º��� ������ ����� �����ϰ� �ϰ� �ʹ�.
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

    //************************** ���� �Լ� ************************//

    // �ð��� �帣�ٰ�
    // ���� ����ð��� �����ð��� �Ǹ� (�����ð�)
    // ������ �̵��Ѵ�
    void Idle()
    {
        currentTime += Time.deltaTime;

        if (currentTime > createTime)
        {
            ResetWeakness();

            //������ ���� �ʱ�ȭ
            speed = 3;
            currentJumpTime = 0;

            //-------------------------------------------------------------------------------------------
            startPos = transform.position;

            endPos = new Vector3(target[num].position.x, transform.position.y, target[num].position.z);

            center = (startPos + endPos) * 0.5f;

            center.y -= 3;
            isJumpAttack = true;

            //-------------------------------------------------------------------------------------------

            // (�÷��̾� - ��������Ʈ) ����� ����Ʈ ���ϱ�
            SortJumpPoint();

            startPos2 = transform.position;

            endPos2 = new Vector3(jumpPoint.transform.position.x, transform.position.y, jumpPoint.transform.position.z);

            center2 = (startPos2 + endPos2) * 0.5f;

            center2.y -= 3;
            isJumpAttack2 = true;

            //---------------------------------------------------------------------------------------------------

            // Idle -> ����
            m_State = EnemyState.Jump;

            currentTime = 0;
        }
    }

    // HP�� ����
    void Jump()
    {
        if (LHS_EnemyHP.Instance.HP >= 80)
        {
            // �������� ����.
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

    // ü���� 30 ~ 59 �� �� // �ð��� �帣�ٰ� �����ð��� �Ǹ� // ������ ��ġ�� �����ϸ� �̵��ϰ� �ʹ�. (Start �κп��� ���� �� ����)
    void Jump2Attack()
    {
        if (!isWaiting)
        {
            
            //anim.SetTrigger("Jump1");
            Parabola1();

            //�� �Ÿ��� ������ �Ÿ��� 1.5���� ������ ���� �����ϰ� �ʹ�.
            //�ʿ�Ӽ�: �������� ���� �Ÿ�
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

    // ü���� 29���� ���� �� // �ð��� �帣�ٰ� �����ð��� �Ǹ� // �÷��̾�� ��������Ʈ �Ÿ� �� ����� ����Ʈ�� �����ϸ� �̵��ϰ� �ʹ�.
    void Jump3Attack()
    {
        if (!isWaiting)
        {
            
            //anim.SetTrigger("Jump2");
            Parabola2();

            //�� �Ÿ��� ������ �Ÿ��� 1.5���� ������ ���� �����ϰ� �ʹ�.
            //�ʿ�Ӽ�: �������� ���� �Ÿ�
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

                // �ʱ� �������ֱ�
                shortDistance = 500f;
                jumpPoint = jumpPoints[0];

                m_State = EnemyState.Attack;
            }
        }
    }

    // HP���� �������� �ٸ����ϰ� �ʹ� // 1�ܰ� & 2�ܰ� : �Ѿ� & �ֵθ��� / 3�ܰ� : �Ѿ� & �ֵθ��� & ��ġ (���� ��)
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

    // ���Ÿ� ���� (20���� ũ�ٸ�) - �Ÿ��� ���� �ʴ´ٸ� // �ð��� �帣�� ������ aaaaaaad�ð��� �Ǹ� // �÷��̾� �������� �Ѿ� 10�� �߻��ϰ� �ʹ�
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
            // �ð��� �帣�� ������ �ð��� �Ǹ�
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
        //    // ������ true��� (swing ������ �� ������� /turn�� false�̱� ������ �ٽ� �������)
        //    if (isSwingAttack == true)
        //    {
        //        currentTime += Time.deltaTime;

        //        // ���� �ð���ŭ ȸ���� �ϰ� �ʹ�.
        //        if (currentTime < swingTime)
        //        {

        //            transform.Rotate(new Vector3(0, 45, 0) * swingSpeed * Time.deltaTime);
        //            // transform.rotation = Quaternion.Lerp(transform.rotation, swing, swingSpeed * Time.deltaTime);

        //            // ������ ���� GunPosition ���ش�.
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

        // �ð��� �帣�� ������ �ð��� �Ǹ�
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
        //        // ���������� ���¸� �����Ѵ�.
        //        // �ٽ� �������� �ش�
        //        if (randWeak >= 6)
        //        {
        //            anim.SetTrigger("Weakness");
        //            m_State = EnemyState.Weakness;

        //            currentTime = 0;
        //            randAttack = Random.Range(0, 10);
        //        }

        //        // ���� ��� ��
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

    //���� : HP�� ���� ����
    void Weakness()
    {
        //// �ð��� �帣�� // ����ð��� �����ð��������� �����ý����� �۵��ϰ�ʹ�. // hp�� 40�����϶��� 3�� �ƴϸ� 2�� // �÷��̾ ��´ٸ� hp�� ��´�
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
        // ���°� ����!
        if (isSwingAttack == false)
        {
            anim.SetTrigger("Die");
            isSwingAttack = true;
            wnColor2.enabled = true;
            wnColor.enabled = false;
            LHS_SceneManager.Instance.isDieOn = true;
        }
    }

    //************************** �ʿ� �Լ� *************************//

    // Waiting �ʱ�ȭ �κ�
    void ResetWaiting()
    {
        isWaiting = false;
        currentTime = 0;
    }

    // ���� �ʱ�ȭ �κ�
    void ResetWeakness()
    {
        wnColor.enabled = false;
        wnColor.enabled = false;
    }

    // ���������� ���� �ʴ´ٸ� // ��� �÷��̾� ���� �ٶ󺸰� �ʹ�
    void Turn()
    {
        if (isSwingAttack == false)
        {
            // �÷��̾ ���� ������ �ʿ�
            Vector3 dir = player.position - transform.position;

            dir.y = 0;
            dir.Normalize();

            // Quaternion.Slerp �̿��Ͽ� Player�� �ڿ������� �ٶ󺸰� �� 
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);
        }
    }

    // (�÷��̾� - ��) �Ÿ� ���
    void DistanceCheck()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
    }

    void JumpGravity()
    {
        //���̸� �߷� ����
        if (cc.isGrounded)
        {
            yVelocity = 0;

            //isJump = false;


        }

        // ���� �ƴϸ� �߷°��� ������
        else if (!cc.isGrounded)
        {
            yVelocity += gravity * Time.deltaTime;
        }

        //speed �� ������ ���� ���� or ������ �� ������ �� ����.
        //�߷°��ӵ��� ��� �ް��ִ»��� (�����Ҷ���)
        cc.Move(Vector3.up * yVelocity * speed_jump * Time.deltaTime);
        //���������Ҷ���  speed_dir �� 0�� �ƴ϶�, 2f �� �ٲ���
        //cc.Move(dir * speed_dir * Time.deltaTime);
    }

    // ��������Ʈ�� �÷��̾��� �Ÿ��� ���ϰ� // �Ÿ��� ���� ����� ������ �̵��ϰ� �ʹ�.
    void SortJumpPoint()
    {
        // ��������Ʈ �迭 �� ��ŭ �ݺ��Ѵ�
        for (int i = 0; i < jumpPoints.Count; i++)
        {
            // (��������Ʈ - �÷��̾�) �Ÿ�
            float pointDistance = Vector3.Distance(jumpPoints[i].transform.position, player.position);

            // ���� �ʱ⼳���� ������ �۴ٸ�
            if (pointDistance < shortDistance)
            {
                // ��������Ʈ���� i��° ����Ʈ�� ��������Ʈ�� ����
                jumpPoint = jumpPoints[i];
                // ���� ����� �Ÿ��� ���ϱ� ����
                shortDistance = pointDistance;
            }
        }
    }

    // �Ѿ� �߻�
    void Fire()
    {
        // Time.time �� ��ũ��Ʈ�� ������� ������ �귯���� �ð�
        // nextFire���� Time.time + ������ �ð��� ���� ��
        if (Time.time >= nextFire)
        {
            BulletEffect.Play();
            
            // �Ѿ��� �����ͼ� �ѱ� ��ġ�� ���� �ʹ�.
            GameObject _bullet = Instantiate(Bullet, firePos.position, firePos.rotation);

            //GameObject bulletGameObject = Instantiate(bulletGO, firePos.position, firePos.rotation);

            // �Ѿ��� �ϳ��� �����ش�
            currentBullet--;
            print("currentBullet" + currentBullet);

            // ������ �����̸� �ָ� �� �� ������ ������ �� �� ����
            nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);

            if (currentBullet == 9)
            {
                mysfx.PlayOneShot(Bulletfx);
            }
        }
    }

    // ������ ���ӵ��� �ޱ� ���� ����
    Vector3 startPos, endPos;
    Vector3 startPos2, endPos2;
    // ó�� ���ǵ�
    float speed = 3;
    // �����ö� ���ӵǴ� ��ġ
    public float accel = 0.06f;
    // ���� ��ȯ ����
    public bool isJumpAttack;
    public bool isJumpAttack2;
    // ���� ���� �ð�
    float currentJumpTime = 0;
    // ���� ��ġ 
    Vector3 center;
    Vector3 center2;

    // ������ ���� �Լ� (�������� �׸��� ������ �ϰ� �ʹ�)
    void Parabola1()
    {
        // �������� ���¶��
        if (isJumpAttack)
        {
            // �ð��� �帣�ٰ� * ���ǵ带 �����ش�
            currentJumpTime += Time.deltaTime * speed;
            // Slerp�� ������ ���� 0-1 �� �ð��� �����ش� -> 2�ʰ� �Ǹ� 1 �� �Ǵ� ���� 
            float ratio = currentJumpTime / 2;

            // ������������ ��ǥ�������� �̵� + ���Ͱ��� ���ְ� ������
            transform.position = Vector3.Slerp(startPos - center, endPos - center, ratio) + center;
            // ī�޶��� ��鸲�� �ش� -> �����غ��̱� ����
            LHS_CamRotate.Instance.OnShakeCamera(0.5f, 0.99f);

            // �߰������� ���ٸ� ���ӵǴ� ��ġ�� �����ش�
            if (ratio >= 0.5f)
            {
                speed += accel;
            }

            // �����ߴٸ�
            if (ratio >= 1)
            {
                JumpEffect.Play();
                mysfx.PlayOneShot(Jump2fx);

                //���� ������ �����.
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

    //************************** �߰��� �Լ� *************************//

    // ������ ���� �Լ�
    public void OnDamageProcess(int hitPower)
    {
        // �÷��̾��� ���ݷ¸�ŭ ���ʹ��� ü���� ���ҽ�Ų��
        LHS_EnemyHP.Instance.HP -= hitPower;

    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� ����� �÷��̾��̰�
        if (other.gameObject.name.Contains("Player"))
        {
            // ���� ���°� ���� ���°� �ƴ϶��
            if (m_State != EnemyState.Weakness)
            {
                print("�÷��̾�-10");
                // �÷��̾��� �Ǹ� ��´�.
                LHS_PlayerHP.Instance.HP -= 10;
            }
        }
    }
}






