using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tileType
{
    spore,
    grass,
    rock,
    water,
    baseTile,
    base_end,
    smoke,
    flame,
    end
}

public enum moveType
{
    ground,
    air,
    end
}

public enum CardType
{
    Attack_near,
    Attack_direct,
    Attack_indirect,
    Skill,
    Magic,
    Debuff,
    end
}

public enum Character_status
{
    waiting,
    attacking,
    hit
}

public enum Character_type
{
    player,
    enemy,
    fairy
}

static class Constants
{
    public const int mapWidth = 5;
    public const int mapHeight = 5;

    public const int totalCount = mapHeight * mapWidth;

    public static int[] dx = { -1, 1, 0, 0 };
    public static int[] dy = { 0, 0, -1, 1 };

    public static Vector3 character_tile_offset { get { return new Vector3(0, 0.55f, 0); } }
}

public enum phase
{
    map_gimmick,
    card_draw,
    enemy_move,
    player_turn,
    enemy_atk,
    end
}


[System.Serializable]
public class Array2d<T> where T : class
{
    public T[] data;
}