using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Big_Card_Panel : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {

        Debug.Log("close big card");
        StageManager.stageManager.stage = StageStatus.DECK_ON;
        StageManager.stageManager.getCardPreview().SetActive(false);
        CardUI.selected_Card.SetActive(true);
        gameObject.SetActive(false);
    }
}
