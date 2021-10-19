using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhase : Phases
{
    public int remain_move;
    public override void OnStartPhase()
    {
        TurnManager.turnManager.indicator.SetText("player phase");
        TurnManager.turnManager.phase = phase.player_turn;
        remain_move = StageManager.stageManager.GetPlayer().stat.moverange;
    }
    public override bool IsComplete()
    {
        if (!TurnEndButton.player_turn_done) return false;
        TurnEndButton.player_turn_done = false;
        return true;
    }
}
