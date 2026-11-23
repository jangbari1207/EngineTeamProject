using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePathfinder
{
    private class Node
    {
        public Vector3Int pos;
        public Vector3Int dir;     // 이 칸에 들어올 때 향한 방향
        public int g;          // 시작점에서 걸어온 칸 수
        public int h;          // 목표까지 남은 거리 (맨해튼)
        public Node parent;
        public int F { get { return g + h; } }
    }

    private Tilemap _wallMap;
    private int _maxSearch;

    public TilePathfinder(Tilemap wallMap, int maxSearch = 256)
    {
        _wallMap = wallMap;
        _maxSearch = maxSearch;
    }

    private bool IsWalkable(Vector3Int cell)
    {
        return !_wallMap.HasTile(cell);
    }

    private int Distance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private static Vector3Int Left(Vector3Int d) { return new Vector3Int(-d.y, d.x, 0); }
    private static Vector3Int Right(Vector3Int d) { return new Vector3Int(d.y, -d.x, 0); }
    private static Vector3Int Back(Vector3Int d) { return new Vector3Int(-d.x, -d.y, 0); }

    private Vector3Int[] GetOrder(Node node, Vector3Int playerFacing)
    {
        Vector3Int f = node.dir;

        if (node.g > 0)
            return new[] { f, Right(f), Left(f) };

        if (Random.value < 0.07f)                       // 7%  : 뒤 → 왼쪽 → 오른쪽 → 앞
            return new[] { Back(f), Left(f), Right(f), f };
        if (Random.value < 0.70f)                       // 약 65% : 앞 → 오른쪽 → 왼쪽 → 뒤
            return new[] { f, Right(f), Left(f), Back(f) };

        Vector3Int p = playerFacing;                    // 나머지 : 플레이어가 보는 방향 기준
        return new[] { p, Right(p), Left(p), Back(p) };
    }

    private Node Search(Vector3Int start, Vector3Int goal, Vector3Int facing, Vector3Int playerFacing, int maxSearch)
    {
        List<Node> open = new List<Node>();
        Dictionary<Vector3Int, Node> openLookup = new Dictionary<Vector3Int, Node>();
        HashSet<Vector3Int> closed = new HashSet<Vector3Int>();

        Node startNode = new Node { pos = start, dir = facing, g = 0, h = Distance(start, goal) };
        open.Add(startNode);
        openLookup[start] = startNode;

        Node best = startNode;

        for (int i = 0; i < maxSearch && open.Count > 0; i++)
        {
            Node cur = open[0];
            foreach (Node n in open)
                if (n.F < cur.F || (n.F == cur.F && n.h < cur.h)) cur = n;

            open.Remove(cur);
            openLookup.Remove(cur.pos);
            closed.Add(cur.pos);

            if (cur.h < best.h) best = cur;
            if (cur.pos == goal) { best = cur; break; }

            foreach (Vector3Int d in GetOrder(cur, playerFacing))
            {
                Vector3Int next = cur.pos + d;
                if (closed.Contains(next)) continue;
                if (!IsWalkable(next)) continue;

                int newG = cur.g + 1;
                if (openLookup.TryGetValue(next, out Node old))
                {
                    if (newG < old.g) { old.g = newG; old.parent = cur; }
                    continue;
                }

                Node node = new Node { pos = next, dir = d, g = newG, h = Distance(next, goal), parent = cur };
                open.Add(node);
                openLookup[next] = node;
            }
        }
        return best;
    }

    public Vector3Int GetNextCell(Vector3Int start, Vector3Int goal, Vector3Int facing, Vector3Int playerFacing)
    {
        Node step = Search(start, goal, facing, playerFacing, _maxSearch);
        if (step.parent == null) return start;
        while (step.parent.parent != null)
            step = step.parent;
        return step.pos;
    }

    public int GetPathCost(Vector3Int start, Vector3Int goal)
    {
        Node best = Search(start, goal, Vector3Int.down, Vector3Int.down, 2000);
        return best.g + best.h;
    }
}

