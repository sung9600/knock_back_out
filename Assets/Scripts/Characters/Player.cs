using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Characters
{
    public static Cards usedCardInfo;
    public void showNav(List<Pos> tiles)
    {
        foreach (var a in tiles)
        {
            int x = a.x;
            int y = a.y;
            StageManager.stageManager.mapManager.tilemaps[2].SetTile(new Vector3Int(x, y, 0),
                StageManager.stageManager.mapManager.nav_tile);
        }

    }
    public void useCard(Cards info)
    {
        List<Pos> tiles = new List<Pos>();
        usedCardInfo = info;
        switch (info.cardInfo.type)
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
                            if (StageManager.stageManager.mapManager.map[ny + nx * Constants.mapWidth] == (int)tileType.rock)
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
    private void getCardRanges()
    {

    }
}
