using System.Collections;
using System.Collections.Generic;
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

    public static Pos getDir(Pos first, Pos second)
    {
        // 좌: 2
        // 우: 3
        // 상: 5
        // 하: 7
        // 아니면 11
        Pos dir = second - first;
        if (dir.x > 0)
        {
            return new Pos(1, 0);
        }
        else if (dir.x < 0)
        {
            return new Pos(-1, 0);
        }
        else
        {
            if (dir.y > 0)
            {
                return new Pos(0, 1);
            }
            else if (dir.y < 0)
            {
                return new Pos(0, -1);
            }
        }
        return new Pos(0, 0);
    }

    public static int getDir2(Pos first, Pos second)
    {

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
        return 11;
    }

}
