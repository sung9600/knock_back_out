using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    public static void makeMap(int[,] map)
    {
        while (true)
        {
            for (int i = 0; i < Constants.mapWidth; i++)
            {
                for (int j = 0; j < Constants.mapHeight; j++)
                    map[i, j] = -1;
            }
            int size = (int)tileType.baseTile;
            int[] initCount = new int[(int)tileType.baseTile] { 2, 5, 2, 3 }; // 포자 풀 돌 물
            for (int i = 0; i < size; i++)
            {
                int count = initCount[i];
                for (int j = count; j > 0; j--)
                {
                    int x = Random.Range(0, Constants.mapHeight);
                    int y = Random.Range(0, Constants.mapWidth);
                    if (map[x, y] != -1)
                    {
                        j++;
                        continue;
                    }
                    map[x, y] = i;
                }
            }
            for (int i = 0; i < Constants.mapHeight; i++)
            {
                for (int j = 0; j < Constants.mapWidth; j++)
                {
                    if (map[i, j] == -1)
                        map[i, j] = 4;
                }
            }
            if (checkvalidmap(map))
            {
                break;
            }
        }
        return;
    }
    public static void destroyMap()
    {
        return;
    }

    static bool checkvalidmap(int[,] map)
    {
        bool[,] visit = new bool[Constants.mapHeight, Constants.mapWidth];
        Queue<(int, int)> queue = new Queue<(int, int)>();
        visit[2, 2] = true;
        queue.Enqueue((0, 0));
        while (queue.Count != 0)
        {
            var top = queue.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                int nx = Constants.dx[i] + top.Item1;
                int ny = Constants.dy[i] + top.Item2;
                // 범위초과
                if (nx < 0 || nx >= Constants.mapHeight || ny < 0 || ny >= Constants.mapWidth) continue;
                // 갈수없는타일
                if (map[nx, ny] == (int)tileType.rock ||
                    map[nx, ny] == (int)tileType.water) continue;
                // 갈수있고 visit안했으면 enqueue
                if (!visit[nx, ny])
                {
                    visit[nx, ny] = true;
                    queue.Enqueue((nx, ny));
                }
            }
        }
        for (int i = 0; i < Constants.mapHeight; i++)
        {
            for (int j = 0; j < Constants.mapWidth; j++)
            {
                int idx = i * Constants.mapWidth + j;
                // 갈수없는타일 제외
                if (map[i, j] == (int)tileType.rock ||
                       map[i, j] == (int)tileType.water) continue;
                // 갈수있는데 visit안됨 -> return false
                if (!visit[i, j]) return false;
            }
        }
        return true;
    }
}
