using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_CamFollow : MonoBehaviour
{
    //필요속성 : CamPos, 이동속도
    public Transform campos; // 우리가 따라다녀야 할 녀석
    public float speed = 5;

    LHS_CamRotate camRotate;
    GameObject player;
    GameObject enemy;

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

        // CamPos 를 따라서 이동하고 싶다.
        transform.position = Vector3.Lerp(transform.position, campos.position, speed * Time.deltaTime);
        // 회전 적용
        transform.forward = Vector3.Lerp(transform.forward, campos.forward, speed * Time.deltaTime);

        // Campos에 도착했다면 도착했다면 camRotate 스크립트를 켜준다
        // 플레이어 / Enemy 스크립트도 실행 될 수 있게 한다
        if(Vector3.Distance(campos.position, transform.position) <= 0.2f)
        {
            print("같은위치도착 = 게임실행");
            camRotate.enabled = true;
            player.GetComponent<Player>().enabled = true;
            enemy.GetComponent<LHS_Enemy>().enabled = true;
        }
    }
}
