using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LHS_EnemyHP : MonoBehaviour
{
    //#region ü�� �Ӽ�
    //// �ʿ�Ӽ� : HP , �ִ� HP , HP Silder����
    //[Header("ü��")]
    //public int hp = 100;
    //int maxHP = 100;
    //public Slider hpSlider;
    //#endregion

    // �ʿ�Ӽ� : ü��
    int hp = 100;
    int maxHP = 100;
    public Slider hpSlider;

    public static LHS_EnemyHP Instance;
    
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
            print("ü��1 ����");

            if (hp <= 0)
            {
                print("���� ����");
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
