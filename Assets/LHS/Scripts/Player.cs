using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Linq;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region �̱��� ����
    //------------------------------//
    public static Player Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region �ӵ�, ����, ���� ���� ����
    [Header("�ӵ�")]
    public float speed = 20;

    [Header("�������� ����")]
    public float jumpPower = 10f;
    public float yVelocity;
    public float gravity = -30f;

    // Player �� 2�������� ���� ����
    int jumpCount = 0;
    int jumpCountMax = 2;

    // ���� ��
    float hAxis;
    float vAxis;
    #endregion



    //---------------------------------------------------------------------------------------------//
    //�÷��̾� ����
    public Vector3 dir;

    // ���� ����
    public Vector3 subDir;

    //ĳ������Ʈ�ѷ� ����
    CharacterController cc;
    //---------------------------------------------------------------------------------------------//

    // �뽬 ���� �ܾƿ� ī�޶�
    float curTime = 0f;
    float dashTime = 1f;

    Animator anim;


    [Header("bool �� ����")]
    public bool isDash;
    public bool isRader;
    public bool isBuzz;
    bool isJump = false;
    bool isAttackOn;

    bool isJumpFall;

    #region ���̴� ���� ����
    [Header("Ÿ�� ĵ����")]
    public GameObject Target_Canvas;

    float curTime_2;
    float raderTime = 0.2f;

    bool isRaderReady;
    #endregion

    // ����Ʈ ȿ��
    public GameObject WeaknessEffect;
    public ParticleSystem WeaknessEffect_particle;
    public ParticleSystem Test;
    public ParticleSystem Test2;

    // ����ȿ��
    public AudioSource playersfx;
    public AudioClip Weak_Attackfx;

    void Start()
    {
        //ĳ������Ʈ�ѷ� ������Ʈ ��������
        cc = GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animator>();

    }


    string TargetOfRader = "TargetOfRader";

    void Update()
    {
        // ���� �� ���� ���� 
        InputAxis();
        SetDirection();
        SetSubDirection();
        // ������ ���� �Լ�
        Move();
        Jump();
        Dash();
        AnimationController();

        // �ӵ��� Ŀ�� �� ȭ�� ��鸮�� �ϴ� �Լ�
        CameraBuzzOnOff();

        // ������� ã�Ƽ� ������ �����ϴ� �ڵ�
        FindNearestObjectByTag(TargetOfRader);
        RaderOnOff();
        CrossHairOnOff();

        Attack();
        WhenAttack_GameSlowdown();
    }

    private void AnimationController()
    {
        // ������ ��
        if (yVelocity < 0)
        {
            isJumpFall = true;
        }

        // �ִϸ��̼ǿ��� ���� ���� �̸��̶�, �̸��� ����
        anim.SetBool("isJumpFall", isJumpFall);

        anim.SetFloat("Boost", speed);
    }



    private void InputAxis()
    {
        #region [INPUT.GETAXIS]

        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        float speed_sum = Mathf.Abs(hAxis) + Mathf.Abs(vAxis);

        //Ű�� �ϳ��� �����⸸ �ϸ�, ����� ��
        anim.SetFloat("Speed", speed_sum);
        #endregion
    }



    private void SetDirection()
    { 
        //---------------------------------------------------------------------------------------------------------------
        #region [dir ����]

        // ������ ��ǲ�� �Է¿� ���� ����
        dir = hAxis * Vector3.right + vAxis * Vector3.forward;
        // dir ����ȭ
        dir.Normalize();


        #endregion
        //---------------------------------------------------------------------------------------------------------------

        #region    [ī�޶� ���� ������ �չ������� �ϴ� �ڵ�]


        if(!isTurnCircle)
        {
            // ī�޶��� y ȸ���� ���ؿ´�.
            Quaternion v3Rotation = Quaternion.Euler(0f, LHS_CamRotate.Instance.gameObject.transform.eulerAngles.y, 0f);

            //�̵��� ���͸� ������.
            dir = v3Rotation * dir;
            //subDir = v3Rotation * subDir;

        }

        #endregion
        //---------------------------------------------------------------------------------------------------------------

        #region [����Ű�� ������ ��, �÷��̾ �� ������ �ٶ󺸰� �ϴ� �ڵ�]

        // ������ ��
        if (!(hAxis == 0 && vAxis == 0))
        {
            // ���� ���� �ִ� �� ���� �����̴� �������� �ٶ󺸴� ���� �� ���� Lerp �� ȸ��
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 20.0f);
        }

        #endregion
        //---------------------------------------------------------------------------------------------------------------
    }

    private void SetSubDirection()
    {
        // �� ��������� dir �� ������ ��ġ�� �ȵǴ� ���� �������� �����.

        // �����ؾ��ϴ� ����
        // ī�޶� ȸ�� ��Ű��, ���� ���̴� �������� ��� ���� �ִ� �����̴�.
        // ��, ��� ������ ���ٰ� ī�޶� ������ �ٲٸ�, �� �������� ���� �ֱ⶧���� ������ �ִ°�ó�� ���� �� �ڿ� ����

        // �׷��ٸ� subDir �� ������ �ƴ϶�,  dir �� Lerp �� �������ٸ�?
        // ī�޶� ���� ���⿡ ���� �޶��� ����?!

        // �׷��� subDir �� Lerp �� dir ũ���� 2�谡 �ǰ� �Ѵٸ�?
        // dir �� ������� ���� ���ϸ鼭��, ���� �ִ°� ���� ������ ������

        #region ���꺤�͸� ���� ������ Lerp��

        subDir = Vector3.Lerp(subDir, 3 * dir, Time.deltaTime);

        #endregion
    }

    private void Move() //Player �����̱�
    {

        // speed �� ������, �������� �ʹ� �������� �ʱ� ���ؼ� �̴�.
        // [dir ����] �� ������, �����Ҷ� player �� ���� ���� ��.

        #region [�̵�]

            dir.y =  5 * yVelocity / speed;

            // ���ݻ��°� �ƴҶ��� �̷��� �����̰� �Ѵ�!
            if(!isAttackOn)
            cc.Move( (subDir + dir) * speed * Time.deltaTime);      
        
        #endregion
    }

    private void Jump()
    {
        // ���� ��� �ְų� isAttackOn �� �������� ��
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            //WeaknessEffect.SetActive(false);

            isWeaknessAttack = false;
            yVelocity = 0;
            jumpCount = 0;
            isJump = false;
            isJumpFall = false;

        }
        // �������� ��
        else if (isAttackOn)
        {
            yVelocity = 0;
        }

        // ���� ���϶�
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
        // LeftShift ������ ���ǵ� 50 ����

        // �ӵ��� 10�� ������ ��� �����ϰ� �ؾ���.
        if (speed > 20)
        {
            speed -= 30 * Time.deltaTime;
        }
        else
        {
            speed = 20;
        }

        // leftShift ������ 
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // �뽬 �����ִ� ����.
            isDash = true;

            // ī�޶� ��鸲
            // 0.2 �� ����� 1�ʸ�ŭ
            LHS_CamRotate.Instance.OnShakeCamera(0.4f, 0.5f);
            //CamRotate.Instance.OnShakeCamera_Rot(0.1f, 2f);

            speed += 50;
        }

        //�뽬�� true �̸�
        if (isDash == true)
        {
            // �ð��� �帣��
            curTime += Time.deltaTime;

            // 1�ʰ� �����ų�, �ӵ��� 100���� �۴ٸ�
            if (curTime > dashTime && speed < 80)
            {
                // isDash �� false ��
                isDash = false;
                curTime = 0f;
            }
        }
    }

    private void CameraBuzzOnOff()
    {    
        // �ӵ��� 100���� Ŀ����, ȭ�� ��鸮�� �ϴ� �ڵ�
         // �������� ��鸲�� CamShake.cs �� �ִ�.
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


    // TargetOfRader �� �±׷� �� ������Ʈ �� ���� �����̿� �ִ� ������Ʈ�� ã�� �ڵ�
    private GameObject FindNearestObjectByTag(string tag)
    {
        // Ž���� ������Ʈ ����� List �� ����
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ �޼ҵ带 �̿��� ���� ����� ���� ã�´�.
        var neareastObject = objects.OrderBy(obj =>
        {
            return Vector3.Distance(transform.position, obj.transform.position);
        }).FirstOrDefault();

        return neareastObject;

    }

    private void RaderOnOff()
    {
        // ���� ���� �ϴ� ���̴��� �ʿ���
        #region ���̴� �����

        // ������ �Ÿ��� 2���� �۰� isRader �� false �̰�,
        // �����ϰ��ִ� ���̰�,
        // Player �� ���ֺ��� �ִ� ���¶�� ( (������Ʈ���� �÷��̾������ ����)�� ũ�� > (������Ʈ���� �÷��̾������ ������ ũ�� + �÷��̾��� forward ����)�� ũ��)
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
        // isRader �� ������, ���� ����� ������Ʈ���� TargetCanvas �� Ű�� �ϰ�ʹ�.aaaaaaaaaaaaadddd
        if (isRader == true)
        {
            if (LHS_Enemy.Instance.m_State == LHS_Enemy.EnemyState.Weakness)
            {
                //�������� Ű��
                
                Target_Canvas.SetActive(true);

                //������ ������ ����
                Target_Canvas.transform.position = FindNearestObjectByTag(TargetOfRader).transform.position + (transform.position - FindNearestObjectByTag(TargetOfRader).transform.position).normalized * 5;
                //��� �÷��̾ �ٶ󺸰�
                Target_Canvas.transform.forward = transform.position - FindNearestObjectByTag(TargetOfRader).transform.position;

                // 0.2�� �ִٰ� Ready �ǰ� �ϰ�ʹ�.
                curTime_2 += Time.deltaTime;
                if (curTime_2 > raderTime)
                {
                    // �̹����� ���� �������� �ٲٰ�
                    //Target_Canvas.GetComponent<Image>().color = Color.red;
                    // ���� ��
                    
                    isRaderReady = true;
                    //Target_Canvas.SetActive(false);
                }
            }
        }

        // isRader �� true �� �ƴϸ� �׳� ����
        else
        {
            //Target_Canvas.GetComponent<Image>().color = Color.blue;
            curTime_2 = 0;
            isRaderReady = false;
            Target_Canvas.SetActive(false);
        }
    }
    //--------------------------------------------------------------------------------------------------------------------------------------------------
    // �������� �ð��� ����, isRaderReady ���� �����Ǵ°� �Ϸ���!
    // �ٵ� �����̿� �ִ� ���� �ٲ�ٸ� isRaderReady �� �ٽ� �ʱ�ȭ ������� �ϴµ� �� �κ��� ���� �ȵ�!
    //--------------------------------------------------------------------------------------------------------------------------------------------------



    // Ű�� ������ ���ÿ��� �߰�,
    // AttackOn ���� �϶���, �浹 �Ҷ����� ��ġ�⸦ �ϰ�ʹ�.
    // �� AttackOn ���� �϶��� gravity �� ������ ���� �ʰ� �Ѵ�.
    // ��ġ�⸦ �ϰ��� AttackOn �� Ǯ���� �ǰ�, �������� �־� ������Ų��.

    private void Attack()
    {
        // isRaderReady �� �������� �� ���� Ű�� ������
        // ���� ������Ʈ���� ��ġ�� �ϰ�ʹ�.

        // Ÿ�ٱ����� ����
        Vector3 dir = (FindNearestObjectByTag(TargetOfRader).transform.position - transform.position).normalized;

        // RaderReady �� �غ�Ǿ��ִ� ���¿���
        if (isRaderReady == true)
        { 
            // Ű�� ������
            if(Input.GetKeyDown(KeyCode.Space))
            {
                // ������ �� �ִ� ���°� ��
                isAttackOn = true;
            }

        }
        // ���⼭ ���� �� �� �ִ� ���°� �Ǹ�
        if (isAttackOn == true)
        {
            // ���� �������� ��ġ��
            cc.Move(dir * 400 * Time.deltaTime);
        }

    }

    private void WhenAttack_GameSlowdown()
    {
        // ���� �����ϸ�, slowFinishTime ���� ������ 0.3��ŭ ������
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
        // ���� ���� ��ü�� �±װ� TargetOfRader ���
        // isAttackOn �� ����.
        if (hit.gameObject.tag == "TargetOfRader")
        {
            // ���� ���¶�� ����Ʈ ȿ���� / ī�޶� ��鸲 ���� / ���� ������ ���� ���
            if (LHS_Enemy.Instance.m_State == LHS_Enemy.EnemyState.Weakness)
            {
                Test.Play();
                Test2.Play();
                WeaknessEffect.SetActive(true);
                WeaknessEffect_particle.Stop();
                WeaknessEffect_particle.Play();
                WeaknessEffect.transform.position = transform.position + new Vector3(0, 0, 0.5f);

                //ī�޶� ����
                LHS_CamRotate.Instance.OnShakeCamera(0.2f, 0.3f);

                // �ð� �������� �ϴ°� ������ ��
                isAttackSucces = true;

                // ���� �ѹ� �ְ�
                yVelocity = jumpPower;


                // isAttackOn �� ����
                isAttackOn = false;
                //Destroy(hit.gameObject);

                // ���࿡ weakness �����϶� attack�� ������ �ʾҴٸ�
                // ���࿡ enemy�� WEAKNESS �󤺤��� �̾ȹ̾�


                // �÷��̾ ���� ����� ��
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
