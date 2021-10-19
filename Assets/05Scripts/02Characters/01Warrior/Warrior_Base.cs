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
        Debug.Log(curpos.x + "," + curpos.y + ":" + MapManager.mapManager.map[curpos.x, curpos.y]);
    }
    public override void SetTarget()
    {
        Debug.Log("war settarget");
        int distance = 100;
        atk_target = null;
        atk_dir = null;
        List<(Pos, int)> candid_pos = new List<(Pos, int)>();
        foreach (Characters target in targets)
        {
            Pos target_position = target.curpos;
            for (int i = 0; i < 4; i++)
            {
                Pos new_pos = new Pos(target_position.x + Constants.dx[i], target_position.x + Constants.dy[i]);
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
        Pos final_pos;
        List<Pos> final_path = new List<Pos>();
        foreach ((Pos, int) element in candid_pos)
        {
            Debug.Log("Element: " + element.Item1 + ", Curpos: " + curpos);
            if (curpos.x == element.Item1.x && curpos.y == element.Item1.y)
            {
                Debug.Log("war " + element.Item1.x + "," + element.Item1.y + " already");
                final_pos = curpos;
                final_path = null;
                atk_target = targets[element.Item2];
                break;
            }
            List<Pos> path = MapManager.getPath(curpos.x, curpos.y, element.Item1.x, element.Item1.y, stat.moveType == moveType.ground);
            if (path == null) { Debug.Log("null path"); continue; }
            if (path.Count <= stat.moverange + 1 && path.Count < distance)
            {
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
        Debug.Log("/////////////////////////////");
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

    public void attack()
    {
        // 공격 방향 ( 상대위치로 ) 기억해야할듯
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
    public override void Skill() { }

    public override void Warning()
    {
        if (atk_target == null) return;
        atk_dir = atk_target.curpos - curpos;
        /// 이거를 타일로 표시하는거 말고 indicator ( 화살표? )로 표시할수 있도록 바꿔야할듯
        warning_pos = new Pos(curpos.x + atk_dir.x, curpos.y + atk_dir.y);
        MapManager.mapManager.GetTilemap(3).SetTile(new Vector3Int(curpos.x + atk_dir.x, curpos.y + atk_dir.y, 0), MapManager.mapManager.GetTile(0, 7));
    }
}
