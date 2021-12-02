using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery_Base : Enemy
{
    public override void init(Pos pos)
    {
        targets.Add(StageManager.stageManager.GetPlayer());
        curpos = pos;
        ChangeMapByte();
        //(curpos.x + "," + curpos.y + ":" + MapManager.mapManager.map[curpos.x, curpos.y]);
    }
    public override void SetTarget()
    {
        atk_target = null;
        atk_dir = null;
        status = Character_status.waiting;
        List<(Pos, int)> candid_pos = new List<(Pos, int)>();
        foreach (Characters target in targets)
        {
            Pos target_position = target.curpos;
            for (int index = 0; index < 4; index++)
            {
                for (int i = 5; i > 1; i--)
                {
                    int nx = target_position.x + i * Constants.dx[index];
                    int ny = target_position.y + i * Constants.dy[index];
                    if (nx == curpos.x && ny == curpos.y)
                    {
                        candid_pos.Add((new Pos(nx, ny), targets.IndexOf(target)));
                        break;
                    }

                    if (MapManager.checkWidthHeight(nx, ny) && !MapManager.checkCantGoTile(nx, ny, stat.moveType == moveType.ground))
                    {
                        candid_pos.Add((new Pos(nx, ny), targets.IndexOf(target)));
                        break;
                    }
                }
            }
        }

        Pos final_pos;
        List<Pos> final_path = new List<Pos>();
        foreach ((Pos, int) element in candid_pos)
        {
            if (curpos.x == element.Item1.x && curpos.y == element.Item1.y)
            {
                status = Character_status.attacking;
                final_pos = curpos;
                final_path = null;
                atk_target = targets[element.Item2];
                break;
            }
            List<Pos> path = MapManager.getPath(curpos.x, curpos.y, element.Item1.x, element.Item1.y, stat.moveType == moveType.ground);
            if (path == null)
            {
                // 경로없음
                //Debug.Log("null path");
                continue;
            }
            if (path.Count <= stat.moverange + 1)
            {
                // 이동가능 범위 내 => 반반확률로 변경
                if (atk_target == null)
                {
                    // 첫 타겟인경우
                    atk_target = targets[element.Item2];
                    final_path = path;

                }
                else if (Random.Range(0, 10) < 5)
                {
                    atk_target = targets[element.Item2];
                    final_path = path;
                }
            }
        }
        if (atk_target == null)
        {
            // 공격가능 상대 없음
            Skill();
        }
        else
        {
            if (final_path != null)
            {
                move(final_path);
                Invoke("Warning", (final_path.Count + 1) * 0.2f);
            }
            else
                Invoke("Warning", 0.5f);
        }

        if (!turn_done) turn_done = true;
    }
    public override void Warning()
    {
        if (atk_target == null) return;
        atk_dir = atk_target.curpos - curpos;
        /// 이거를 타일로 표시하는거 말고 indicator ( 화살표? )로 표시할수 있도록 바꿔야할듯
        warning_pos = curpos + atk_dir;
        MapManager.mapManager.GetTilemap(2).SetTile(new Vector3Int(atk_target.curpos.x, atk_target.curpos.y, 0), MapManager.mapManager.GetTile(0, 6));
    }
    public override void Skill() { Debug.Log("artskill"); }

    public virtual void attack()
    {
        MapManager.Push4direction(warning_pos, atk_dir);
        MapManager.mapManager.GetTilemap(2).SetTile(new Vector3Int(warning_pos.x, warning_pos.y, 0), null);
        if (!turn_done) turn_done = true;
    }
}

