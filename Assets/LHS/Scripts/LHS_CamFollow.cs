using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_CamFollow : MonoBehaviour
{
    //�ʿ�Ӽ� : CamPos, �̵��ӵ�
    public Transform campos; // �츮�� ����ٳ�� �� �༮
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
    //Lerp�� �̿��� ī�޶� �̵�
    void Update()
    {

        // CamPos �� ���� �̵��ϰ� �ʹ�.
        transform.position = Vector3.Lerp(transform.position, campos.position, speed * Time.deltaTime);
        // ȸ�� ����
        transform.forward = Vector3.Lerp(transform.forward, campos.forward, speed * Time.deltaTime);

        // Campos�� �����ߴٸ� �����ߴٸ� camRotate ��ũ��Ʈ�� ���ش�
        // �÷��̾� / Enemy ��ũ��Ʈ�� ���� �� �� �ְ� �Ѵ�
        if(Vector3.Distance(campos.position, transform.position) <= 0.2f)
        {
            print("������ġ���� = ���ӽ���");
            camRotate.enabled = true;
            player.GetComponent<Player>().enabled = true;
            enemy.GetComponent<LHS_Enemy>().enabled = true;
        }
    }
}
