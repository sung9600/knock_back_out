using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Indicators_Controller : MonoBehaviour
{
    [SerializeField]
    private Transform AtkIndicator_Parent;
    [SerializeField]
    private Transform HP_Indicator_Parent;
    [SerializeField]
    private Transform Shield_Indicator_Parent;
    [SerializeField]
    private Transform Buff_Indicator_Parent;


    public void Init()
    {
        AtkIndicator_Parent = transform.Find("AtkIndicators");
        HP_Indicator_Parent = transform.Find("Indicators").Find("HP_Shield Indicators").Find("HP");
        Shield_Indicator_Parent = transform.Find("Indicators").Find("HP_Shield Indicators").Find("Shield");
        Buff_Indicator_Parent = transform.Find("Indicators").Find("Buff Indicators");
    }

    public void UpdateHP(int Max_HP, int Cur_HP)
    {
        HP_Indicator_Parent.Find("HPBar").Find("Slider").GetComponent<Slider>().value = 1 - (float)Cur_HP / Max_HP;
    }

    public void ShieldOn()
    {
        Shield_Indicator_Parent.gameObject.SetActive(true);
    }
    public void ShieldOff()
    {
        Shield_Indicator_Parent.gameObject.SetActive(false);
    }
    public void UpdateShield(int Max_Shield, int Cur_Shield)
    {
        Shield_Indicator_Parent.Find("ShieldBar").Find("Slider").GetComponent<Slider>().value = 1 - (float)Cur_Shield / Max_Shield;
    }
}
