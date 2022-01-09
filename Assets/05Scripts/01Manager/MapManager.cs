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
    private Tilemap[] tilemaps;// base_tile, nav_tile, warning_tile, env_back_tile, env_front_tile, path_navigation_tile,Direct_X,Direct_Y;
    public Tilemap GetTilemap(int idx)
    {
        return tilemaps[idx];
    }
    [SerializeField]
    private Array2d<TileBase>[] map_tiles;
    // 0-포자 풀 돌 물 base1 navigation warning enemy_coming
    // 1-smoke front  8
    // 2-smoke back   8 
    // 3-fire front   8
    // 4-fire back    8
    // 5-move Lines   :  start 4(2 5 7 11) -> 6(25, 27, 57, 112, 511, 711) 
    // 6-melee arrows : 1 5 7 11
    // 7-direct arrows: x, y
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
    public int[,] map { get; set; }

    public static int groundInfo(int x, int y)
    {
        return mapManager.map[x, y] & 0x0000ffff;
    }
    public static int groundInfo(Pos pos)
    {
        return mapManager.map[pos.x, pos.y] & 0x0000ffff;
    }

    public Dictionary<(int, int), IEnumerator> tileanims = new Dictionary<(int, int), IEnumerator>();
    public static IEnumerator returnTileAnims(int x, int y)
    {
        IEnumerator result;
        mapManager.tileanims.TryGetValue((x, y), out result);
        return result;
    }

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
                    tilemaps[0].SetTile(pos, map_tiles[0].data[4]);
                    if (map[i, j] != (int)tileType.baseTile)
                        tilemaps[0].SetTile(pos, map_tiles[0].data[map[i, j]]);
                }
                else
                {
                    //물타일
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

    public void update_tileanims(int pos_x, int pos_y, int version)
    {
        // version 0: fire 1: smoke
        IEnumerator effect_coroutine = effect(pos_x, pos_y, version);
        tileanims.Add((pos_x, pos_y), effect_coroutine);
        StartCoroutine(effect_coroutine);
    }

    public void init()
    {
        map = new int[Constants.mapWidth, Constants.mapHeight];
        MapGenerator.makeMap(map);
        drawBases(map);
        for (int i = 0; i < Constants.mapHeight; i++)
        {
            for (int j = 0; j < Constants.mapHeight; j++)
            {
                if (map[i, j] == (int)tileType.water) map[i, j] = 1 << 6;
                if (map[i, j] == (int)tileType.rock) map[i, j] = 1 << 7;
                //Debug.Log(i + "," + j + ":" + map[i, j]);
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
                    if (isRockTile(xx, yy)) continue;
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
        if (startx == endx && starty == endy) return new List<Pos>();
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
    public void clearNavTiles(int index)
    {
        for (int i = 0; i < Constants.mapHeight; i++)
        {
            for (int j = 0; j < Constants.mapWidth; j++)
            {
                tilemaps[index].SetTile(new Vector3Int(i, j, 0), null);
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
        if (isWaterTile(x, y)) return groundUnit;
        if ((isRockTile(x, y))) return true;
        return false;
        // if (!isEmptyTile(x, y))
        // {
        //     return true;
        // }
        // if (!isPassibleTile(x, y))
        // {
        //     return groundUnit;
        // }
        // return false;
    }
    public static bool checkCantGoTile(Pos p, bool groundUnit)
    {
        if (!isEmptyTile(p.x, p.y))
        {
            return true;
        }
        if (isWaterTile(p.x, p.y)) return groundUnit;
        if ((isRockTile(p.x, p.y))) return true;
        return false;
        // if (!isEmptyTile(p.x, p.y))
        // {
        //     return true;
        // }
        // if (!isPassibleTile(p.x, p.y))
        // {
        //     return groundUnit;
        // }
        // return false;
    }

    public static bool isRockTile(int x, int y)
    {
        if ((mapManager.map[x, y] & 1 << 7) == 1 << 7) return true;
        return false;
    }
    public static bool isWaterTile(int x, int y)
    {
        if ((mapManager.map[x, y] & 1 << 6) == 1 << 6) return true;
        return false;
    }

    public static bool isEmptyTile(int x, int y)
    {
        // 비어있는지 return
        if ((mapManager.map[x, y] & 0x40000000) > 0) { return false; }
        return true;
    }

    public static bool isEmptyTile(Pos pos)
    {
        if ((mapManager.map[pos.x, pos.y] & 0x40000000) > 0) { return false; }
        return true;
    }


    public static bool isPassibleTile(int x, int y)
    {
        if (groundInfo(x, y) == (int)tileType.rock || groundInfo(x, y) == (int)tileType.water)
            return false;
        return true;
    }

    public static void Push4direction(Pos origin, Pos dir)
    {
        //Debug.Log("origin : " + origin);
        for (int i = 0; i < 4; i++)
        {
            int nx = origin.x + Constants.dx[i];
            int ny = origin.y + Constants.dy[i];
            //Debug.Log(nx + " " + ny);
            if (!checkWidthHeight(nx, ny)) continue;
            if (!isEmptyTile(nx, ny))
            {
                Characters target = StageManager.stageManager.GetCharacterByVector3Int(new Vector3Int(nx, ny, 0));
                //Debug.Log("try push " + target.name);
                target.Pushedto(new Pos(Constants.dx[i], Constants.dy[i]));
            }
        }
    }

    public static void Push(Pos from, Pos dir)
    {
        Pos to = from + dir;
        if (!checkWidthHeight(to))
        {
            //Debug.Log("pushed to out of boundary");
            return;
        }
        if (!isEmptyTile(to))
        {
            //Debug.Log("pushed to not empty tile");
            Characters target = StageManager.stageManager.GetCharacterByVector3Int(new Vector3Int(to.x, to.y, 0));
            target.Pushedto(dir);
        }
    }

    public static void DrawPath(List<Pos> path)
    {
        int count = path.Count;
        Tilemap path_tilemap = MapManager.mapManager.GetTilemap(5);
        // 경로 길이가 2개인경우랑 3이상인경우로 나눠야하나?
        if (path.Count == 2)
        {

        }
        else
        {
            // 0~1
            Pos dir = Pos.getDir(path[0], path[1]);

            if (dir.y == 0)
            {
                if (dir.x > 0)
                {
                    mapManager.tilemaps[5].SetTile(new Vector3Int(path[0].x, path[0].y, 0), mapManager.map_tiles[5].data[0]);
                }
                else if (dir.x < 0)
                {
                    mapManager.tilemaps[5].SetTile(new Vector3Int(path[0].x, path[0].y, 0), mapManager.map_tiles[5].data[2]);
                }
            }
            else if (dir.y > 0)
            {
                mapManager.tilemaps[5].SetTile(new Vector3Int(path[0].x, path[0].y, 0), mapManager.map_tiles[5].data[3]);
            }
            else
            {

                mapManager.tilemaps[5].SetTile(new Vector3Int(path[0].x, path[0].y, 0), mapManager.map_tiles[5].data[1]);
            }
            // 1~끝-1
            int multiply_result;
            for (int i = 1; i < count - 1; i++)
            {
                multiply_result = Pos.getDir2(path[i], path[i - 1]) * Pos.getDir2(path[i], path[i + 1]);
                switch (multiply_result)
                {
                    case 6:
                        mapManager.tilemaps[5].SetTile(new Vector3Int(path[i].x, path[i].y, 0), mapManager.map_tiles[5].data[5]);
                        break;
                    case 10:
                        mapManager.tilemaps[5].SetTile(new Vector3Int(path[i].x, path[i].y, 0), mapManager.map_tiles[5].data[9]);
                        break;
                    case 14:
                        mapManager.tilemaps[5].SetTile(new Vector3Int(path[i].x, path[i].y, 0), mapManager.map_tiles[5].data[6]);
                        break;
                    case 15:
                        mapManager.tilemaps[5].SetTile(new Vector3Int(path[i].x, path[i].y, 0), mapManager.map_tiles[5].data[7]);
                        break;
                    case 21:
                        mapManager.tilemaps[5].SetTile(new Vector3Int(path[i].x, path[i].y, 0), mapManager.map_tiles[5].data[4]);
                        break;
                    case 35:
                        mapManager.tilemaps[5].SetTile(new Vector3Int(path[i].x, path[i].y, 0), mapManager.map_tiles[5].data[8]);
                        break;
                }
            }
            //끝-1~끝
            multiply_result = Pos.getDir2(path[count - 1], path[count - 2]);
            switch (multiply_result)
            {
                case 2:
                    mapManager.tilemaps[5].SetTile(new Vector3Int(path[count - 1].x, path[count - 1].y, 0), mapManager.map_tiles[5].data[2]);
                    break;
                case 3:
                    mapManager.tilemaps[5].SetTile(new Vector3Int(path[count - 1].x, path[count - 1].y, 0), mapManager.map_tiles[5].data[0]);
                    break;
                case 5:
                    mapManager.tilemaps[5].SetTile(new Vector3Int(path[count - 1].x, path[count - 1].y, 0), mapManager.map_tiles[5].data[3]);
                    break;
                case 7:
                    mapManager.tilemaps[5].SetTile(new Vector3Int(path[count - 1].x, path[count - 1].y, 0), mapManager.map_tiles[5].data[1]);
                    break;
            }

        }
    }


}
