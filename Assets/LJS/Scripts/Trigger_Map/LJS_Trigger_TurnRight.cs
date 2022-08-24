using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LJS_Trigger_TurnRight : MonoBehaviour
{


    // 트리거로 닿으면, 캐릭터의 앞으로가는 Vector 의 값을 줄이고
    // 옆 방향 벡터의 값을 올린다.
    // 카메라의 방향은 캐릭터를 바라보게 해야함.

    // 필요속성 : Player 의 벡터


    bool isTurn;

    private void OnTriggerEnter(Collider other)
    {
        // hAxis 값만 늘린다.

        // 또한 카메라의 방향도 생각해봐야 함.


        // Trigger 된 gameobject 가 플레이어라면
        if (other.gameObject == LJS_Player.Instance.gameObject)
        {
            print("트리거 작용");

            // 카메라를 그냥 돌려버리면 될거같은데?!
            // 카메라가 바라보는 방향이  Player 의 방향이니까!
            isTurn = true;


            // 카메라를 돌리고 나서, 플레이어의  hAxis 를 다 죽여버리자 
            LJS_Player.Instance.dir.x = 0;
            LJS_Player.Instance.subDir = new Vector3(0,0,0);

            // 그다음 속도만 올려주면 끝인데?!
            LJS_Player.Instance.speed += 30;


        }


    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTurn == true)
        {
            LJS_CamRotate.Instance.x = Mathf.Lerp(LJS_CamRotate.Instance.x, 100, 15 * Time.deltaTime) ;

            if (LJS_CamRotate.Instance.x > 95)
            {
                isTurn = false;
                print("작용");
            }
        }
    }
}
