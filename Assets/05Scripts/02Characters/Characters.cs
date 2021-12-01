using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class Pos
{
    public int x;
    public int y;
    public Pos(int x1, int y1)
    {
        this.x = x1;
        this.y = y1;
    }
    public static Pos operator -(Pos first, Pos second)
    {
        return new Pos(first.x - second.x, first.y - second.y);
    }

    public static Pos operator +(Pos first, Pos second)
    {
        return new Pos(first.x + second.x, first.y + second.y);
    }

    public static Pos operator *(Pos pos, int i)
    {
        return new Pos(pos.x * i, pos.y * i);
    }
    public static Pos operator *(int i, Pos pos)
    {
        return new Pos(pos.x * i, pos.y * i);
    }

    public static bool equals(Pos first, Pos second)
    {
        return (first.x == second.x) && (first.y == second.y);
    }

    public override string ToString()
    {
        return x + ", " + y;
    }

    public static int getDir(Pos first, Pos second)
    {
        // 좌: 2
        // 우: 3
        // 상: 5
        // 하: 7
        // 아니면 11
        Pos dir = second - first;
        if (dir.x > 0)
        {
            return 3;
        }
        else if (dir.x < 0)
        {
            return 2;
        }
        else
        {
            if (dir.y > 0)
            {
                return 5;
            }
            else if (dir.y < 0)
            {
                return 7;
            }
        }
        return 5;
    }

}
[System.Serializable]
public class Character_stat
{
    public int hp;
    public int maxhp;
    public int shield;
    public int moverange;
    public moveType moveType;

    public Character_stat()
    {
        this.hp = 3;
        this.maxhp = 3;
        this.shield = 0;
        this.moverange = 3;
        this.moveType = moveType.ground;
    }
}
public class Characters : MonoBehaviour
{

    //살아있는지, 이번턴 행동했는지 등등 flag
    public Character_status status = Character_status.waiting;
    public Character_stat stat;
    public Character_type character_Type;
    public Pos curpos = new Pos(2, 2);

    public void move(List<Pos> path)
    {
        StartCoroutine(move_c(path));
    }
    IEnumerator move_c(List<Pos> path)
    {
        MapManager.mapManager.map[curpos.x, curpos.y] &= 0x3fffffff;
        for (int i = 0; i < path.Count; i++)
        {
            transform.position = MapManager.mapManager.GetTilemap(0).GetCellCenterWorld(new Vector3Int(path[i].x, path[i].y, 0)) + Constants.character_tile_offset;
            yield return new WaitForSeconds(0.2f);
        }
        curpos = path[path.Count - 1];
        ChangeMapByte();
        MoveButtons.nav_on = false;
        status = Character_status.waiting;
        if (gameObject.GetComponent<Player>() != null)
            StageManager.stageManager.stage = StageStatus.DEFAULT;
        yield break;
    }
    private bool flip;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    public Animator GetAnimator()
    {
        return animator;
    }

    public virtual void init(Pos pos) { }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void changeStatus()
    {
        status = Character_status.hit;
    }

    public virtual void GetHit()
    {
        status = Character_status.hit;
        animator.SetTrigger("Hit");
    }

    public virtual void Attack_animation()
    {
        //Debug.Log("attack");
        animator.SetTrigger("Attack");
    }

    public void Pushedto(Pos direction)
    {
        int nx = curpos.x + direction.x;
        int ny = curpos.y + direction.y;
        // 일단 밀고 밀리는 위치가 유효한지 판단할까?
        Vector3 targetPos = MapManager.mapManager.GetTilemap(0).GetCellCenterWorld(new Vector3Int(nx, ny, 0)) + Constants.character_tile_offset;
        transform.DOMove(targetPos, 0.5f, false)
        .OnComplete(() =>
        {

            if (!MapManager.checkCantGoTile(nx, ny, stat.moveType == moveType.ground))
            {
                MapManager.mapManager.map[curpos.x, curpos.y] &= 0x3fffffff;
                curpos.x = nx;
                curpos.y = ny;
                ChangeMapByte();
            }
            else
            {
                // 물타일 or 충돌
                targetPos = MapManager.mapManager.GetTilemap(0).GetCellCenterWorld(new Vector3Int(curpos.x, curpos.y, 0)) + Constants.character_tile_offset;
                transform.DOMove(targetPos, 0.2f, false);
            }
        });
    }


    public void ChangeMapByte()
    {
        MapManager.mapManager.map[curpos.x, curpos.y] |= 0x40000000;
    }


}
