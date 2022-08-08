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
    [Header("Ÿ�� ĵ����")]
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

    // ���̴��� �ɸ��� canvas �߰� �ؾ���
    private void RaderOn()
    {
        // ���̴��� ������
        if (Player.Instance.isRader == true)
        {
            //canvas On
            Target_Canvas.SetActive(true);
            //������ ������ ����
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
            print("���� ����!");
        }
    }
}
