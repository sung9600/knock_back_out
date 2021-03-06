using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Characters
{
    public GameObject bullet_prefab;
    public CardUI usedcard;
    public int remain_move;
    public int moverange;
    public int total_card = 10;


    public void showNav(List<Pos> tiles)
    {
        foreach (var a in tiles)
        {
            int x = a.x;
            int y = a.y;
            MapManager.mapManager.GetTilemap(1).SetTile(new Vector3Int(x, y, 0),
                MapManager.mapManager.GetTile(0, 5));
        }

    }
    public void useCard(CardUI info)
    {
        List<Pos> tiles = new List<Pos>();
        usedcard = info;
        switch (info.cardInfo.Card_type)
        {
            case CardType.Attack_near:
                for (int i = 0; i < 4; i++)
                {
                    int xx = curpos.x + Constants.dx[i];
                    int yy = curpos.y + Constants.dy[i];
                    if (MapManager.checkWidthHeight(xx, yy))
                    {
                        tiles.Add(new Pos(xx, yy));
                    }
                }
                showNav(tiles);
                break;
            case CardType.Attack_direct:
                for (int i = 0; i < 4; i++)
                {
                    int dx = Constants.dx[i];
                    int dy = Constants.dy[i];
                    for (int k = 0; k < 2; k++)
                    {
                        for (int j = 1; j < Constants.mapHeight; j++)
                        {
                            int nx = dx * j * (k == 0 ? 1 : -1) + curpos.x;
                            int ny = dy * j * (k == 0 ? 1 : -1) + curpos.y;
                            if (!MapManager.checkWidthHeight(nx, ny))
                            {
                                break;
                            }
                            if (MapManager.mapManager.map[nx, ny] == (int)tileType.rock)
                            {
                                break;
                            }
                            tiles.Add(new Pos(nx, ny));
                        }
                    }
                }
                showNav(tiles);
                break;
            case CardType.Attack_indirect:
                for (int i = 0; i < 4; i++)
                {
                    int dx = Constants.dx[i];
                    int dy = Constants.dy[i];
                    for (int k = 0; k < 2; k++)
                    {
                        for (int j = 2; j < Constants.mapHeight; j++)
                        {
                            int nx = dx * j * (k == 0 ? 1 : -1) + curpos.x;
                            int ny = dy * j * (k == 0 ? 1 : -1) + curpos.y;
                            if (!MapManager.checkWidthHeight(nx, ny))
                            {
                                break;
                            }
                            tiles.Add(new Pos(nx, ny));
                        }
                    }
                }
                showNav(tiles);
                break;
            case CardType.Skill:
                Debug.Log("used skill");
                break;
            case CardType.Magic:
                Debug.Log("used magic");
                break;
            case CardType.Debuff:
                Debug.Log("used debuff");
                break;
        }
    }

    public override void init(Pos pos)
    {
        //  PARENT??? CHARACTERS??? AWAKE?????? ?????????
        //  TODO: ???????????? ???????????? ?????? ??????????????? ?????? ???????

        base.init(pos);
        moverange = stat.moverange;
        remain_move = moverange;
        MapManager.mapManager.map[curpos.x, curpos.y] |= 0x40000000;
        indicators_Controller.UpdateHP(stat.maxhp, stat.hp);
    }
    private void Start()
    {
        StageManager.stageManager.GetCharactersList().Add(this);
        init(curpos);
    }
}
