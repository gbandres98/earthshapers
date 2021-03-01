using System.Collections.Generic;
using UnityEngine;

public class GlobalMovementMesh : MonoBehaviour
{
    public static GlobalMovementMesh Instance;
    private readonly List<MovementMeshNode> allNodes = new List<MovementMeshNode>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddNode(MovementMeshNode n)
    {
        allNodes.Add(n);
    }
    public MovementMeshNode GetNode(int i)
    {
        return (i >= allNodes.Count) ? null : allNodes[i];
    }

    public MovementMeshNode FindClosestNode(Vector2 p)
    {
        MovementMeshNode result = null;
        float minDistance = -1;
        foreach (MovementMeshNode n in allNodes)
        {
            float distance = Vector2.Distance(n.GetPosition() + Vector2.up, p);
            if (minDistance == -1 || distance < minDistance)
            {
                minDistance = distance;
                result = n;
            }
        }
        return result;
    }

    private int Heuristic(MovementMeshNode s, MovementMeshNode f)
    {
        // TODO
        return 1;
    }

    private List<MovementMeshNode> RebuildPath(Dictionary<MovementMeshNode, MovementMeshNode> cameFrom, MovementMeshNode last)
    {
        List<MovementMeshNode> path = new List<MovementMeshNode>() { last };
        MovementMeshNode current = last;
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    public List<MovementMeshNode> GetRoute(MovementMeshNode start, MovementMeshNode goal)
    {
        if (start == null || goal == null)
        {
            return null;
        }
        SortedSet<MovementMeshNode> openSet = new SortedSet<MovementMeshNode>(new BestNextNode(goal)) { start };
        Dictionary<MovementMeshNode, MovementMeshNode> cameFrom = new Dictionary<MovementMeshNode, MovementMeshNode>();
        Dictionary<MovementMeshNode, int> costSoFar = new Dictionary<MovementMeshNode, int>() { { start, 0 } };
        Dictionary<MovementMeshNode, int> expectedCost = new Dictionary<MovementMeshNode, int>() { { start, Heuristic(start, goal) } };

        while (openSet.Count > 0)
        {
            MovementMeshNode current = openSet.Max;
            if (current == goal)
            {
                return RebuildPath(cameFrom, current);
            }
            _ = openSet.Remove(current);
            foreach (MovementMeshNode n in current.GetConnected())
            {
                int next_score = costSoFar[current] + 1;
                if (!costSoFar.ContainsKey(n) || (next_score < costSoFar[n]))
                {
                    cameFrom[n] = current;
                    costSoFar[n] = next_score;
                    expectedCost[n] = costSoFar[n] + Heuristic(n, goal);
                    if (!openSet.Contains(n))
                    {
                        _ = openSet.Add(n);
                    }
                }
            }
        }
        return null;
    }

    private void Update()
    {
    }
}

public class BestNextNode : IComparer<MovementMeshNode>
{
    private Vector2 d;

    public BestNextNode(MovementMeshNode node)
    {
        d = node.GetPosition();
    }

    int IComparer<MovementMeshNode>.Compare(MovementMeshNode x, MovementMeshNode y)
    {
        if (x == y)
        {
            return 0;
        }
        float distanceX = Vector2.Distance(d, x.GetPosition());
        float distanceY = Vector2.Distance(d, y.GetPosition());
        return (distanceX > distanceY) ? 1 : -1;
    }
}