using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_CamFollow : MonoBehaviour
{
    //필요속성 : CamPos, 이동속도
    //우리가 따라다녀야 할 녀석
    public Transform campos; 
    public Transform campos2;
    public float speed1 = 5;
    public float speed2 = 1;

    LHS_CamRotate camRotate;
    GameObject player;
    GameObject enemy;

    bool isCampos1 = false;
    bool isCampos2 = false;

    private void Start()
    {
        camRotate = GetComponent<LHS_CamRotate>();

        player = GameObject.Find("Player");
        enemy = GameObject.Find("MainEnemy");
    }
    //<target-me>
    //Lerp를 이용한 카메라 이동
    void Update()
    {
        if(isCampos1 == false)
        {
            // CamPos 를 따라서 이동하고 싶다.
            transform.position = Vector3.Lerp(transform.position, campos.position, speed1 * Time.deltaTime);
            // 회전 적용
            transform.forward = Vector3.Lerp(transform.forward, campos.forward, speed1 * Time.deltaTime);
            //isCampos = true;

            // Campos에 도착했다면 두번째 Lerp 실행될 수 있도록 
            if (Vector3.Distance(campos.position, transform.position) <= 1f)
            {
                isCampos2 = true;
                isCampos1 = true;
            }
        }

        else if(isCampos1 == true && isCampos2 == true)
        {
            print("들어감");
            // CamPos 를 따라서 이동하고 싶다.
            transform.position = Vector3.Lerp(transform.position, campos2.position, speed2 * Time.deltaTime);
            // 회전 적용
            transform.forward = Vector3.Lerp(transform.forward, campos2.forward, speed2 * Time.deltaTime);
            
            // Campos에 도착했다면 camRotate 스크립트를 켜준다
            // 플레이어 / Enemy 스크립트도 실행 될 수 있게 한다
            if (Vector3.Distance(campos2.position, transform.position) <= 0.2f)
            {
                print("같은위치도착 = 게임실행");
                camRotate.enabled = true;
                player.GetComponent<Player>().enabled = true;
                enemy.GetComponent<LHS_Enemy>().enabled = true;
            }
        }
    }
}
