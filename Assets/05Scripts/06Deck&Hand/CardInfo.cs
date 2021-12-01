using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CardInfo
{
    public static int card_num = 1;
    public string name;
    [SerializeField]
    public CardType type;
    public int Cost = 1;
    public int Range = 3;
    public int ID = 0;
    public CardInfo(int a)
    {
        ID = card_num++;
        name = "card" + ID;
        type = CardType.Attack_indirect;
        Cost = 1;
    }

    public CardInfo()
    {
        //Debug.Log("int 생성자");
    }
}
