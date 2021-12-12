using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frowler : Warrior_Base
{
    private bool can_Divide = false;
    public bool can_divide() { return can_Divide; }
    public void set_can_divide(bool set) { can_Divide = set; }
    public override void Skill()
    {
        Debug.Log("skill");
        if (can_Divide)
        {
            int target_hp = stat.hp / 2;
            stat.hp = target_hp;

            // get random empty position
            // + instantiate clone

            for (int i = 0; i < 4; i++)
            {


            }

            can_Divide = false;
        }
        else
        {
            return;
        }
    }
}
