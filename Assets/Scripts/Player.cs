using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
{
    //�̱��� ����
    //------------------------------//
    public static Player Instance;

    private void Awake()
    {
        Instance = this;
    }
    //------------------------------//


    [Header("--------------------------")]
    [Header("�ӵ�         ")]
    //public float speed = 20f;
    [Header("--------------------------")]


    [Header("���ӵ�")]
    public float accel = 0;  
    public float maxAccel = 200f;
    public float avgAccel = 60f;
    public float minAccel = 0f;


    [Header("���ι�")]
    public float zeroTohundred = 5f;
    [Header("--------------------------")]


    [Header("�÷��̾� ������")]
    public float jumpPower = 10f;
    public float yVelocity;

    // �������� Ȯ��
    bool isJump = false;

    // Player �� 2�������� ���� ����
    int jumpCount = 0;
    int jumpCountMax = 2;

    // �߷°�
    public float gravity = -20f;


    float hAxis;
    float vAxis;

    //---------------------------------------------------------------------------------------------//

    //�÷��̾� ����
    Vector3 dir;

    //ĳ������Ʈ�ѷ� ����
    CharacterController cc;
    //---------------------------------------------------------------------------------------------//

    // �뽬 ���� �ܾƿ� ī�޶�
    float curTime = 0f;
    float dashTime = 1f;

    public bool isDash;


    PostProcessProfile myProfile;
    LensDistortion myLD;


    void Start()
    {
        //ĳ������Ʈ�ѷ� ������Ʈ ��������
        cc = GetComponent<CharacterController>();

        // �ڽ����� PostProcessVolume ������Ʈ�� �ִ�
        // �������� �������� (���� postprocess)
        myProfile = GetComponentInChildren<PostProcessVolume>().profile;

        // �������Ͽ� ���� ����� ��������
        myProfile.TryGetSettings<LensDistortion>(out myLD);


        //myLD.enabled.value = true;
    }


    void Update()
    {
        Jump();
        Dash();
        //pp_LensDistortion(); ���� ���θ���
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


    // ������ �ӵ��� ���� ����� �ɵ�
    // �ٵ� �ӵ��� ũ�Ⱑ 60������ ���� value = 0
    // �ӵ��� ũ�Ⱑ 60���� Ŀ���� Ŀ�� ����ŭ value�� -���� ����
    // �ٵ� value �� -70 ������

    private void pp_LensDistortion()
    {
        float abs_accel = Mathf.Abs(accel);
        //���� ����

        // �ٵ� �ӵ��� ũ�Ⱑ 60������ ���� value = 0
        if (abs_accel <= 60)
        {
            abs_accel = 0;
        }
        // �ӵ��� ũ�Ⱑ 60���� Ŀ����
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

        // ���� ���� �ϴ� ���̴��� �ʿ���
        #region ���̴� �����


        // �Ÿ� 2
        float attackDistance = 2f;

        print("isRader :" + isRader);
        print("isJumpt :" + isJump);

        //������ �Ÿ��� 2���� �۰� isRader �� false �̰�, isJump �� true �� ��쿡
        if (Vector3.Distance(transform.position, Enemy.Instance.transform.position) < 30 &&  isJump == true)
        {
            // ���� ���� ������ -30 ~ 30 �� ���̿� �ִٸ�
            // ���ͳ����� ������ ��� ��
           // if(Vector3.Angle(Enemy.Instance.transform.position - transform.position,   )
            // isRader �� true �� ������
            isRader = true;
        }
        else
            isRader = false;

        #endregion
        // ���̴� �ȿ��� ���� ����� �ѳ����׸� Target_Canvas �� ��



        // Enemy �� ���̴��� �ɸ��� bool �Լ��� true �� �ߵ��ϰ�
        // �ϳ��� �ߵ��Ǹ�, 
        // �ߵ��ϸ� �÷��̾ ���� �� �� �ִ� ���°� ��

    }


    Rigidbody rigid;

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {

            Vector3 dir = Enemy.Instance.transform.position - transform.position;
            dir.Normalize();

            // ���̴��� true �̸�
            if (isRader == true)
            {
                cc.Move(dir * accel);
            }
        }        
    }

    private void Move() //Player �����̱�
    {     
        
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        //------------------------------------------------------------------------------------------------ //Character Controller �� ���� ��������� �߷°� 0�̰� �����.
        //y�ӵ����� 0���� ũ�ų� ���� ���� �ʾ��� ��쿡�� yVelocity �� �߷°� ����
        if (yVelocity > 0 || !cc.isGrounded)
        {
            isJump = true;
            yVelocity += gravity * Time.deltaTime;
        }

        //y�ӵ����� ���� ��� �ӵ��� 0 �ΰ�쿡�� yVelocity = 0
        else if (yVelocity <= 0 && cc.isGrounded)
        {
            isJump = false;
            yVelocity = 0f;
            jumpCount = 0;
        }
        //------------------------------------------------------------------------------------------------



        // ���� ����
        dir = new Vector3(hAxis, 0, vAxis).normalized;

         
        # region    ī�޶� ���� ������ �չ������� �ϴ� �ڵ�
        // ī�޶��� y ȸ���� ���ؿ´�.
        Quaternion v3Rotation = Quaternion.Euler(0f, CamRotate.Instance.gameObject.transform.eulerAngles.y, 0f);

            //�̵��� ���͸� ������.
            dir = v3Rotation * dir;
        #endregion

        
        #region ����Ű�� ������ ��, �� ������ �ٶ󺸰� �ϴ� �ڵ�
        // ������ ��
        if (!(hAxis == 0 && vAxis == 0))
            {
                // ���� ���� �ִ� �� ���� �����̴� �������� �ٶ󺸴� ���� �� ���� Lerp �� ȸ��
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 20.0f);
            }

        #endregion



        //------------------------------------------------------------------------------------------------ ���� �� �ٽ� ������

        
        #region ���� �ִ� �ּڰ� ����
        // �չ���Ű�� ��� ������ ������
        if (vAxis > 0)
            {
                // ���� avgAccel ���� ������
                if (accel < avgAccel)
                {
                    // ���� accel ���� ������, �ʴ� 200 ����
                    if (accel < 0)
                    {
                        accel += 200 * Time.deltaTime;
                    }

                    // �װ� �ƴ϶�� ���ι鸸ŭ ���� ��
                    accel += (100 / zeroTohundred) * Time.deltaTime;
                }
                // ���Ӱ��� �Ϲ� ���Ӱ����� ũ�ٸ� �ʴ� 10��ŭ ����
                else if (accel > avgAccel)
                {
                    accel -= 10f * Time.deltaTime;
                }
                // ���Ӱ� �ִ�� �����
                //���� ���Ӱ��� �ִ밡�Ӱ� �̻��̶��
                else if (accel >= maxAccel)
                {
                    // ������ �ִ밡�Ӱ� ����
                    accel = maxAccel;
                }
            }


            // �չ���Ű�� ��� ������ ������
            else if (hAxis > 0)
            {
                // �׳� �չ���Ű ������ ������ 40��ŭ�� ���ӵ�

                // ���� avgAccel ���� ������
                if (accel < avgAccel)
                {
                    // ���� accel ���� ������, �ʴ� 200 ����
                    if (accel < 0)
                    {
                        accel += 200 * Time.deltaTime;
                    }

                    // �װ� �ƴ϶�� ���ι鸸ŭ ���� ��
                    accel += (100 / zeroTohundred) * Time.deltaTime;
                }


                // ���Ӱ��� �Ϲ� ���Ӱ����� ũ�ٸ� �ʴ� 10��ŭ ����
                else if (accel > avgAccel)
                {
                    accel -= 10f * Time.deltaTime;
                }

                // ���Ӱ� �ִ�� �����
                //���� ���Ӱ��� �ִ밡�Ӱ� �̻��̶��
                else if (accel >= maxAccel)
                {
                    // ������ �ִ밡�Ӱ� ����
                    accel = maxAccel;
                }

            }

            // �� ����Ű�� �����ٸ�,
            // ���� ��û���� ��Ű�ٰ�
            // ���� 
            else if (vAxis < 0)
            {              
                // ���ӵ� ���� -avgAccel ���� ũ�ٸ�
                if (accel > -avgAccel)
                {
                    //�ٵ� ���� �װ� ������
                    if (accel > 0)
                    {
                        //�ʴ� 200��ŭ ����
                        accel -= 200 * Time.deltaTime;
                    }

                    // �����ε� -avgAccel ���� �ȵǸ� ���ι鸸ŭ ���� ��
                    accel -= (100 / zeroTohundred) * Time.deltaTime;
                }

                // ���Ӱ� �ִ�� �����
                //���� ���Ӱ��� �ִ밡�Ӱ� �̻��̶��
                else if (accel <= -maxAccel)
                {
                    // ������ �ִ밡�Ӱ� ����
                    accel = -maxAccel;
                }
            }

            else if (hAxis < 0)
            {
                // ���ӵ� ���� -avgAccel ���� ũ�ٸ�
                if (accel > -avgAccel)
                {
                    //�ٵ� ���� �װ� ������
                    if (accel > 0)
                    {
                        //�ʴ� 200��ŭ ����
                        accel -= 200 * Time.deltaTime;
                    }

                    // �����ε� -avgAccel ���� �ȵǸ� ���ι鸸ŭ ���� ��
                    accel -= (100 / zeroTohundred) * Time.deltaTime;
                }

                // ���Ӱ� �ִ�� �����
                //���� ���Ӱ��� �ִ밡�Ӱ� �̻��̶��
                else if (accel <= -maxAccel)
                {
                    // ������ �ִ밡�Ӱ� ����
                    accel = -maxAccel;
                }
            }


            // �� ����Ű�� ������ ���� �ʴٸ�
            else if (vAxis == 0)
            {              
                accel -= 200 * Time.deltaTime;


                //���� ���Ӱ��� �ּҰ��Ӱ� ���϶��
                if (accel <= minAccel)
                {
                    // ������ �ּҰ��Ӱ� ����
                    accel = minAccel;
                }
            }
        #endregion
        //------------------------------------------------------------------------------------------------


        //------------------------------------------------------------------------------------------------ characterController �� ���� ����
        //Speed => �ӷ� + ����. �� �����ӷ�
        float Speed = accel;

        // ������ y ���� yVelocity
        // Speed �� �������� ������ ���ӵ����� �ʹ� Ŀ��, �����ϸ� �ϴó��� ���ư�
        // �׸��� 5�� �������� ������, RigidBody ���� �����Ѱ�ó�� �� �ߴ� ������

        // [�̵�]     

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
        //�ӵ��� - �ε�, ���� ���°��
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


    //����

    // ��, �� Ű�� �������� ������ => ������

    // �� Ű�� ������ �� ��_�� ������ => ������

    // �� Ű�� ������ �� ������ �������� => ������
    //============================================================

    // �� Ű�� �������ִٰ� ���� �ӵ� �������� ���� ���� Ű ������ => �ݴ�ΰ��ٰ� �������� ��


    // ��, �� Ű�� ������ �� ������ ������ => �ٵ� �뽬�� �ȵ�

    // �� Ű�� ������ �� �������� ������ => �ݴ�� ��

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
        // LeftShift ������ ���ǵ� 50 ����

        // leftShift ������ 
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // �뽬 �����ִ� ����.
            isDash = true;

            // ī�޶� ��鸲
            // 0.2 �� ����� 1�ʸ�ŭ
            CamRotate.Instance.OnShakeCamera(0.4f, 0.5f);
            //CamRotate.Instance.OnShakeCamera_Rot(0.1f, 2f);

            if (vAxis > 0)
            {
                //���ǵ� 50 ����
                accel += 50;
            }
            else if (vAxis < 0)
            {
                accel -= 50;
            }
        }

        //�뽬�� true �̸�
        if (isDash == true)
        {
            // �ð��� �帣��
            curTime += Time.deltaTime;

            // 1�ʰ� �����ų�, �ӵ��� 100���� �۴ٸ�
            if (curTime > dashTime && accel < 150)
            {
                // isDash �� false ��
                isDash = false;
                curTime = 0f;
            }
        }
    }
}
