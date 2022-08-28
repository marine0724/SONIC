using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LJS_Trigger_SceneChanger : MonoBehaviour
{
    public static LJS_Trigger_SceneChanger Instance;

    private void Awake()
    {
        Instance = this;
    }

    float currentTime = 0;
    float createTime = 0.3f;


    public Image fadeOut_Image;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == LJS_Player.Instance.gameObject)
        {
            StartCoroutine(FadeOut());
        }
    }


    IEnumerator FadeOut()
    {
        Time.timeScale = 0.2f;

        // 2�ʵ��� Image ���İ� �� ���̱�
        while (true)
        {
            currentTime += Time.deltaTime;

            fadeOut_Image.color += new Color(0, 0, 0, 5 * Time.deltaTime);

            if (currentTime > createTime)
            {
                print("���ٲ�");
                currentTime = 0;
                Time.timeScale = 1f;

                SceneManager.LoadScene("LHS_Scene");
                break;
            }

            yield return null;
        }
    }
}
