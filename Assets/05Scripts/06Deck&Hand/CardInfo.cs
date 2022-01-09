using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/CardInfo", fileName = "Card", order = 0)]
public class CardInfo : ScriptableObject
{
    public int Card_Type_Num;
    public static int Card_num = 1;
    public string Card_name;
    public string Card_detail;
    [SerializeField]
    public CardType Card_type;
    public int Card_Cost = 1;
    public int ID = 0;

    public Sprite[] card_Sprites;
    // 일러스트 , 베이스 , 속성1 , 속성2
    public CardInfo(int a)
    {
        ID = Card_num++;
        Card_name = "card" + ID;
        Card_type = CardType.Attack_indirect;
        Card_Cost = 1;
    }

    public CardInfo()
    {
        //Debug.Log("int 생성자");
    }
}
