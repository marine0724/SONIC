using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LJS_UI_Score : MonoBehaviour
{

    // 싱글톤으로 구현
    public static LJS_UI_Score Instance;
    public void Awake()
    {

        Instance = this;
    }

    // 링을 먹으면 점수가 100점씩 올라가게 하고싶다.
    // 나중에 Enemy 를 잡아도 점수 100점씩 올라가게 할 거임
    // 필요속성 : 점수

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

            // 점수 변화할 때마다 작성
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
