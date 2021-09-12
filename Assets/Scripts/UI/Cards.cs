using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class Cards : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public class CardInfo
    {
        public string name;
        public CardType type;
        public int Cost = 0;
        public int Range = 3;
        public int ID = 0;
        public CardInfo()
        {
            ID = Cards.card_num++;
            name = "card" + ID;
            type = CardType.Attack;
            Cost = 1;
        }
    }

    public static int card_num = 0;
    public CardInfo cardInfo;
    public TextMeshProUGUI[] texts = new TextMeshProUGUI[3]; // name cost info
    [SerializeField]
    private RectTransform rectTransform;
    public static int clicked_card = -1;
    private Vector3 origin;
    private static Canvas canvas;
    public static bool usingcard = false;
    [SerializeField]
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cardInfo = new CardInfo();
        texts[0].SetText(cardInfo.name);
        texts[1].SetText(cardInfo.Cost.ToString());
        if (canvas == null)
            canvas = GameObject.Find("UI").transform.GetChild(0).GetComponent<Canvas>();
    }

    public void SetOrigin(Vector3 pos)
    {
        origin = pos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (clicked_card == this.cardInfo.ID)
            transform.position = Input.mousePosition + new Vector3(0, 500, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (clicked_card == this.cardInfo.ID)
        {
            if (Input.mousePosition.y >= Screen.height / 2 && !usingcard)
            {
                Debug.Log("use card");
                StageManager.stageManager.player.useCard(cardInfo);
                usingcard = true;
                Destroy(gameObject);
            }
            else
            {
                rectTransform.transform.localPosition = origin;
                transform.localScale = new Vector3(1, 1, 1);
                usingcard = false;
            }
            //clicked_card = -1;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (clicked_card == -1)
        {
            clicked_card = this.cardInfo.ID;
            transform.localScale = new Vector3(3, 3, 3);
            //usingcard = true;
        }
    }

}
