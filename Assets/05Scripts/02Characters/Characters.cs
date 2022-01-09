using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;


public class Characters : MonoBehaviour
{

    //살아있는지, 이번턴 행동했는지 등등 flag
    public Character_status status = Character_status.waiting;
    public Character_stat stat;
    public Character_type character_Type;
    public Pos curpos = new Pos(2, 2);

    [SerializeField]
    private GameObject[] attack_arrows;

    public void move(List<Pos> path)
    {
        StartCoroutine(move_c(path));
    }
    IEnumerator move_c(List<Pos> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Debug.Log("path: " + path[i]);
            GetComponent<RectTransform>().anchoredPosition
                = Camera.main.WorldToScreenPoint(MapManager.mapManager.GetTilemap(0).GetCellCenterWorld(new Vector3Int(path[i].x, path[i].y, 0))) + Constants.character_tile_offset;
            yield return new WaitForSeconds(0.2f);
        }
        ChangeMapByte(curpos, path[path.Count - 1]);
        MoveButtons.nav_on = false;
        status = Character_status.waiting;
        if (character_Type == Character_type.player)
            StageManager.stageManager.stage = StageStatus.DEFAULT;
        else if (character_Type == Character_type.enemy)
        {
            GetComponent<Enemy>().Warning();
        }
        yield break;
    }
    private bool flip;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    public Animator GetAnimator()
    {
        return animator;
    }

    protected Character_Indicators_Controller indicators_Controller;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public virtual void init(Pos pos)
    {
        indicators_Controller.Init();
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        indicators_Controller = GetComponent<Character_Indicators_Controller>();
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
                ChangeMapByte(curpos, new Pos(nx, ny));
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

    public void ChangeMapByte(Pos current, Pos aftermove)
    {
        MapManager.mapManager.map[curpos.x, curpos.y] &= 0x3f00ffff;
        curpos = aftermove;
        MapManager.mapManager.map[curpos.x, curpos.y] |= 0x40000000;
    }


}
