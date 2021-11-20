using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager uIManager;

    private void Awake()
    {
        if (uIManager == null)
            uIManager = this;
    }

    [SerializeField]
    private TextMeshProUGUI cost;
    public void updateCost(int curr, int total)
    {
        cost.SetText(curr.ToString());
    }




}
