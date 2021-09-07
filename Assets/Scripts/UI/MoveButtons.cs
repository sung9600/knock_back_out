using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButtons : Buttons
{
    public static bool nav_on = false;
    public override void OnClick()
    {
        Debug.Log(nav_on);
        if (!nav_on)
        {
            StageManager.stageManager.player.onclick();
            nav_on = true;
        }
        else
        {
            for (int i = 0; i < Constants.mapHeight; i++)
            {
                for (int j = 0; j < Constants.mapWidth; j++)
                {
                    StageManager.stageManager.mapManager.tilemaps[2].SetTile(new Vector3Int(i, j, 0), null);
                }
            }
            nav_on = false;
        }
    }
}
