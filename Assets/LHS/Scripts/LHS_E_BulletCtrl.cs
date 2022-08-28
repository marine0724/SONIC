using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총알발사
public class LHS_E_BulletCtrl : MonoBehaviour
{
    [Header("총알 속도")]
    public float rgSpeed = 100;

    Rigidbody rigid;


    // 총알 효과
    // public ParticleSystem bulletPS;
    //public GameObject bulletGO;
    //GameObject bulletGameObject;

    // Start is called before the first frame update
    void Start()
    {
        
        rigid = GetComponent<Rigidbody>();

        //GetComponent<Rigidbody>().AddForce(Vector3.forward * rgSpeed, ForceMode.Impulse); -> 월드좌표가 기준이 되기때문에 방향이 맞지않는다.

        //bulletPS.Stop();
        //bulletPS.Play();
        //GameObject bulletGameObject = Instantiate(bulletGO);
        //bulletGameObject.transform.position = transform.position;
        //bulletGameObject.transform.forward = transform.forward;

        // 총알을 포물선을 그리며 가고싶다 (앞으로의 힘 / 위로의 힘)
        rigid.AddRelativeForce(Vector3.forward * rgSpeed, ForceMode.Impulse);
        rigid.AddRelativeForce(Vector3.up * 5, ForceMode.Impulse);

        // 카메라 흔들림 효과
        LHS_CamRotate.Instance.OnShakeCamera(0.5f, 0.9f);

    }

    // Update is called once per frame
    void Update()
    {
    }

    // 충돌한다면 나 삭제AAAAA
    // 충돌처리에서 어떻게 할건지 생각해보기
    //private void OnCollisionEnter(Collision collision)
    //{
    //    // 닿인 대상이 플레이어이고
    //    if (collision.gameObject.name.Contains("Player"))
    //    {
    //        print("총알-1");
    //        // 플레이어의 피를 깎는다.
    //        LHS_PlayerHP.Instance.HP -= 10;
    //    }
    //}


    private void OnCollisionEnter(Collision collision)
    {
        // 닿인 대상이 플레이어이고
        if (collision.gameObject.name.Contains("Player"))
        {

            print("총알-1");
            // 플레이어의 피를 깎는다.
            LHS_PlayerHP.Instance.HP -= 5;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
