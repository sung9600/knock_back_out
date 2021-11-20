using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class CardUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public static int card_num = 0;
    public CardInfo cardInfo;
    private RectTransform rectTransform;
    public static int clicked_card = -1;
    [SerializeField]
    private Vector3 origin;
    private Quaternion rotation_origin;
    private static Canvas canvas;

    public static bool usingcard = false;

    private static Transform canvasParent = null;
    private static Transform realParent = null;

    public void cardInfoUI()
    {
        if (canvas == null)
            canvas = GameObject.Find("UI").transform.GetChild(0).GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(cardInfo.ID.ToString());
        canvasParent = GameObject.Find("DefaultUI").transform;
        // 여기에 이미지 + 카드효과 등등 표시하는 기능 추가해야함
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (TurnManager.turnManager.phase != phase.player_turn) return;
        if (clicked_card == this.cardInfo.ID)
        {
            //transform.position = Input.mousePosition;// + new Vector3(0, 250, 0);
            var screenPoint = (Vector3)eventData.position;
            screenPoint.z = 10.0f;
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint) + Vector3.up * 2;
            Debug.Log(transform.position);
        }
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
                StageManager.stageManager.GetPlayer().useCard(this);
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
        transform.SetParent(realParent);
        rectTransform.anchoredPosition = origin;
        transform.localScale = new Vector3(1, 1, 1);
        usingcard = false;
        clicked_card = -1;
        rectTransform.rotation = rotation_origin;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rotation_origin = rectTransform.rotation;
        origin = rectTransform.localPosition;
        realParent = transform.parent;
        if (TurnManager.turnManager.phase != phase.player_turn) return;
        if (clicked_card == -1)
        {
            clicked_card = cardInfo.ID;
            transform.localScale = new Vector3(2, 2, 2);
            rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            transform.SetParent(canvasParent);
        }
    }

}
