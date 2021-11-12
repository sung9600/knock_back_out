using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Archer_Base
{
    public override void Skill()
    {
        // 힘+2
    }

    public override void attack()
    {
        if (atk_dir == null) { turn_done = true; return; }
        for (int i = 1; i < Constants.mapHeight; i++)
        {
            Pos pos = curpos + i * atk_dir;
            // 지금은 돌에 부딛히는지만 판별하는데 물인지도 판별해서 돌진중 물 =>사망
            if (MapManager.groundInfo(pos.x, pos.y) == (int)tileType.rock) break;

            Characters target = StageManager.stageManager.GetCharacterByVector3Int(new Vector3Int(pos.x, pos.y, 0));
            if (target != null)
            {
                // 밀치고 데미지 
                break;
            }
        }
    }
}
