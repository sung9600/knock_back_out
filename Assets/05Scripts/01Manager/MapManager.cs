using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class MapManager : MonoBehaviour
{
    private void Awake()
    {
        if (_mapManager == null)
            _mapManager = this;
        init();
    }
    private static MapManager _mapManager;
    public static MapManager mapManager
    {
        get
        {
            if (_mapManager == null)
                return null;
            return _mapManager;
        }
    }
    [SerializeField]
    private Tilemap[] tilemaps;// base_tile, floor_tile, nav_tile, warning_tile, env_back_tile, env_front_tile;
    public Tilemap GetTilemap(int idx)
    {
        return tilemaps[idx];
    }
    [SerializeField]
    private Array2d<TileBase>[] map_tiles;
    // 포자 풀 돌 물 base1 base2 navigation warning
    // smoke front  8
    // smoke back   8 
    // fire front   8
    // fire back    8
    public TileBase GetTile(int idx1, int idx2)
    {
        return map_tiles[idx1].data[idx2];
    }
    [SerializeField]
    private Grid grid;

    public Vector3 getGridPosition(Vector3Int position)
    {
        return grid.GetCellCenterWorld(position);
    }
    //public Array2d<TileBase> char_tiles;
    // characters
    // 좌하
    // 우상
    public int[,] map;

    public Dictionary<(int, int), IEnumerator> tileanims = new Dictionary<(int, int), IEnumerator>();

    public void drawBases(int[,] map)
    {
        Vector3Int pos;
        for (int i = 0; i < Constants.mapHeight; i++)
        {
            for (int j = 0; j < Constants.mapWidth; j++)
            {
                pos = new Vector3Int(i, j, 0);
                Vector3 gridpos = getGridPosition(pos);
                if (map[i, j] != (int)tileType.water)
                {
                    if (Random.Range(0, 1f) > 0.5f)
                        tilemaps[0].SetTile(pos, map_tiles[0].data[4]);
                    else
                        tilemaps[0].SetTile(pos, map_tiles[0].data[5]);
                    if (map[i, j] != (int)tileType.baseTile)
                        tilemaps[1].SetTile(pos, map_tiles[0].data[map[i, j]]);
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
            tilemaps[5].SetTile(new Vector3Int(x, y, 0), map_tiles[version * 2 + 1].data[idx]);
            tilemaps[4].SetTile(new Vector3Int(x, y, 0), map_tiles[version * 2 + 2].data[idx++]);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void update_tileanims(int pos_x, int pos_y, int version)
    {
        IEnumerator effect_coroutine = effect(pos_x, pos_y, version);
        tileanims.Add((pos_x, pos_y), effect_coroutine);
        StartCoroutine(effect_coroutine);
    }

    public void init()
    {
        map = new int[Constants.mapWidth, Constants.mapHeight];
        MapGenerator.makeMap(map);
        drawBases(map);
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Debug.Log(i + "," + j + ":" + tilemaps[0].GetCellCenterWorld(new Vector3Int(i, j, 0)));
            }
        }
    }

    #region navigation
    public static List<Pos> getPossiblePos(int nx, int ny, int range, moveType moveType)
    {
        List<Pos> result = new List<Pos>();
        //todo : change to var
        int[,] map = new int[Constants.mapHeight, Constants.mapWidth];
        for (int i = 0; i < Constants.mapHeight; i++)
        {
            for (int j = 0; j < Constants.mapWidth; j++)
            {
                map[i, j] = mapManager.map[i, j];
            }
        }
        if (moveType == moveType.ground)
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
                    if (checkCantGoTile(xx, yy, moveType == moveType.ground)) continue;
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
                    if (!isPassibleTile(i, j)) continue;
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
    public static List<Pos> getPath(int startx, int starty, int endx, int endy, bool groundUnit)
    {
        if (startx == endx && starty == endy) return null;
        int[,] map = mapManager.map;
        Node[,] map2d = new Node[Constants.mapHeight, Constants.mapWidth];

        for (int i = 0; i < Constants.mapHeight; i++)
        {
            for (int j = 0; j < Constants.mapWidth; j++)
            {
                Node node = new Node(i, j);
                node.gCost = 100000;
                node.calcFCost();
                node.grid = map[i, j];
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

                    if (!openList.Contains(n) && !checkCantGoTile(n.x, n.y, groundUnit))
                    {
                        openList.Add(n);
                    }
                }
            }
        }
        return null;
    }

    private static List<Node> GetNeighborList(Node currNode, Node[,] mapp)
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

    private static List<Node> CalcPath(Node a)
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

    private static int CalcDistance(Node a, Node b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
    private static Node GetLowestNode(List<Node> list)
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

    public static bool checkWidthHeight(Pos position)
    {
        return position.x >= 0 && position.x < Constants.mapHeight && position.y >= 0 && position.y < Constants.mapWidth;
    }

    public static bool checkCantGoTile(int x, int y, bool groundUnit)
    {
        if (!isEmptyTile(x, y))
        {
            return true;
        }
        if (!isPassibleTile(x, y))
        {
            return groundUnit;
        }
        return false;
    }

    //abc
    public static bool isEmptyTile(int x, int y)
    {
        // 비어있는지 return
        if ((mapManager.map[x, y] & 0x40000000) > 0) { return false; }
        return true;
    }

    public static bool isPassibleTile(int x, int y)
    {
        if (mapManager.map[x, y] == (int)tileType.rock || mapManager.map[x, y] == (int)tileType.water)
            return false;
        return true;
    }

    public static void Push4direction(Pos origin, Pos dir)
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Debug.Log(i + " " + j + " :" + mapManager.map[i, j]);
            }
        }
        Debug.Log("origin : " + origin);
        for (int i = 0; i < 4; i++)
        {
            int nx = origin.x + Constants.dx[i];
            int ny = origin.y + Constants.dy[i];
            Debug.Log(nx + " " + ny);
            if (!checkWidthHeight(nx, ny)) continue;
            if (!isEmptyTile(nx, ny))
            {
                Characters target = StageManager.stageManager.GetCharacterByVector3Int(new Vector3Int(nx, ny, 0));
                Debug.Log("try push " + target.name);
                target.Pushedto(new Pos(Constants.dx[i], Constants.dy[i]));
                // if (!checkCantGoTile(nx + Constants.dx[i], ny + Constants.dy[i], target.stat.moveType == moveType.ground))
                // {
                //     Debug.Log("push to " + new Pos(nx + Constants.dx[i], ny + Constants.dy[i]));
                //     Debug.Log(target.gameObject.name);
                //     target.curpos = new Pos(nx + Constants.dx[i], ny + Constants.dy[i]);
                //     target.transform.position = mapManager.tilemaps[0].GetCellCenterWorld(new Vector3Int(nx + Constants.dx[i], ny + Constants.dy[i], 0)) + Constants.character_tile_offset;
                //     target.GetHit();
                // }
                // else
                // {
                //     // 충돌 or 물타일 떨어짐
                // }
            }
        }
    }


}
