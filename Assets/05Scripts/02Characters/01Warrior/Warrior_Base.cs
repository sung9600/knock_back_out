using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_Base : Enemy
{
    public override void init(Pos pos)
    {
        Debug.Log("war init");
        targets.Add(StageManager.stageManager.GetPlayer());
        curpos = pos;
        ChangeMapByte();
        //Debug.Log(curpos.x + "," + curpos.y + ":" + MapManager.mapManager.map[curpos.x, curpos.y]);
    }
    public override void SetTarget()
    {
        Debug.Log("war settarget");
        atk_target = null;
        atk_dir = null;
        candid_pos.Clear();
        foreach (Characters target in targets)
        {
            Pos target_position = target.curpos;
            for (int i = 0; i < 4; i++)
            {
                Pos new_pos = new Pos(target_position.x + Constants.dx[i], target_position.x + Constants.dy[i]);
                if (new_pos.x == curpos.x && new_pos.y == curpos.y)
                {
                    candid_pos.Add((new_pos, targets.IndexOf(target)));
                    continue;
                }
                if (!MapManager.checkWidthHeight(new_pos.x, new_pos.y))
                {
                    continue;
                }
                if (MapManager.checkCantGoTile(new_pos.x, new_pos.y, stat.moveType == moveType.ground))
                {
                    continue;
                }
                candid_pos.Add((new_pos, targets.IndexOf(target)));
                Debug.Log("candid_pos add " + new_pos);
            }
        }
        List<Pos> final_path = new List<Pos>();
        foreach ((Pos, int) element in candid_pos)
        {
            if (curpos.x == element.Item1.x && curpos.y == element.Item1.y)
            {
                // 현재 위치에서 공격가능 예외처리
                Debug.Log("war " + element.Item1.x + "," + element.Item1.y + " already");
                final_path = null;
                atk_target = targets[element.Item2];
                break;
            }
            List<Pos> path = MapManager.getPath(curpos.x, curpos.y, element.Item1.x, element.Item1.y, stat.moveType == moveType.ground);
            if (path == null)
            {
                // 경로없는 경우
                Debug.Log("null path");
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
        MapManager.mapManager.GetTilemap(3).SetTile(new Vector3Int(curpos.x + atk_dir.x, curpos.y + atk_dir.y, 0), MapManager.mapManager.GetTile(0, 7));
    }

    public virtual void attack()
    {
        // 공격 방향 ( 상대위치로 ) 기억해야할듯
        if (atk_target == null) { turn_done = true; return; }
        status = Character_status.attacking;
        Pos target_position = curpos + atk_dir;
        if (!MapManager.checkWidthHeight(target_position)) return;
        // idle이 오른쪽 => 위공격, 왼공격시에만 flip
        if (target_position.x < 0 || target_position.y > 0) spriteRenderer.flipX = true;
        List<Characters> characters = StageManager.stageManager.GetCharactersList();
        foreach (Characters character in characters)
        {
            if (Pos.equals(character.curpos, target_position))
            {
                // 피격판정
                character.GetHit();

                Debug.Log(character.name + " hit!!");
                break;
            }
        }
        if (MapManager.mapManager.GetTilemap(3).GetTile(new Vector3Int(warning_pos.x, warning_pos.y, 0)) != null)
            MapManager.mapManager.GetTilemap(3).SetTile(new Vector3Int(warning_pos.x, warning_pos.y, 0), null);
        if (!turn_done) turn_done = true;

    }
    public override void Skill() { Debug.Log("wskill"); }

}
