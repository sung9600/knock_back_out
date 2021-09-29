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
    public int Cost = 1;
    public int Range = 3;
    public int ID = 0;
    public CardInfo(int a)
    {
        ID = Cards.card_num++;
        name = "card" + ID;
        type = CardType.Attack_near;
        Cost = 1;
        Debug.Log(name);
    }

    public CardInfo()
    {
        //Debug.Log("int 생성자");
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
    private Quaternion rotation_origin;
    private static Canvas canvas;
    public static bool usingcard = false;

    public void cardInfoUI()
    {
        texts[0].SetText(cardInfo.name);
        texts[1].SetText(cardInfo.Cost.ToString());
        if (canvas == null)
            canvas = GameObject.Find("UI").transform.GetChild(0).GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        // 여기에 이미지 + 카드효과 등등 표시하는 기능 추가해야함
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
            toOrigin();
            return;
        }

        if (clicked_card == cardInfo.ID)
        {
            if (Input.mousePosition.y >= Screen.height / 2 && !usingcard)
            {

                if (TurnManager.turnManager.cur_cost >= cardInfo.Cost)
                {
                    TurnManager.turnManager.cur_cost -= cardInfo.Cost;
                    UIManager.uIManager.updateCost(TurnManager.turnManager.cur_cost, TurnManager.turnManager.total_cost);
                }
                else
                {
                    Cost.cost_indicator.shake();
                    toOrigin();
                    return;
                }

                Debug.Log("use card");
                StageManager.stageManager.player.useCard(this);
                usingcard = true;
                DeckSystem.deckSystem.toUsedCard(cardInfo);
                rectTransform.anchoredPosition = origin;
                transform.localScale = new Vector3(1, 1, 1);
                //gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("to Origin");
                toOrigin();
            }
        }
    }


    private void toOrigin()
    {
        rectTransform.anchoredPosition = origin;
        transform.localScale = new Vector3(1, 1, 1);
        usingcard = false;
        clicked_card = -1;
        rectTransform.rotation = rotation_origin;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (TurnManager.turnManager.phase != phase.player_turn) return;
        Debug.Log(cardInfo.ID);
        if (clicked_card == -1)
        {
            clicked_card = cardInfo.ID;
            transform.localScale = new Vector3(3, 3, 3);
            rotation_origin = rectTransform.rotation;
            origin = rectTransform.localPosition;
            rectTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
