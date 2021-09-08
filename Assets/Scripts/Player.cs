using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Characters
{
    public void showNav()
    {
        List<Pos> candids = MapManager.getPossiblePos(curpos.x, curpos.y);
        foreach (var a in candids)
        {
            int x = a.x;
            int y = a.y;
            StageManager.stageManager.mapManager.tilemaps[2].SetTile(new Vector3Int(x, y, 0),
                StageManager.stageManager.mapManager.nav_tile);
        }
    }
}
