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

    public Queue<CardInfo> deck = new Queue<CardInfo>();
    [SerializeField]
    private List<Cards> handGO;

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
        ShuffleDeck();
        // for (int i = 0; i < hand.Count; i++)
        // {
        //     handGO[i].cardInfo = hand[i];
        //     handGO[i].gameObject.SetActive(true);
        // }
    }

    public void ShuffleDeck()
    {
        // used -> deckTemp + deckTemp shuffle + deckTemp -> hand
        deckTemp = usedCards.ToList();
        usedCards.Clear();
        int n = deckTemp.Count - 1;
        for (int i = n; i >= 0; i--)
        {
            int random = UnityEngine.Random.Range(0, i);
            CardInfo temp = deckTemp[i];
            deckTemp[i] = deckTemp[random];
            deckTemp[random] = temp;
        }
        for (int i = 0; i < n; i++)
        {
            deck.Enqueue(deckTemp[i]);
        }
        deckTemp.Clear();
    }

    public CardInfo DrawCardFromDeck()
    {
        CardInfo temp = deck.Dequeue();
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

}
