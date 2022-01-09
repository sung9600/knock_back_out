using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card_Use_AbsType : Object
{
    private card_Type1 type1;
    private card_Type2 type2;

    public card_Type1 GetType1() { return type1; }
    public card_Type2 GetType2() { return type2; }

    public abstract void useCard();
}
