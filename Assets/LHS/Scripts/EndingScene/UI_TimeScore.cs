using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class UI_TimeScore : MonoBehaviour
{
    TextMeshProUGUI finishTime;

    string finishTimeString;

    // Start is called before the first frame update
    void Start()
    {
        finishTime = GetComponent<TextMeshProUGUI>();

        // 저장한 값 받아오기
        finishTimeString = PlayerPrefs.GetString("FinishTime");

        // UI에 나오게 하기
        finishTime.text = finishTimeString;

        // 초기화.
        PlayerPrefs.DeleteAll();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
