using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButtons : Buttons
{
    public static bool nav_on = false;
    public override void OnClick()
    {
        if (TurnManager.turnManager.phase != phase.player_turn) return;
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
                StageManager.stageManager.mapManager.clearNavTiles();
                nav_on = false;
            }

        }
    }
}
