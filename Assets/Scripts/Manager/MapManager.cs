using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class MapManager : MonoBehaviour
{
    public Tilemap[] tilemaps;// base_tile, floor_tile, nav_tile, env_back_tile, env_front_tile;
    [SerializeField]
    private Array2d<TileBase>[] map_tiles;
    // 포자 풀 돌 물 base1 base2
    // smoke front  8
    // smoke back   8 
    // fire front   8
    // fire back    8
    public Grid grid;
    //public Array2d<TileBase> char_tiles;
    // characters
    // 좌하
    // 우상
    public TileBase nav_tile;
    public int[] map;

    public Dictionary<(int, int), IEnumerator> tileanims = new Dictionary<(int, int), IEnumerator>();

    public void drawBases(int[] map)
    {
        Vector3Int pos;
        for (int i = 0; i < Constants.mapHeight; i++)
        {
            for (int j = 0; j < Constants.mapWidth; j++)
            {
                pos = new Vector3Int(i, j, 0);
                Vector3 gridpos = grid.GetCellCenterWorld(pos);
                int idx = i * Constants.mapWidth + j;
                if (map[idx] != (int)tileType.water)
                {
                    if (Random.Range(0, 1f) > 0.5f)
                        tilemaps[0].SetTile(pos, map_tiles[0].data[4]);
                    else
                        tilemaps[0].SetTile(pos, map_tiles[0].data[5]);
                    if (map[idx] != (int)tileType.baseTile)
                        tilemaps[1].SetTile(pos, map_tiles[0].data[map[idx]]);
                }
                else
                {
                    tilemaps[0].SetTile(pos, map_tiles[0].data[3]);
                }
            }
        }
    }

    IEnumerator effect(int x, int y, int version)
    {
        yield return null;
        int idx = 0;
        while (true)
        {
            idx %= map_tiles[2].data.Length;
            tilemaps[4].SetTile(new Vector3Int(x, y, 0), map_tiles[version * 2 + 1].data[idx]);
            tilemaps[3].SetTile(new Vector3Int(x, y, 0), map_tiles[version * 2 + 2].data[idx++]);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void update_tileanims(int pos, int version)
    {
        int x = pos / 5;
        int y = pos % 5;
        IEnumerator effect_coroutine = effect(x, y, version);
        tileanims.Add((x, y), effect_coroutine);
        StartCoroutine(effect_coroutine);
    }

    public void init()
    {
        map = new int[Constants.totalCount];
        MapGenerator.makeMap(map);
        drawBases(map);
        //update_tileanims(10, 0);
        //update_tileanims(20, 1);
        // characters_tile.SetTile(new Vector3Int(2, 0, 0), char_tiles[0].arr[0]);
        // characters_tile.SetTile(new Vector3Int(4, 0, 0), char_tiles[1].arr[0]);
    }

    #region navigation
    public static List<Pos> getPossiblePos(int nx, int ny)
    {
        List<Pos> result = new List<Pos>();
        //todo : change to var
        uint range = 3;
        int[,] map = new int[Constants.mapHeight, Constants.mapWidth];
        for (int i = 0; i < Constants.mapHeight; i++)
        {
            for (int j = 0; j < Constants.mapWidth; j++)
            {
                map[i, j] = StageManager.stageManager.mapManager.map[i * Constants.mapHeight + j];
            }
        }
        if (StageManager.stageManager.player.character_Stat.moveType == moveType.ground)
        {
            int[,] dist = new int[Constants.mapHeight, Constants.mapWidth];
            for (int i = 0; i < Constants.mapHeight; i++)
            {
                for (int j = 0; j < Constants.mapWidth; j++)
                {
                    dist[i, j] = int.MaxValue;
                }
            }
            dist[nx, ny] = 0;
            Queue<(int, int, int)> q = new Queue<(int, int, int)>();
            q.Enqueue((nx, ny, 0));
            while (q.Count != 0)
            {
                (int, int, int) top = q.Dequeue();
                int x = top.Item1;
                int y = top.Item2;
                int val = top.Item3;

                for (int i = 0; i < 4; i++)
                {
                    int xx = x + Constants.dx[i];
                    int yy = y + Constants.dy[i];
                    if (!checkWidthHeight(xx, yy)) continue;
                    if (map[xx, yy] == (int)tileType.water || map[xx, yy] == (int)tileType.rock) continue;
                    if (dist[xx, yy] > val + 1)
                    {
                        dist[xx, yy] = val + 1;
                        q.Enqueue((xx, yy, val + 1));
                    }
                }
            }
            for (int i = 0; i < Constants.mapHeight; i++)
            {
                for (int j = 0; j < Constants.mapWidth; j++)
                {
                    if (dist[i, j] <= range)
                        result.Add(new Pos(i, j));
                }
            }

            return result;
        }
        else
        {
            for (int i = 0; i < Constants.mapHeight; i++)
            {
                for (int j = 0; j < Constants.mapWidth; j++)
                {
                    if (StageManager.stageManager.mapManager.map[i * Constants.mapHeight + j] == (int)tileType.water ||
                    StageManager.stageManager.mapManager.map[i * Constants.mapHeight + j] == (int)tileType.rock) continue;
                    int dx = nx - i;
                    int dy = ny - j;
                    if (Mathf.Abs(dx) + Mathf.Abs(dy) <= range)
                    {
                        result.Add(new Pos(i, j));
                    }
                }
            }
            return result;
        }
    }


    private class Node
    {
        public int x, y;
        public int gCost, hCost, fCost;
        public int grid;
        public Node cameFrom;
        public Node(int x, int y) { this.x = x; this.y = y; }
        public void calcFCost() { fCost = gCost + hCost; }
    }
    public List<Pos> getPath(int startx, int starty, int endx, int endy)
    {
        if (startx == endx && starty == endy) return null;
        int[] map = StageManager.stageManager.mapManager.map;
        Node[,] map2d = new Node[Constants.mapHeight, Constants.mapWidth];

        for (int i = 0; i < Constants.mapHeight; i++)
        {
            for (int j = 0; j < Constants.mapWidth; j++)
            {
                Node node = new Node(i, j);
                node.gCost = 100000;
                node.calcFCost();
                node.grid = map[Constants.mapWidth * i + j];
                node.cameFrom = null;
                map2d[i, j] = node;
            }
        }
        map2d[startx, starty].gCost = 0;
        map2d[startx, starty].hCost = CalcDistance(map2d[startx, starty], map2d[endx, endy]);
        map2d[startx, starty].calcFCost();

        List<Node> openList = new List<Node>() { map2d[startx, starty] };
        List<Node> closeList = new List<Node>();

        while (openList.Count > 0)
        {
            Node currNode = GetLowestNode(openList);
            if (currNode.x == endx && currNode.y == endy)
            {
                List<Node> path = CalcPath(map2d[endx, endy]);
                List<Pos> result = new List<Pos>();
                foreach (Node a in path)
                {
                    result.Add(new Pos(a.x, a.y));
                }
                return result;
            }

            openList.Remove(currNode);
            closeList.Add(currNode);
            List<Node> neighbors = GetNeighborList(currNode, map2d);
            foreach (Node n in neighbors)
            {
                if (closeList.Contains(n))
                    continue;
                int tGcost = currNode.gCost + CalcDistance(currNode, n);
                if (tGcost < n.gCost)
                {
                    n.cameFrom = currNode;
                    n.gCost = tGcost;
                    n.hCost = CalcDistance(n, map2d[endx, endy]);
                    n.calcFCost();

                    if (!openList.Contains(n) && map2d[n.x, n.y].grid != (int)tileType.rock && map2d[n.x, n.y].grid != (int)tileType.water)
                    {
                        // todo : 여기에 요정우리나 적이 있는지 조건도 추가해야함
                        // 지금은 지형지물만 고려했음
                        openList.Add(n);
                    }
                }
            }
        }
        return null;
    }

    private List<Node> GetNeighborList(Node currNode, Node[,] mapp)
    {
        List<Node> result = new List<Node>();
        for (int i = 0; i < 4; i++)
        {
            int nx = currNode.x + Constants.dx[i];
            int ny = currNode.y + Constants.dy[i];
            if (checkWidthHeight(nx, ny))
                result.Add(mapp[nx, ny]);
        }
        return result;
    }

    private List<Node> CalcPath(Node a)
    {
        List<Node> result = new List<Node>();
        result.Add(a);
        Node curr = a;
        while (curr.cameFrom != null)
        {
            result.Add(curr.cameFrom);
            curr = curr.cameFrom;
        }
        result.Reverse();
        return result;
    }

    private int CalcDistance(Node a, Node b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
    private Node GetLowestNode(List<Node> list)
    {
        Node top = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].fCost < top.fCost)
                top = list[i];
        }
        return top;
    }
    #endregion
    public void clearNavTiles()
    {
        for (int i = 0; i < Constants.mapHeight; i++)
        {
            for (int j = 0; j < Constants.mapWidth; j++)
            {
                tilemaps[2].SetTile(new Vector3Int(i, j, 0), null);
            }
        }
    }

    public static bool checkWidthHeight(int x, int y)
    {
        return x >= 0 && x < Constants.mapHeight && y >= 0 && y < Constants.mapWidth;
    }


}
