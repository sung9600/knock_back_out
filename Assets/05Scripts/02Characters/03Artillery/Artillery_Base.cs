using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery_Base : Enemy
{
    public override void init(Pos pos)
    {
        Debug.Log("art init");
        targets.Add(StageManager.stageManager.GetPlayer());
        curpos = pos;
        ChangeMapByte();
        Debug.Log(curpos.x + "," + curpos.y + ":" + MapManager.mapManager.map[curpos.x, curpos.y]);
    }
    public override void SetTarget()
    {
        Debug.Log("art settarget");
        int distance = 100;
        atk_target = null;
        atk_dir = null;
        status = Character_status.waiting;
        List<(Pos, int)> candid_pos = new List<(Pos, int)>();
        foreach (Characters target in targets)
        {
            Pos target_position = target.curpos;
            for (int index = 0; index < 4; index++)
            {
                int i = 5;
                while (true)
                {
                    if (i == 1) break;
                    int nx = target_position.x + i * Constants.dx[index];
                    int ny = target_position.y + i * Constants.dy[index];
                    if (MapManager.checkWidthHeight(nx, ny) && !MapManager.checkCantGoTile(nx, ny, stat.moveType == moveType.ground))
                    {
                        candid_pos.Add((new Pos(nx, ny), targets.IndexOf(target)));
                        break;
                    }
                    i--;
                }
            }
        }

        Pos final_pos;
        List<Pos> final_path = new List<Pos>();
        foreach ((Pos, int) element in candid_pos)
        {
            if (curpos.x == element.Item1.x && curpos.y == element.Item1.y)
            {
                Debug.Log("art " + element.Item1.x + "," + element.Item1.y + " already");
                status = Character_status.attacking;
                final_pos = curpos;
                final_path = null;
                atk_target = targets[element.Item2];
                break;
            }
            List<Pos> path = MapManager.getPath(curpos.x, curpos.y, element.Item1.x, element.Item1.y, stat.moveType == moveType.ground);
            if (path.Count <= stat.moverange + 1 && path.Count < distance)
            {
                status = Character_status.attacking;
                distance = path.Count;
                final_pos = element.Item1;
                atk_target = targets[element.Item2];
                final_path = path;
            }
            else if (path.Count == distance)
            {
                if (element.Item2 < targets.IndexOf(atk_target))
                {
                    // 현재 iteration의 우선순위가 더 높은경우
                    status = Character_status.attacking;
                    distance = path.Count;
                    final_pos = element.Item1;
                    atk_target = targets[element.Item2];
                    final_path = path;
                }
                else if (element.Item2 == targets.IndexOf(atk_target))
                {
                    // 거리 같고, 우선순위까지 같은경우
                    // 둘중에 랜덤으로 정합시다
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
            }
            Invoke("Warning", 0.5f);
        }

        if (!turn_done) turn_done = true;
    }
    public override void Warning()
    {
        if (atk_target == null) return;
        //atk_dir = atk_target.curpos - curpos;
        /// 이거를 타일로 표시하는거 말고 indicator ( 화살표? )로 표시할수 있도록 바꿔야할듯
        //warning_pos = new Pos(curpos.x + atk_dir.x, curpos.y + atk_dir.y);
        MapManager.mapManager.GetTilemap(3).SetTile(new Vector3Int(atk_target.curpos.x, atk_target.curpos.y, 0), MapManager.mapManager.GetTile(0, 7));
    }
    public override void Skill()
    {

    }

    public void attack()
    {
        if (status == Character_status.waiting)
        {
            turn_done = true;
            return;
        }
        MapManager.Push4direction(warning_pos, atk_dir);
        MapManager.mapManager.GetTilemap(3).SetTile(new Vector3Int(warning_pos.x, warning_pos.y, 0), null);
        if (!turn_done) turn_done = true;
    }
}

