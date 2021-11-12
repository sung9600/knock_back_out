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
            Destroy(StageManager.stageManager.hand1.GetChild(i).gameObject);
        n = StageManager.stageManager.hand2.childCount;
        for (int i = 0; i < n; i++)
            Destroy(StageManager.stageManager.hand2.GetChild(i).gameObject);
        n = StageManager.stageManager.GetPlayer().total_card;
        for (int i = 0; i < n; i++)
        {
            if (i < 5)
            {
                GameObject card = Instantiate(StageManager.stageManager.card_Prefab, StageManager.stageManager.hand1);
                card.GetComponent<CardUI>().cardInfo = DeckSystem.deckSystem.DrawCardFromDeck();
                card.GetComponent<CardUI>().cardInfoUI();

            }
            else
            {
                GameObject card = Instantiate(StageManager.stageManager.card_Prefab, StageManager.stageManager.hand2);
                card.GetComponent<CardUI>().cardInfo = DeckSystem.deckSystem.DrawCardFromDeck();
                card.GetComponent<CardUI>().cardInfoUI();
            }

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
