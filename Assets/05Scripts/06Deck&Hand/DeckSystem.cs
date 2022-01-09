using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
public class DeckSystem : MonoBehaviour
{
    public static DeckSystem deckSystem;
    public List<CardInfo> deckTemp = new List<CardInfo>();
    public List<CardInfo> hand = new List<CardInfo>();
    public List<CardInfo> ExceptDeck = new List<CardInfo>();
    public List<CardInfo> TombDeck = new List<CardInfo>();

    public List<CardInfo> MainDeck = new List<CardInfo>();
    [SerializeField]
    private TextMeshProUGUI[] DeckTexts; // 덱 , 제외덱 , 무덤덱


    private void Awake()
    {
        if (deckSystem == null)
            deckSystem = this;
    }
    private void Start()
    {

        ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(51));
        ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(51));
        ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(24));
        ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(24));
        ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(2));
        ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(2));
        ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(2));
        ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(2));
        ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(30));
        ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(42));

        // ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(51));
        // ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(51));
        // ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(24));
        // ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(24));
        // ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(2));
        // ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(2));
        // ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(2));
        // ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(2));
        // ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(30));
        // ExceptDeck.Add(StageManager.stageManager.getCardOriginalByNum(42));

        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        // used -> deckTemp + deckTemp shuffle + deckTemp -> hand
        deckTemp = ExceptDeck.ToList();
        ExceptDeck.Clear();
        int n = deckTemp.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int random = UnityEngine.Random.Range(0, i);
            CardInfo temp = deckTemp[i];
            deckTemp[i] = deckTemp[random];
            deckTemp[random] = temp;
        }
        for (int i = 0; i < n; i++)
        {
            MainDeck.Add(deckTemp[i]);
        }
        deckTemp.Clear();
        UpdateDeckCounts();
    }

    public CardInfo DrawCardFromDeck()
    {
        if (MainDeck.Count == 0) ShuffleDeck();
        CardInfo temp = MainDeck[0];
        MainDeck.RemoveAt(0);
        hand.Add(temp);

        //Debug.Log(temp.ID);
        UpdateDeckCounts();
        return temp;
    }
    public void toUsedCard(CardInfo cardInfo)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            if (cardInfo.ID == hand[i].ID)
            {
                ExceptDeck.Add(hand[i]);
                hand.Remove(hand[i]);
                break;
            }
        }
        UpdateDeckCounts();
    }

    public void clearHand()
    {
        Debug.Log("clearHand");
        int count = StageManager.stageManager.getHand().childCount;
        for (int i = 0; i < count; i++)
        {
            CardInfo card = StageManager.stageManager.getHand().GetChild(i).GetComponent<CardUI>().cardInfo;
            toUsedCard(card);
            Destroy(StageManager.stageManager.getHand().GetChild(i).gameObject);
        }
        UpdateDeckCounts();
    }

    public void UpdateDeckCounts()
    {
        DeckTexts[0].SetText(MainDeck.Count.ToString());
        DeckTexts[1].SetText(ExceptDeck.Count.ToString());
        DeckTexts[2].SetText(TombDeck.Count.ToString());
    }
}
