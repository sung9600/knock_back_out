using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    public static void makeMap(int[] map)
    {
        while (true)
        {
            for (int i = 0; i < Constants.totalCount; i++)
            {
                map[i] = -1;
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
                    int idx = x * Constants.mapWidth + y;
                    if (map[idx] != -1)
                    {
                        j++;
                        continue;
                    }
                    map[idx] = i;
                }
            }
            for (int i = 0; i < Constants.mapHeight; i++)
            {
                for (int j = 0; j < Constants.mapWidth; j++)
                {
                    int idx = i * Constants.mapWidth + j;
                    if (map[idx] == -1)
                        map[idx] = 4;
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

    static bool checkvalidmap(int[] map)
    {
        bool[] visit = new bool[Constants.totalCount];
        Queue<(int, int)> queue = new Queue<(int, int)>();
        visit[0] = true;
        queue.Enqueue((0, 0));
        while (queue.Count != 0)
        {
            var top = queue.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                int nx = Constants.dx[i] + top.Item1;
                int ny = Constants.dy[i] + top.Item2;
                int idx = nx * Constants.mapWidth + ny;
                // 범위초과
                if (nx < 0 || nx >= Constants.mapHeight || ny < 0 || ny >= Constants.mapWidth) continue;
                // 갈수없는타일
                if (map[idx] == (int)tileType.rock ||
                    map[idx] == (int)tileType.water) continue;
                // 갈수있고 visit안했으면 enqueue
                if (!visit[idx])
                {
                    visit[nx * Constants.mapWidth + ny] = true;
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
                if (map[idx] == (int)tileType.rock ||
                       map[idx] == (int)tileType.water) continue;
                // 갈수있는데 visit안됨 -> return false
                if (!visit[idx]) return false;
            }
        }
        return true;
    }
}
