using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grinder : Archer_Base
{

    public override void Skill()
    {
        stat.shield++;
        // strength +1

    }
    public void _catch(Pos direction)
    {
        // when?
        for (int i = 1; i < 5; i++)
        {
            Pos target = i * direction + curpos;
            // 돌이 있거나 or 캐릭터가 있으면 stop 
            if (!MapManager.isEmptyTile(target.x, target.y))
            {
                // 끌 대상 있음
                // mapmanager에서 대상 누군지 확인하고
                // 대상 curpos 바꾸고
                // 끌려오는 애니메이션
                // 끌려오는 위치는 grinder의 curpos + direction
                // 위치 바꾼 후 위치타일 효과 확인
                
                break;
            }
            if (MapManager.groundInfo(target.x, target.y) == (int)tileType.rock)
            {
                //  갈고리가 돌에 부딛힘
                break;
            }

        }
    }

}
