using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Base : Enemy
{

    public override void SetTarget()
    {
        //Debug.Log("arc settarget");
        atk_target = null;
        atk_dir = null;
        List<(Pos, int, int)> candid_pos = new List<(Pos, int, int)>();
        foreach (Characters target in targets)
        {
            Pos target_position = target.curpos;
            for (int index = 0; index < 4; index++)
            {
                for (int i = 1; i < Constants.mapHeight; i++)
                {
                    int nx = target_position.x + i * Constants.dx[index];
                    int ny = target_position.y + i * Constants.dy[index];
                    if (!MapManager.checkWidthHeight(nx, ny)) break;
                    if ((MapManager.groundInfo(nx, ny) & 1 << 6) == 1) continue;
                    if (MapManager.checkCantGoTile(nx, ny, stat.moveType == moveType.ground)) break;
                    candid_pos.Add((new Pos(nx, ny), targets.IndexOf(target), i));
                }
            }
        }
        List<Pos> final_path = new List<Pos>();
        int cur = -1;
        foreach ((Pos, int, int) element in candid_pos)
        {
            Debug.Log(element.Item1);
            if (curpos.x == element.Item1.x && curpos.y == element.Item1.y)
            {
                Debug.Log("break");
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
                    cur = element.Item3;
                    atk_target = targets[element.Item2];
                    final_path = path;
                }
                else if (element.Item3 > cur)
                {
                    cur = element.Item3;
                    atk_target = targets[element.Item2];
                    final_path = path;
                }
                else if (element.Item3 == cur)
                {

                    if (Random.Range(0, 10) < 5)
                    {
                        cur = element.Item3;
                        atk_target = targets[element.Item2];
                        final_path = path;
                    }
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
            atk_dir = atk_target.curpos - curpos;
            if (final_path != null)
            {
                move(final_path);
            }
            else
            {
                Warning();
            }
            StageManager.stageManager.attackList.attackList.Add(new AttackCommand(atk_target.curpos, curpos, this));
        }

        if (!turn_done) turn_done = true;
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
