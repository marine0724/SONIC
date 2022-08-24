using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UI_Timer : MonoBehaviour
{
    TextMeshProUGUI timer;

    string timerCheck;
    bool isTimerOn;
    float time;
    string[] ClockText = new string[3];

    // Start is called before the first frame update
    void Start()
    {
        timer = GetComponent<TextMeshProUGUI>();

        isTimerOn = true;
    }

    float msTime;

    bool isBossDie;

    // Update is called once per frame
    void Update()
    {
        if (isTimerOn)
        {

            // Enemy 상태가 Die 상태가 아니라면
            // 시간이 흐르게 하자.
            if (LHS_Enemy.Instance.m_State != LHS_Enemy.EnemyState.Die)
            {
                time += Time.deltaTime;
                msTime += 100 * Time.deltaTime;

                // 보스 아직 안죽음
                isBossDie = false;
            }

            // 보스가 아직 안죽었고,
            // Enemy 가 Die 가 되면 시간을 저장 해보자
            if (isBossDie == false)
            {
                if (LHS_Enemy.Instance.m_State == LHS_Enemy.EnemyState.Die)
                {
                    // 보스 죽음
                    isBossDie = true;

                    if (isBossDie == true)
                    {
                        //이전에 저장된 키 다 삭제
                        PlayerPrefs.DeleteAll();

                        // FinishTime 으로 시간 저장
                        PlayerPrefs.SetString("FinishTime", string.Format("{0}\"{1}\"{2}\"", ClockText[0], ClockText[1], ClockText[2]));
                        PlayerPrefs.SetInt("Finish_Sec", int.Parse(ClockText[1]));
                        PlayerPrefs.SetInt("Finish_Min", int.Parse(ClockText[0]));
                    }
                }
            }
            

            //분
            ClockText[0] = ((int)time / 60 % 60).ToString();
            //초
            ClockText[1] = ((int)time % 60).ToString();
            //ms
            ClockText[2] = ((int)msTime % 100).ToString();

            timer.text = string.Format("{0}:{1}:{2}", ClockText[0], ClockText[1], ClockText[2]);

        }


    }
}
