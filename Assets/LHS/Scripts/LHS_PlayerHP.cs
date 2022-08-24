using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �÷��̾��� ü���� �����ϰ� �ʹ�.
public class LHS_PlayerHP : MonoBehaviour
{
    int hp = 100;
    int maxHP = 100;
    public Slider hpSlider;

    public static LHS_PlayerHP Instance;

    // HP => property �� �ٲٰ� �ʹ�.
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
                print("�÷��̾� ����");
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
