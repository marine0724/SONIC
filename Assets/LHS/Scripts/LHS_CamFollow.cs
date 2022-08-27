using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_CamFollow : MonoBehaviour
{
    //�ʿ�Ӽ� : CamPos, �̵��ӵ�
    //�츮�� ����ٳ�� �� �༮
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
    //Lerp�� �̿��� ī�޶� �̵�
    void Update()
    {
        if(isCampos1 == false)
        {
            // CamPos �� ���� �̵��ϰ� �ʹ�.
            transform.position = Vector3.Lerp(transform.position, campos.position, speed1 * Time.deltaTime);
            // ȸ�� ����
            transform.forward = Vector3.Lerp(transform.forward, campos.forward, speed1 * Time.deltaTime);
            //isCampos = true;

            // Campos�� �����ߴٸ� �ι�° Lerp ����� �� �ֵ��� 
            if (Vector3.Distance(campos.position, transform.position) <= 1f)
            {
                isCampos2 = true;
                isCampos1 = true;
            }
        }

        else if(isCampos1 == true && isCampos2 == true)
        {
            print("��");
            // CamPos �� ���� �̵��ϰ� �ʹ�.
            transform.position = Vector3.Lerp(transform.position, campos2.position, speed2 * Time.deltaTime);
            // ȸ�� ����
            transform.forward = Vector3.Lerp(transform.forward, campos2.forward, speed2 * Time.deltaTime);
            
            // Campos�� �����ߴٸ� camRotate ��ũ��Ʈ�� ���ش�
            // �÷��̾� / Enemy ��ũ��Ʈ�� ���� �� �� �ְ� �Ѵ�
            if (Vector3.Distance(campos2.position, transform.position) <= 0.2f)
            {
                print("������ġ���� = ���ӽ���");
                camRotate.enabled = true;
                player.GetComponent<Player>().enabled = true;
                enemy.GetComponent<LHS_Enemy>().enabled = true;
            }
        }
    }
}
