using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Characters
{
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
    public void useCard(Cards.CardInfo info)
    {
        List<Pos> tiles = new List<Pos>();
        switch (info.type)
        {
            case CardType.Attack:
                int range = info.Range;
                switch (character_Stat.atkType)
                {
                    case atkType.direct:
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
                    case atkType.melee:
                        break;
                    case atkType.indirect:
                        for (int i = 0; i < Constants.mapHeight; i++)
                        {
                            for (int j = 0; j < Constants.mapWidth; j++)
                            {
                            }
                        }
                        break;
                }
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
