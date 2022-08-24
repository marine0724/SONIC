using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 플레이어의 체력을 관리하고 싶다.
public class LHS_PlayerHP : MonoBehaviour
{
    int hp = 100;
    int maxHP = 100;
    public Slider hpSlider;

    public static LHS_PlayerHP Instance;

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

            if (hp <= 0)
            {
                print("플레이어 죽음");
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
