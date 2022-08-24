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

        // ������ �� �޾ƿ���
        finishTimeString = PlayerPrefs.GetString("FinishTime");

        // UI�� ������ �ϱ�
        finishTime.text = finishTimeString;

        // �ʱ�ȭ.
        PlayerPrefs.DeleteAll();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
