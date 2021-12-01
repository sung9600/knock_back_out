using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButtons : Buttons
{
    public static bool nav_on = false;

    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
    public override void OnClick()
    {
        // 플레이어 턴인지
        if (TurnManager.turnManager.phase != phase.player_turn) return;
        // 잔여 이동이 남아있는지
        if (StageManager.stageManager.GetPlayer().remain_move <= 0) return;
        //         if (StageManager.stageManager.GetPlayer().status != Character_status.waiting)
        //             return;

        // 일단 스테이지 바꿔서 다른행동 막고
        if (StageManager.stageManager.stage == StageStatus.DEFAULT || StageManager.stageManager.stage == StageStatus.SELECTING_MOVE)
        {
            // navigation 안켜진 상태
            if (!nav_on)
            {
                Debug.Log("nav on");
                StageManager.stageManager.stage = StageStatus.SELECTING_MOVE;

                // 이동가능 타일 표시
                Player player = StageManager.stageManager.GetPlayer();
                Pos player_curpos = player.curpos;
                player.showNav(MapManager.getPossiblePos(player_curpos.x, player_curpos.y, player.remain_move, player.stat.moveType));
                nav_on = true;
            }
            // 이미 켜진상태 => 끄기
            else
            {
                Debug.Log("nav off");
                StageManager.stageManager.stage = StageStatus.DEFAULT;
                MapManager.mapManager.clearNavTiles(1);
                nav_on = false;
            }

        }


        // if (CardUI.clicked_card == -1)
        // {
        //     if (!nav_on && !CardUI.usingcard)
        //     {
        //         if (StageManager.stageManager.GetPlayer().status != Character_status.waiting)
        //             return;
        //         Pos curpos = StageManager.stageManager.GetPlayer().curpos;
        //         StageManager.stageManager.GetPlayer().showNav(MapManager.getPossiblePos(curpos.x, curpos.y
        //             , StageManager.stageManager.GetPlayer().remain_move
        //             , StageManager.stageManager.GetPlayer().stat.moveType
        //             ));
        //         nav_on = true;
        //     }
        //     else
        //     {
        //         MapManager.mapManager.clearNavTiles();
        //         nav_on = false;
        //     }

        // }
    }
}
