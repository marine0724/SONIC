using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ѿ˹߻�
public class LHS_E_BulletCtrl : MonoBehaviour
{
    [Header("�Ѿ� �ӵ�")]
    public float rgSpeed = 100;

    Rigidbody rigid;


    // �Ѿ� ȿ��
    // public ParticleSystem bulletPS;
    //public GameObject bulletGO;
    //GameObject bulletGameObject;

    // Start is called before the first frame update
    void Start()
    {
        
        rigid = GetComponent<Rigidbody>();

        //GetComponent<Rigidbody>().AddForce(Vector3.forward * rgSpeed, ForceMode.Impulse); -> ������ǥ�� ������ �Ǳ⶧���� ������ �����ʴ´�.

        //bulletPS.Stop();
        //bulletPS.Play();
        //GameObject bulletGameObject = Instantiate(bulletGO);
        //bulletGameObject.transform.position = transform.position;
        //bulletGameObject.transform.forward = transform.forward;

        // �Ѿ��� �������� �׸��� ����ʹ� (�������� �� / ������ ��)
        rigid.AddRelativeForce(Vector3.forward * rgSpeed, ForceMode.Impulse);
        rigid.AddRelativeForce(Vector3.up * 5, ForceMode.Impulse);

        // ī�޶� ��鸲 ȿ��
        LHS_CamRotate.Instance.OnShakeCamera(0.5f, 0.9f);

    }

    // Update is called once per frame
    void Update()
    {
    }

    // �浹�Ѵٸ� �� ����AAAAA
    // �浹ó������ ��� �Ұ��� �����غ���
    //private void OnCollisionEnter(Collision collision)
    //{
    //    // ���� ����� �÷��̾��̰�
    //    if (collision.gameObject.name.Contains("Player"))
    //    {
    //        print("�Ѿ�-1");
    //        // �÷��̾��� �Ǹ� ��´�.
    //        LHS_PlayerHP.Instance.HP -= 10;
    //    }
    //}


    private void OnCollisionEnter(Collision collision)
    {
        // ���� ����� �÷��̾��̰�
        if (collision.gameObject.name.Contains("Player"))
        {

            print("�Ѿ�-1");
            // �÷��̾��� �Ǹ� ��´�.
            LHS_PlayerHP.Instance.HP -= 5;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
