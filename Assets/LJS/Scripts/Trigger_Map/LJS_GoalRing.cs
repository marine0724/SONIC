using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LJS_GoalRing : MonoBehaviour
{
    // 계속 돌아감
    // 플레이어가 먹으면 사라지면서 소리남
    
    float rotSpeed = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //계속 돌아가게
        transform.eulerAngles += new Vector3(0, rotSpeed * Time.deltaTime, 0 );

    }

}
