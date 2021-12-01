using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeckSystem : MonoBehaviour
{
    public static DeckSystem deckSystem;
    public List<CardInfo> deckTemp = new List<CardInfo>();
    public List<CardInfo> hand = new List<CardInfo>();
    public List<CardInfo> usedCards = new List<CardInfo>();

    public List<CardInfo> deck = new List<CardInfo>();

    private void Awake()
    {
        if (deckSystem == null)
            deckSystem = this;
    }
    private void Start()
    {

        for (int i = 0; i < 30; i++)
        {
            CardInfo cardInfo = new CardInfo(0);
            usedCards.Add(cardInfo);
        }
        //ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        // used -> deckTemp + deckTemp shuffle + deckTemp -> hand
        deckTemp = usedCards.ToList();
        usedCards.Clear();
        int n = deckTemp.Count;
        Debug.Log("n:" + n);
        for (int i = n - 1; i > 0; i--)
        {
            int random = UnityEngine.Random.Range(0, i);
            CardInfo temp = deckTemp[i];
            deckTemp[i] = deckTemp[random];
            deckTemp[random] = temp;
        }
        for (int i = 0; i < n; i++)
        {
            deck.Add(deckTemp[i]);
        }
        deckTemp.Clear();
    }

    public CardInfo DrawCardFromDeck()
    {
        if (deck.Count == 0) ShuffleDeck();
        CardInfo temp = deck[0];
        deck.RemoveAt(0);
        hand.Add(temp);
        //Debug.Log(temp.ID);
        return temp;
    }
    public void toUsedCard(CardInfo cardInfo)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            if (cardInfo.ID == hand[i].ID)
            {
                usedCards.Add(hand[i]);
                hand.Remove(hand[i]);
                break;
            }
        }
    }

    public void clearHand()
    {
        Debug.Log("clearHand");
        int count = StageManager.stageManager.hand.childCount;
        for (int i = 0; i < count; i++)
        {
            CardInfo card = StageManager.stageManager.hand.GetChild(i).GetComponent<CardUI>().cardInfo;
            toUsedCard(card);
            Destroy(StageManager.stageManager.hand.GetChild(i).gameObject);
        }
    }

}
