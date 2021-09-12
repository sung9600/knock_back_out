using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButtons : Buttons
{
    public static bool nav_on = false;
    public override void OnClick()
    {
        if (Cards.clicked_card == -1)
        {
            if (!nav_on && !Cards.usingcard)
            {
                if (StageManager.stageManager.player.status == Character_status.moving)
                    return;
                Pos curpos = StageManager.stageManager.player.curpos;
                StageManager.stageManager.player.showNav(MapManager.getPossiblePos(curpos.x, curpos.y));
                nav_on = true;
            }
            else
            {
                for (int i = 0; i < Constants.mapHeight; i++)
                {
                    for (int j = 0; j < Constants.mapWidth; j++)
                    {
                        StageManager.stageManager.mapManager.tilemaps[2].SetTile(new Vector3Int(i, j, 0), null);
                    }
                }
                nav_on = false;
            }

        }
    }
}
