using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardsPanel : MonoBehaviour
{
    public void MoveUp()
    {
        if (StageManager.stageManager.stage != StageStatus.DEFAULT) return;
        float height = GetComponent<RectTransform>().rect.height;
        Debug.Log("height:" + height);
        GetComponent<RectTransform>().DOLocalMoveY(-Screen.height / 2 + height / 2, 0.15f);
        StageManager.stageManager.stage = StageStatus.DECK_ON;
    }

    public void MoveDown()
    {
        if (StageManager.stageManager.stage != StageStatus.DECK_ON &&
        StageManager.stageManager.stage != StageStatus.BIG_CARD_ON)
            return;
        float height = GetComponent<RectTransform>().rect.height;
        GetComponent<RectTransform>().DOLocalMoveY(-Screen.height / 2 - height / 2, 0.15f);
        StageManager.stageManager.stage = StageStatus.DEFAULT;
    }
}
