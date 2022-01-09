using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnEndButton : Buttons
{
    public static bool player_turn_done;
    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public override void OnClick()
    {
        if (TurnManager.turnManager.phase != phase.player_turn) return;
        TurnManager.turnManager.turns[TurnManager.turnManager.turnIndex].index = 4;
        MoveButtons.nav_on = false;
        MapManager.mapManager.clearNavTiles(1);
        DeckSystem.deckSystem.clearHand();
        player_turn_done = true;
        StageManager.stageManager.inputManager.get_lineon(false);
        StageManager.stageManager.stage = StageStatus.CANNOT_TOUCH;
    }
}
