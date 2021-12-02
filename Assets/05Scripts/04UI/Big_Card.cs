using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Big_Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image CardImage;
    [SerializeField] private Image CostImage;
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI DetailText;

    [SerializeField] private CardsPanel cardsPanel;
    [SerializeField] private Big_Card_Panel big_Card_Panel;
    public void SetData(CardInfo card)
    {
        NameText.text = card.name;
        DetailText.text = "details";
        big_Card_Panel.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        cardsPanel.MoveDown();
        StageManager.stageManager.stage = StageStatus.SELECTING_CARD;
        // 핸드에 원본카드 덱으로 보내기
        // disable this
        // destroy 원본
        // navigation 띄우기
        StageManager.stageManager.GetPlayer().useCard(CardUI.selected_Card.GetComponent<CardUI>());
        big_Card_Panel.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
