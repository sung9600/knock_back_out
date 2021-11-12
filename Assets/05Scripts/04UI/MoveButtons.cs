using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButtons : Buttons
{
    public static bool nav_on = false;
    public override void OnClick()
    {

        if (TurnManager.turnManager.phase != phase.player_turn) return;
        if (StageManager.stageManager.GetPlayer().remain_move <= 0) return;
        if (CardUI.clicked_card == -1)
        {
            if (!nav_on && !CardUI.usingcard)
            {
                if (StageManager.stageManager.GetPlayer().status != Character_status.waiting)
                    return;
                Pos curpos = StageManager.stageManager.GetPlayer().curpos;
                StageManager.stageManager.GetPlayer().showNav(MapManager.getPossiblePos(curpos.x, curpos.y
                    , StageManager.stageManager.GetPlayer().remain_move
                    , StageManager.stageManager.GetPlayer().stat.moveType
                    ));
                nav_on = true;
            }
            else
            {
                MapManager.mapManager.clearNavTiles();
                nav_on = false;
            }

        }
    }
}
