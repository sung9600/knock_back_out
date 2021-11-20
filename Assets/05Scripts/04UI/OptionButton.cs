using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : Buttons
{
    [SerializeField]
    private GameObject optionWindow;
    public override void OnClick()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
            optionWindow.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            optionWindow.SetActive(false);
        }
    }
}
