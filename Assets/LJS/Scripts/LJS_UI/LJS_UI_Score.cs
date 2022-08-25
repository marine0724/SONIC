using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LJS_UI_Score : MonoBehaviour
{
    TextMeshProUGUI score;

    string timerCheck;
    bool isTimerOn;
    float time;
    string[] ClockText = new string[3];

    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<TextMeshProUGUI>();

        isTimerOn = true;
    }

    float msTime;

    // Update is called once per frame
    void Update()
    {
        if (isTimerOn)
        {
            time += Time.deltaTime;
            msTime += 100*Time.deltaTime;

            //Ка
            ClockText[0] = ((int)time / 60 % 60).ToString();
            //УЪ
            ClockText[1] = ((int)time % 60).ToString();
            //ms
            ClockText[2] = ((int)msTime % 100).ToString();

            score.text = string.Format("{0}:{1}:{2}", ClockText[0], ClockText[1], ClockText[2]);

        }


    }
}
