using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPhase : Phases
{
    public override void OnStartPhase()
    {
        TurnManager.turnManager.indicator.SetText("draw phase");
    }
    public override bool IsComplete()
    {
        while (DeckSystem.deckSystem.hand.Count != 0)
        {
            DeckSystem.deckSystem.toUsedCard(DeckSystem.deckSystem.hand[0]);
        }
        int n = StageManager.stageManager.hand.childCount;
        for (int i = 0; i < n; i++)
        {
            StageManager.stageManager.hand.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < 5; i++)
        {
            StageManager.stageManager.hand.GetChild(i).GetComponent<Cards>().cardInfo
                = DeckSystem.deckSystem.DrawCardFromDeck();
            StageManager.stageManager.hand.GetChild(i).gameObject.SetActive(true);
        }

        return true;
    }
}
