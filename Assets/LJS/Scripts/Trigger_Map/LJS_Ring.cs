using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LJS_Ring : MonoBehaviour
{
    // ��� ���ư�
    // �÷��̾ ������ ������鼭 �Ҹ���


    AudioSource audioSource;

    float rotSpeed = 100;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find("SoundMgr_Ring").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //��� ���ư���
        transform.eulerAngles += new Vector3(0, 0, rotSpeed * Time.deltaTime);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == LJS_Player.Instance.gameObject)
        {
            print("Ʈ���� ����");

            LJS_UI_Score.Instance.Score += 10;

            audioSource.Stop();
            audioSource.Play();
            //audioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/RingSound_Trim_"));

            Destroy(gameObject);

        }
    }
}
