using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowler : Warrior_Base
{
    public override void Skill()
    {
        stat.shield++;
        status = Character_status.attacking;
    }
}
