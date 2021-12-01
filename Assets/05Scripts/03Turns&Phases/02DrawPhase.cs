using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPhase : Phases
{
    public override void OnStartPhase()
    {
        TurnManager.turnManager.indicator.SetText("draw phase");
        TurnManager.turnManager.phase = phase.card_draw;
        StageManager.stageManager.stage = StageStatus.DEFAULT;
    }
    public override bool IsComplete()
    {
        if (DeckSystem.deckSystem.hand.Count != 0)
        {
            DeckSystem.deckSystem.clearHand();
        }
        int n = StageManager.stageManager.hand.childCount;
        for (int i = 0; i < n; i++)
            Destroy(StageManager.stageManager.hand.GetChild(i).gameObject);
        n = StageManager.stageManager.GetPlayer().total_card;
        for (int i = 0; i < n; i++)
        {
            GameObject card = Instantiate(StageManager.stageManager.card_Prefab, StageManager.stageManager.hand);
            card.GetComponent<CardUI>().cardInfo = DeckSystem.deckSystem.DrawCardFromDeck();
            card.GetComponent<CardUI>().cardInfoUI();
        }
        StageManager.stageManager.hand.GetComponent<HandUI>().setWidth();

        return true;
    }
}
