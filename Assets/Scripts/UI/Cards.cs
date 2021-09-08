using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.EventSystems;

public class Cards : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public class CardInfo
    {
        public string name;
        public CardType type;
        public int Cost = 0;
        public int ID = 0;
        public CardInfo()
        {
            ID = card_num++;
            name = "card" + ID;
            type = CardType.Attack;
            Cost = 1;
        }
    }
    public static int card_num = 0;
    public CardInfo cardInfo;
    public TextMeshProUGUI[] texts = new TextMeshProUGUI[3]; // name cost info
    Camera cam;
    private RectTransform rectTransform;
    public static bool usingcard = false;
    public static Cards selected = null;
    private Vector3 origin;
    private static Canvas canvas;
    private void Awake()
    {
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        cardInfo = new CardInfo();
        texts[0].SetText(cardInfo.name);
        texts[1].SetText(cardInfo.Cost.ToString());
        if (canvas == null)
            canvas = GameObject.Find("UI").transform.GetChild(0).GetComponent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (rectTransform.position.y >= 960)
        {
            Debug.Log($"use card {name}");
            Destroy(gameObject);
        }
        else
        {
            rectTransform.transform.position = origin;
        }
        usingcard = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        origin = rectTransform.position;
        usingcard = true;
        selected = this;
        if (MoveButtons.nav_on) MoveButtons.nav_on = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = 1.5f * transform.localScale;
        if (!usingcard)
        {
            GetComponent<Image>().color = Color.blue;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"{name}exit");
        transform.localScale = (1 / 1.5f) * transform.localScale;
        if (!usingcard)
        {
            GetComponent<Image>().color = Color.white;

        }
    }

}
