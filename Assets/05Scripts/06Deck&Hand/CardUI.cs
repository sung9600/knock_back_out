using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class CardUI : MonoBehaviour, IPointerClickHandler//, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public CardInfo cardInfo;

    public static bool usingcard = false;


    public static GameObject selected_Card;

    public void cardInfoUI()
    {
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(cardInfo.ID.ToString());
        // 여기에 이미지 + 카드효과 등등 표시하는 기능 추가해야함
    }
    #region dragg
    // public void OnDrag(PointerEventData eventData)
    // {
    //     if (TurnManager.turnManager.phase != phase.player_turn) return;
    //     if (clicked_card == this.cardInfo.ID)
    //     {
    //         //transform.position = Input.mousePosition;// + new Vector3(0, 250, 0);
    //         var screenPoint = (Vector3)eventData.position;
    //         screenPoint.z = 10.0f;
    //         transform.position = Camera.main.ScreenToWorldPoint(screenPoint) + Vector3.up * 2;
    //         Debug.Log(transform.position);
    //     }
    // }

    // public void OnEndDrag(PointerEventData eventData)
    // {
    //     if (TurnManager.turnManager.phase != phase.player_turn)
    //     {
    //         Debug.Log("to Origin : not player turn");
    //         toOrigin();
    //         return;
    //     }

    //     if (clicked_card == cardInfo.ID)
    //     {
    //         if (Input.mousePosition.y >= Screen.height / 2 && !usingcard)
    //         {

    //             if (TurnManager.turnManager.cur_cost >= cardInfo.Cost)
    //             {
    //                 TurnManager.turnManager.cur_cost -= cardInfo.Cost;
    //                 UIManager.uIManager.updateCost(TurnManager.turnManager.cur_cost, TurnManager.turnManager.total_cost);
    //             }
    //             else
    //             {
    //                 Cost.cost_indicator.shake();
    //                 toOrigin();
    //                 return;
    //             }
    //             StageManager.stageManager.GetPlayer().useCard(this);
    //             usingcard = true;
    //             DeckSystem.deckSystem.toUsedCard(cardInfo);
    //             rectTransform.anchoredPosition = origin;
    //             transform.localScale = new Vector3(1, 1, 1);
    //             //gameObject.SetActive(false);
    //             Destroy(this.gameObject);
    //         }
    //         else
    //         {
    //             Debug.Log("to Origin");
    //             toOrigin();
    //         }
    //     }
    // }


    // private void toOrigin()
    // {
    //     transform.SetParent(realParent);
    //     rectTransform.anchoredPosition = origin;
    //     transform.localScale = new Vector3(1, 1, 1);
    //     usingcard = false;
    //     clicked_card = -1;
    //     rectTransform.rotation = rotation_origin;
    // }

    // public void OnBeginDrag(PointerEventData eventData)
    // {
    //     rotation_origin = rectTransform.rotation;
    //     origin = rectTransform.localPosition;
    //     realParent = transform.parent;
    //     if (TurnManager.turnManager.phase != phase.player_turn) return;
    //     if (clicked_card == -1)
    //     {
    //         clicked_card = cardInfo.ID;
    //         transform.localScale = new Vector3(2, 2, 2);
    //         rectTransform.rotation = Quaternion.Euler(0, 0, 0);
    //         transform.SetParent(canvasParent);
    //     }
    // }
    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (StageManager.stageManager.stage)
        {
            case StageStatus.DECK_ON:
                Debug.Log("open big card");
                selected_Card = this.gameObject;
                StageManager.stageManager.big_Card.SetActive(true);
                StageManager.stageManager.big_Card.GetComponent<Big_Card>().SetData(this.cardInfo);
                gameObject.SetActive(false);
                StageManager.stageManager.stage = StageStatus.BIG_CARD_ON;
                break;
            case StageStatus.BIG_CARD_ON:
                Debug.Log("open other big card");
                selected_Card.SetActive(true);
                selected_Card = this.gameObject;
                StageManager.stageManager.big_Card.GetComponent<Big_Card>().SetData(this.cardInfo);
                gameObject.SetActive(false);
                break;
        }
    }
}
