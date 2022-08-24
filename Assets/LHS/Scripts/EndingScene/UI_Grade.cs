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
        // 컴포넌트 들고오기
        grade = GetComponent<TextMeshProUGUI>();

        //끝난 시간 가져오기
        finishTIme_Sec = PlayerPrefs.GetInt("Finish_Sec");

        //끝난 분 가져오기
        finishTIme_Min = PlayerPrefs.GetInt("Finish_Min");

        // 1분이 넘으면 등급은 B
        if (finishTIme_Min > 1)
        {
            grade.text = "B";
        }

        // 0분 일때는 초 로만
        else if (finishTIme_Min == 0)
        {
            // 30초가 넘으면 등급은 A
            if (finishTIme_Sec > 30)
                grade.text = "A";

            // 30초 이하이면 등급은 S
            else if (finishTIme_Sec <= 30)
                grade.text = "S";

        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
