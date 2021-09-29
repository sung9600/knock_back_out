using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPhase : Phases
{
    public override void OnStartPhase()
    {
        TurnManager.turnManager.indicator.SetText("draw phase");
        TurnManager.turnManager.phase = phase.card_draw;
    }
    public override bool IsComplete()
    {
        while (DeckSystem.deckSystem.hand.Count != 0)
        {
            DeckSystem.deckSystem.toUsedCard(DeckSystem.deckSystem.hand[0]);
        }
        int n = StageManager.stageManager.hand1.childCount;
        for (int i = 0; i < n; i++)
            Destroy(StageManager.stageManager.hand1.GetChild(i));
        n = StageManager.stageManager.hand2.childCount;
        for (int i = 0; i < n; i++)
            Destroy(StageManager.stageManager.hand2.GetChild(i));

        for (int i = 0; i < 5; i++)
        {
            GameObject card = Instantiate(StageManager.stageManager.card_Prefab, StageManager.stageManager.hand1);
            card.GetComponent<Cards>().cardInfo = DeckSystem.deckSystem.DrawCardFromDeck();
            card.GetComponent<Cards>().cardInfoUI();
            card = Instantiate(StageManager.stageManager.card_Prefab, StageManager.stageManager.hand2);
            card.GetComponent<Cards>().cardInfo = DeckSystem.deckSystem.DrawCardFromDeck();
            card.GetComponent<Cards>().cardInfoUI();
            // StageManager.stageManager.hand1.GetChild(i).GetComponent<Cards>().cardInfo
            //     = DeckSystem.deckSystem.DrawCardFromDeck();
            // StageManager.stageManager.hand1.GetChild(i).gameObject.SetActive(true);
            // StageManager.stageManager.hand2.GetChild(i).GetComponent<Cards>().cardInfo
            //     = DeckSystem.deckSystem.DrawCardFromDeck();
            // StageManager.stageManager.hand2.GetChild(i).gameObject.SetActive(true);
        }

        return true;
    }
}
