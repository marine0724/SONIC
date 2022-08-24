using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_Gun : MonoBehaviour
{
    [Header("ȸ�� �ӵ�")]
    public float rotateSpeed = 5;

    Vector3 dir;

    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //(������ ����) Ÿ�ٰ� ���� ����
        dir = player.position - transform.position;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);
    }
}
