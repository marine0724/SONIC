using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LHS_EnemyHP : MonoBehaviour
{
    //#region 체력 속성
    //// 필요속성 : HP , 최대 HP , HP Silder변수
    //[Header("체력")]
    //public int hp = 100;
    //int maxHP = 100;
    //public Slider hpSlider;
    //#endregion

    // 필요속성 : 체력
    int hp = 100;
    int maxHP = 100;
    public Slider hpSlider;

    public static LHS_EnemyHP Instance;
    
    // HP => property 로 바꾸고 싶다.
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            print("체력1 깎임");

            if (hp <= 0)
            {
                print("보스 죽음");
                LHS_Enemy.Instance.m_State = LHS_Enemy.EnemyState.Die;
            }
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {

    }

    public void Update()
    {
        hpSlider.value = (float)hp / (float)maxHP;
    }


}
