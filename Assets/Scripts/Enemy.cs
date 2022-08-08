using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance;

    private void Awake()
    {
        Instance = this;
    }
    [Header("타겟 캔버스")]
    public GameObject Target_Canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaderOn();
    }

    // 레이더에 걸리면 canvas 뜨게 해야함
    private void RaderOn()
    {
        // 레이더가 켜지면
        if (Player.Instance.isRader == true)
        {
            //canvas On
            Target_Canvas.SetActive(true);
            //나한테 가져다 놓기
            Target_Canvas.transform.position = transform.position + (Player.Instance.transform.position - transform.position).normalized * 2 ;
            Target_Canvas.transform.forward = Player.Instance.transform.position - transform.position;
        }

        else
        {
            Target_Canvas.SetActive(false);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject == Player.Instance.gameObject)
        {
            print("공격 맞음!");
        }
    }
}
