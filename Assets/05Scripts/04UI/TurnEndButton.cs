using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndButton : Buttons
{
    public static bool player_turn_done;
    public override void OnClick()
    {
        if (TurnManager.turnManager.phase != phase.player_turn) return;
        TurnManager.turnManager.turns[TurnManager.turnManager.turnIndex].index++;
        MoveButtons.nav_on = false;
        MapManager.mapManager.clearNavTiles();
        player_turn_done = true;
        player_turn_done = false;
    }
}
