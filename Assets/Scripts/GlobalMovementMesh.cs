﻿using System.Collections.Generic;
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
    public void RemoveNode(MovementMeshNode n)
    {
        _ = allNodes.Remove(n);
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

    private float Heuristic(MovementMeshNode s, MovementMeshNode f)
    {
        return Vector2.Distance(s.GetPosition(), f.GetPosition());
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
        Dictionary<MovementMeshNode, float> costSoFar = new Dictionary<MovementMeshNode, float>() { { start, 0 } };
        Dictionary<MovementMeshNode, float> expectedCost = new Dictionary<MovementMeshNode, float>() { { start, Heuristic(start, goal) } };

        while (openSet.Count > 0)
        {
            MovementMeshNode current = openSet.Max;
            if (current == goal)
            {
                return RebuildPath(cameFrom, current);
            }
            _ = openSet.Remove(current);
            float next_score = costSoFar[current] + 1;
            foreach (MovementMeshNode n in current.GetConnected())
            {
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
}

public class BestNextNode : Comparer<MovementMeshNode>
{
    private Vector2 d;

    public BestNextNode(MovementMeshNode node)
    {
        d = node.GetPosition();
    }

    public override int Compare(MovementMeshNode x, MovementMeshNode y)
    {
        if (x == y)
        {
            return 0;
        }
        float distanceX = Vector2.Distance(d, x.GetPosition());
        float distanceY = Vector2.Distance(d, y.GetPosition());
        return (distanceX > distanceY) ? -1 : 1;
    }
}