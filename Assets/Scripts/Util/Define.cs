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

public enum characterType
{
    player,
    enemy,
    end
}

public enum atkType
{
    direct,
    melee,
    indirect,
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
    Attack,
    Skill,
    Magic,
    Debuff,
    end
}

public enum Character_status
{
    waiting,
    moving,
    attacking
}

static class Constants
{
    public const int mapWidth = 5;
    public const int mapHeight = 5;

    public const int totalCount = mapHeight * mapWidth;

    public static int[] dx = { -1, 1, 0, 0 };
    public static int[] dy = { 0, 0, -1, 1 };


}

[System.Serializable]
public class Array2d<T> where T : class
{
    public T[] data;
}