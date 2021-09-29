using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class Cost : MonoBehaviour
{

    public static Cost cost_indicator;

    private void Awake()
    {
        if (cost_indicator == null)
            cost_indicator = this;
        gameObject.GetComponent<Image>().color = new Color32(255, 255, 0, 255);
    }

    public void shake()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOShakePosition(0.5f, 25, 50, 90, false, true)
        .OnPlay(() => gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255)))
        .OnComplete(() => gameObject.GetComponent<Image>().color = new Color32(255, 255, 0, 255));

    }

}
