using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LJS_UI_Score : MonoBehaviour
{

    // �̱������� ����
    public static LJS_UI_Score Instance;
    public void Awake()
    {

        Instance = this;
    }

    // ���� ������ ������ 100���� �ö󰡰� �ϰ�ʹ�.
    // ���߿� Enemy �� ��Ƶ� ���� 100���� �ö󰡰� �� ����
    // �ʿ�Ӽ� : ����

    TextMeshProUGUI score_UI;

    int _score = 0;

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            score_UI.text = "Score : " + _score;

            // ���� ��ȭ�� ������ �ۼ�
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        score_UI = GetComponent<TextMeshProUGUI>();

    }


    // Update is called once per frame
    void Update()
    {


    }


}
