using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Characters
{
    protected string _name;
    public static int static_id;

    public int id;

    [SerializeField]
    protected Characters atk_target;
    [SerializeField]
    protected Pos atk_dir = null;
    protected Pos warning_pos = null;
    public bool turn_done = false;
    [SerializeField]
    protected List<Characters> targets = new List<Characters>();

    [SerializeField]
    protected List<(Pos, int)> candid_pos = new List<(Pos, int)>();
    private void Start()
    {
        StageManager.stageManager.GetCharactersList().Add(this);
    }


    public abstract void SetTarget();
    public void Warning()
    {
        //if (atk_target == null) return;
        atk_dir = atk_target.curpos - curpos;
        /// 이거를 타일로 표시하는거 말고 indicator ( 화살표? )로 표시할수 있도록 바꿔야할듯
        warning_pos = curpos + atk_dir;

        Pos dir = Pos.getDir(curpos, atk_target.curpos);
        switch (stat.atkType)
        {
            case atkType.melee:
                {
                    if (dir.y == 0)
                    {
                        if (dir.x < 0)
                        {
                            transform.GetChild(5).gameObject.SetActive(true);
                            MapManager.mapManager.GetTilemap(2).SetTile(new Vector3Int(atk_target.curpos.x, atk_target.curpos.y, 0), MapManager.mapManager.GetTile(0, 6));
                            break;
                        }
                        else if (dir.x > 0)
                        {
                            transform.GetChild(7).gameObject.SetActive(true);
                            MapManager.mapManager.GetTilemap(2).SetTile(new Vector3Int(atk_target.curpos.x, atk_target.curpos.y, 0), MapManager.mapManager.GetTile(0, 6));
                            break;
                        }
                    }
                    else if (dir.y > 0)
                    {
                        transform.GetChild(6).gameObject.SetActive(true);
                        MapManager.mapManager.GetTilemap(2).SetTile(new Vector3Int(atk_target.curpos.x, atk_target.curpos.y, 0), MapManager.mapManager.GetTile(0, 6));
                        break;
                    }
                    else if (dir.y < 0)
                    {
                        transform.GetChild(4).gameObject.SetActive(true);
                        MapManager.mapManager.GetTilemap(2).SetTile(new Vector3Int(atk_target.curpos.x, atk_target.curpos.y, 0), MapManager.mapManager.GetTile(0, 6));
                        break;
                    }
                    break;
                }
            case atkType.direct:
                {
                    if (dir.y == 0)
                    {
                        if (dir.x < 0)
                        {
                            for (int i = 1; i < Constants.mapHeight; i++)
                            {
                                Pos p = new Pos(curpos.x - i, curpos.y);
                                Debug.Log(p);
                                if (MapManager.checkCantGoTile(p, stat.moveType == moveType.air))
                                {
                                    MapManager.mapManager.GetTilemap(2).SetTile(new Vector3Int(atk_target.curpos.x, atk_target.curpos.y, 0), MapManager.mapManager.GetTile(0, 6));
                                    break;
                                }
                                MapManager.mapManager.GetTilemap(6).SetTile(new Vector3Int(p.x, p.y, 0), MapManager.mapManager.GetTile(7, 0));
                            }
                            break;
                        }
                        else if (dir.x > 0)
                        {
                            for (int i = 1; i < Constants.mapHeight; i++)
                            {
                                Pos p = new Pos(curpos.x + i, curpos.y);
                                Debug.Log(p);
                                if (MapManager.checkCantGoTile(p, stat.moveType == moveType.air))
                                {
                                    MapManager.mapManager.GetTilemap(2).SetTile(new Vector3Int(atk_target.curpos.x, atk_target.curpos.y, 0), MapManager.mapManager.GetTile(0, 6));
                                    break;
                                }
                                MapManager.mapManager.GetTilemap(6).SetTile(new Vector3Int(p.x, p.y, 0), MapManager.mapManager.GetTile(7, 0));
                            }
                            break;
                        }
                    }
                    else if (dir.y > 0)
                    {
                        for (int i = 1; i < Constants.mapHeight; i++)
                        {
                            Pos p = new Pos(curpos.x, curpos.y + i);
                            Debug.Log(p);
                            if (MapManager.checkCantGoTile(p, stat.moveType == moveType.air))
                            {
                                MapManager.mapManager.GetTilemap(2).SetTile(new Vector3Int(atk_target.curpos.x, atk_target.curpos.y, 0), MapManager.mapManager.GetTile(0, 6));
                                break;
                            }
                            MapManager.mapManager.GetTilemap(7).SetTile(new Vector3Int(p.x, p.y, 0), MapManager.mapManager.GetTile(7, 1));
                        }
                    }
                    else if (dir.y < 0)
                    {
                        for (int i = 1; i < Constants.mapHeight; i++)
                        {
                            Pos p = new Pos(curpos.x, curpos.y - i);
                            Debug.Log(p);
                            if (MapManager.checkCantGoTile(p, stat.moveType == moveType.air))
                            {
                                MapManager.mapManager.GetTilemap(2).SetTile(new Vector3Int(atk_target.curpos.x, atk_target.curpos.y, 0), MapManager.mapManager.GetTile(0, 6));
                                break;
                            }
                            MapManager.mapManager.GetTilemap(7).SetTile(new Vector3Int(p.x, p.y, 0), MapManager.mapManager.GetTile(7, 1));
                        }
                        break;
                    }
                    break;
                }
            case atkType.indirect:
                {
                    if (dir.y == 0)
                    {
                        if (dir.x < 0)
                        {
                            transform.GetChild(1).gameObject.SetActive(true);
                            break;
                        }
                        else if (dir.x > 0)
                        {
                            transform.GetChild(3).gameObject.SetActive(true);
                            break;
                        }
                    }
                    else if (dir.y > 0)
                    {
                        transform.GetChild(2).gameObject.SetActive(true);
                        break;
                    }
                    else if (dir.y < 0)
                    {
                        transform.GetChild(0).gameObject.SetActive(true);
                        break;
                    }
                    break;
                }
        }
    }
    public abstract void Skill();

    public override void init(Pos pos)
    {
        //Debug.Log("war init");
        id = static_id++;
        targets.Add(StageManager.stageManager.GetPlayer());
        curpos = pos;
        ChangeMapByte();
        //Debug.Log(curpos.x + "," + curpos.y + ":" + MapManager.mapManager.map[curpos.x, curpos.y]);
    }
}
