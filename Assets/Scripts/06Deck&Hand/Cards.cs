using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


[System.Serializable]
public class CardInfo
{
    public string name;
    [SerializeField]
    public CardType type;
    public int Cost = 0;
    public int Range = 3;
    public int ID = 0;
    public CardInfo()
    {
        ID = Cards.card_num++;
        name = "card" + ID;
        type = CardType.Attack_near;
        Cost = 1;
        //Debug.Log(name);
    }
}
public class Cards : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public static int card_num = 0;
    public CardInfo cardInfo;
    public TextMeshProUGUI[] texts; // name cost info
    [SerializeField]
    private RectTransform rectTransform;
    public static int clicked_card = -1;
    [SerializeField]
    private Vector3 origin;
    private static Canvas canvas;
    public static bool usingcard = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        texts[0].SetText(cardInfo.name);
        texts[1].SetText(cardInfo.Cost.ToString());
        if (canvas == null)
            canvas = GameObject.Find("UI").transform.GetChild(0).GetComponent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (TurnManager.turnManager.phase != phase.player_turn) return;
        if (clicked_card == this.cardInfo.ID)
            transform.position = Input.mousePosition + new Vector3(0, 250, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (TurnManager.turnManager.phase != phase.player_turn)
        {
            Debug.Log("to Origin : not player turn");
            rectTransform.anchoredPosition = origin;
            transform.localScale = new Vector3(1, 1, 1);
            usingcard = false;
            clicked_card = -1;
        }
        if (clicked_card == cardInfo.ID)
        {
            if (Input.mousePosition.y >= Screen.height / 2 && !usingcard)
            {
                Debug.Log("use card");
                StageManager.stageManager.player.useCard(this);
                usingcard = true;
                DeckSystem.deckSystem.toUsedCard(cardInfo);
                rectTransform.anchoredPosition = origin;
                transform.localScale = new Vector3(1, 1, 1);
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }
            else
            {
                Debug.Log("to Origin");
                rectTransform.anchoredPosition = origin;
                transform.localScale = new Vector3(1, 1, 1);
                usingcard = false;
                clicked_card = -1;
            }
            //clicked_card = -1;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (TurnManager.turnManager.phase != phase.player_turn) return;
        Debug.Log(cardInfo.ID);
        if (clicked_card == -1)
        {
            clicked_card = cardInfo.ID;
            transform.localScale = new Vector3(3, 3, 3);
            //usingcard = true;
        }
    }

}
