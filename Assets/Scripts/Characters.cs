using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[System.Serializable]
public class Pos
{
    public int x;
    public int y;
    public Pos(int x1, int y1)
    {
        this.x = x1;
        this.y = y1;
    }

}
[System.Serializable]
public class Character_stat
{
    public uint hp;
    public uint maxhp;
    public uint shield;
    public uint moverange;
    public atkType atkType;
    public moveType moveType;

    public Character_stat()
    {
        this.hp = 0;
        this.maxhp = 0;
        this.shield = 0;
        this.moverange = 0;
        this.atkType = atkType.direct;
        this.moveType = moveType.ground;
    }
}
public class Characters : MonoBehaviour
{

    //살아있는지, 이번턴 행동했는지
    public bool isdead, done;
    public Character_stat character_Stat;
    public Pos atkpos = new Pos(-1, -1);
    public Pos curpos = new Pos(0, 0);

    public void move((int, int) a)
    {
        int targetx = a.Item1;
        int targety = a.Item2;
    }


    public void onclick()
    {
        List<Pos> candids = MapManager.getPossiblePos(curpos.x, curpos.y);
        //Debug.Log($"count:{candids.Count}");
        foreach (var a in candids)
        {
            int x = a.x;
            int y = a.y;
            StageManager.stageManager.mapManager.tilemaps[2].SetTile(new Vector3Int(x, y, 0),
                StageManager.stageManager.mapManager.nav_tile);
        }
    }
}
