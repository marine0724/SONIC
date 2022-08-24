using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Grade : MonoBehaviour
{
    TextMeshProUGUI grade;


    int finishTIme_Sec;
    int finishTIme_Min;
 

    // Start is called before the first frame update
    void Start()
    {
        // ������Ʈ ������
        grade = GetComponent<TextMeshProUGUI>();

        //���� �ð� ��������
        finishTIme_Sec = PlayerPrefs.GetInt("Finish_Sec");

        //���� �� ��������
        finishTIme_Min = PlayerPrefs.GetInt("Finish_Min");

        // 1���� ������ ����� B
        if (finishTIme_Min > 1)
        {
            grade.text = "B";
        }

        // 0�� �϶��� �� �θ�
        else if (finishTIme_Min == 0)
        {
            // 30�ʰ� ������ ����� A
            if (finishTIme_Sec > 30)
                grade.text = "A";

            // 30�� �����̸� ����� S
            else if (finishTIme_Sec <= 30)
                grade.text = "S";

        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
