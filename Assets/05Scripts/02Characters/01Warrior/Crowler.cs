using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowler : Warrior_Base
{
    public override void Skill()
    {
        Debug.Log("skill");
        stat.shield++;
        status = Character_status.attacking;
    }
}
