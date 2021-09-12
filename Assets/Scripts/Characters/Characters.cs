using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

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
}
[System.Serializable]
public class Character_stat
{
    public uint hp;
    public uint maxhp;
    public uint shield;
    public uint moverange;
    public atkType atkType;
    public moveType moveType;

    public Character_stat()
    {
        this.hp = 0;
        this.maxhp = 0;
        this.shield = 0;
        this.moverange = 0;
        this.atkType = atkType.direct;
        this.moveType = moveType.ground;
    }
}
public class Characters : MonoBehaviour
{

    //살아있는지, 이번턴 행동했는지 등등 flag
    public Character_status status = Character_status.waiting;
    public Character_stat character_Stat;
    public Pos atkpos = new Pos(-1, -1);
    public Pos curpos = new Pos(0, 0);

    private bool left = true;
    private bool up;

    public void move(List<Pos> path)
    {
        StartCoroutine(move_c(path));
    }
    IEnumerator move_c(List<Pos> path)
    {
        status = Character_status.moving;
        for (int i = 0; i < path.Count; i++)
        {
            if (i != path.Count - 1)
            {
                changeDir(path[i], path[i + 1]);
            }
            transform.position = StageManager.stageManager.mapManager.tilemaps[0].GetCellCenterWorld(new Vector3Int(path[i].x, path[i].y, 0)) + new Vector3(0, 0.5f, 0);
            yield return new WaitForSeconds(0.2f);
        }
        MoveButtons.nav_on = false;
        status = Character_status.waiting;
        yield break;
    }
    private bool flip;
    private SpriteRenderer spriteRenderer;

    //down up
    [SerializeField]
    private Array2d<Sprite>[] idle_sprites;
    [SerializeField]
    private Array2d<Sprite>[] moving_sprites;
    [SerializeField]
    private Array2d<Sprite>[] attack_sprites;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        StartCoroutine("animate");
    }
    IEnumerator animate()
    {
        int i = 0;
        float idle_gap = 1f / idle_sprites[0].data.Length;
        float moving_gap = 1f / moving_sprites[0].data.Length;
        float attack_gap = 2f / attack_sprites[0].data.Length;
        while (true)
        {
            switch (status)
            {
                case Character_status.waiting:
                    spriteRenderer.sprite = idle_sprites[up ? 1 : 0].data[i++];
                    if (i >= idle_sprites[0].data.Length)
                        i = 0;
                    yield return new WaitForSeconds(idle_gap);
                    break;
                case Character_status.moving:
                    if (i >= moving_sprites[0].data.Length)
                        i = 0;
                    spriteRenderer.sprite = moving_sprites[up ? 1 : 0].data[i++];
                    yield return new WaitForSeconds(moving_gap);
                    break;
                case Character_status.attacking:
                    if (i >= attack_sprites[0].data.Length)
                    {
                        i = 0;
                    }
                    spriteRenderer.sprite = attack_sprites[up ? 1 : 0].data[i++];
                    if (i >= attack_sprites[0].data.Length)
                    {
                        i = 0;
                        status = Character_status.waiting;
                    }
                    yield return new WaitForSeconds(attack_gap);
                    break;
            }
        }
    }

    public void changeDir(Pos before, Pos after)
    {
        Pos nextDir = after - before;
        up = nextDir.x + nextDir.y > 0;
        if (nextDir.x < 0 || nextDir.x == 0 && nextDir.y > 0)
            left = true;
        else left = false;
        if (left == up) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }
}
