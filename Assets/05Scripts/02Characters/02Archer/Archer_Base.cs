using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Base : Enemy
{

    public override void init(Pos pos)
    {
        //Debug.Log("archer init");
        targets.Add(StageManager.stageManager.GetPlayer());
        curpos = pos;
        ChangeMapByte();
        //Debug.Log(curpos.x + "," + curpos.y + ":" + MapManager.mapManager.map[curpos.x, curpos.y]);
    }
    public override void SetTarget()
    {
        //Debug.Log("arc settarget");
        atk_target = null;
        atk_dir = null;
        List<(Pos, int)> candid_pos = new List<(Pos, int)>();
        foreach (Characters target in targets)
        {
            Pos target_position = target.curpos;
            for (int index = 0; index < 4; index++)
            {
                for (int i = 1; i < 5; i++)
                {
                    int nx = target_position.x + i * Constants.dx[index];
                    int ny = target_position.y + i * Constants.dy[index];
                    if (nx == curpos.x && ny == curpos.y)
                    {
                        candid_pos.Add((new Pos(nx, ny), targets.IndexOf(target)));
                    }
                    else if (MapManager.checkWidthHeight(nx, ny) && !MapManager.checkCantGoTile(nx, ny, stat.moveType == moveType.ground))
                    {
                        candid_pos.Add((new Pos(nx, ny), targets.IndexOf(target)));
                    }
                }
            }
        }
        List<Pos> final_path = new List<Pos>();
        foreach ((Pos, int) element in candid_pos)
        {
            if (curpos.x == element.Item1.x && curpos.y == element.Item1.y)
            {
                //Debug.Log("arc " + element.Item1.x + "," + element.Item1.y + " already");
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
                if (atk_target == null)
                {
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
        warning_pos = new Pos(curpos.x + atk_dir.x, curpos.y + atk_dir.y);
    }
    public virtual void attack()
    {
        if (atk_dir == null) { turn_done = true; return; }
        if (atk_dir.x > 0)
        {
            Characters hitTarget = getTarget(1, 0);
            if (hitTarget != null) hitTarget.GetHit();
        }
        else if (atk_dir.x < 0)
        {
            Characters hitTarget = getTarget(-1, 0);
            if (hitTarget != null) hitTarget.GetHit();
        }
        else
        {
            if (atk_dir.y > 0)
            {
                Characters hitTarget = getTarget(0, 1);
                if (hitTarget != null) hitTarget.GetHit();
            }
            else
            {
                Characters hitTarget = getTarget(0, -1);
                if (hitTarget != null) hitTarget.GetHit();
            }
        }
        if (!turn_done) turn_done = true;
    }
    public override void Skill() { Debug.Log("askill"); }


    protected Characters getTarget(int x, int y)
    {
        // 방향받아서 순차적으로 탐색
        // 처음맞는 개체 return
        for (int i = 1; i < Constants.mapHeight; i++)
        {
            int nx = x * i + curpos.x;
            int ny = y * i + curpos.y;
            // 돌 부딛히는 경우 예외처리
            if (MapManager.groundInfo(nx, ny) == (int)tileType.rock) break;
            Characters target = StageManager.stageManager.GetCharacterByVector3Int(new Vector3Int(nx, ny, 0));
            if (target != null)
            {
                return target;
            }
        }
        return null;
    }
}
