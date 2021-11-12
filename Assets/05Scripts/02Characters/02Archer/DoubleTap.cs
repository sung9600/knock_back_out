using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTap : Archer_Base
{
    public override void Skill()
    {
        // 표식 찍기 ??
    }

    public override void attack()
    {
        if (atk_target == null) { turn_done = true; return; }

        Characters target_front = getTarget(atk_dir.x, atk_dir.y);
        Characters target_back = getTarget(-atk_dir.x, -atk_dir.y);

        if (target_front != null) target_front.GetHit();
        if (target_back != null) target_back.GetHit();

        if (!turn_done) turn_done = true;

    }
}
