using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform RectTransform;

    public void setWidth()
    {
        int childCount = transform.childCount;
        RectTransform.sizeDelta = new Vector2(childCount * 150, RectTransform.rect.height);
    }
}
