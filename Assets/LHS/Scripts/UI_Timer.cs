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

            // Enemy ���°� Die ���°� �ƴ϶��
            // �ð��� �帣�� ����.
            if (LHS_Enemy.Instance.m_State != LHS_Enemy.EnemyState.Die)
            {
                time += Time.deltaTime;
                msTime += 100 * Time.deltaTime;

                // ���� ���� ������
                isBossDie = false;
            }

            // ������ ���� ���׾���,
            // Enemy �� Die �� �Ǹ� �ð��� ���� �غ���
            if (isBossDie == false)
            {
                if (LHS_Enemy.Instance.m_State == LHS_Enemy.EnemyState.Die)
                {
                    // ���� ����
                    isBossDie = true;

                    if (isBossDie == true)
                    {
                        //������ ����� Ű �� ����
                        PlayerPrefs.DeleteAll();

                        // FinishTime ���� �ð� ����
                        PlayerPrefs.SetString("FinishTime", string.Format("{0}\"{1}\"{2}\"", ClockText[0], ClockText[1], ClockText[2]));
                        PlayerPrefs.SetInt("Finish_Sec", int.Parse(ClockText[1]));
                        PlayerPrefs.SetInt("Finish_Min", int.Parse(ClockText[0]));
                    }
                }
            }
            

            //��
            ClockText[0] = ((int)time / 60 % 60).ToString();
            //��
            ClockText[1] = ((int)time % 60).ToString();
            //ms
            ClockText[2] = ((int)msTime % 100).ToString();

            timer.text = string.Format("{0}:{1}:{2}", ClockText[0], ClockText[1], ClockText[2]);

        }


    }
}
