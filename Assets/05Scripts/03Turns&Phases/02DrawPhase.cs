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
        int n = StageManager.stageManager.getHand().childCount;
        for (int i = 0; i < n; i++)
            Destroy(StageManager.stageManager.getHand().GetChild(i).gameObject);
        n = StageManager.stageManager.GetPlayer().total_card;
        for (int i = 0; i < n; i++)
        {
            GameObject card = Instantiate(StageManager.stageManager.getCardPrefab(), StageManager.stageManager.getHand());
            CardInfo tempCardInfo = DeckSystem.deckSystem.DrawCardFromDeck();
            if (tempCardInfo == null) Debug.Log("null draw");
            card.GetComponent<CardUI>().cardInfo = tempCardInfo;
            card.GetComponent<CardUI>().cardInfoUI();
        }
        StageManager.stageManager.getHand().GetComponent<HandUI>().setWidth();

        return true;
    }
}
