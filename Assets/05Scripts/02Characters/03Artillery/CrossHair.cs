using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : Artillery_Base
{
    public override void attack()
    {
        // 밀치기 넣어야하나?
        MapManager.mapManager.GetTilemap(2).SetTile(new Vector3Int(warning_pos.x, warning_pos.y, 0), null);

        Pos pos = curpos + atk_dir;
        for (int i = 0; i < 4; i++)
        {
            Pos p = pos + new Pos(Constants.dx[i], Constants.dy[i]);
            if (!MapManager.checkWidthHeight(p)) continue;
            int t = MapManager.groundInfo(p);
            switch (t)
            {
                case (int)tileType.flame:
                case (int)tileType.water:
                case (int)tileType.rock:
                    continue;
                case (int)tileType.grass:
                case (int)tileType.smoke:
                    continue;
                default:
                    MapManager.mapManager.map[p.x, p.y] |= 0x00000007;
                    MapManager.mapManager.update_tileanims(p.x, p.y, 0);
                    break;

            }
        }
        if (!turn_done) turn_done = true;

    }
}
