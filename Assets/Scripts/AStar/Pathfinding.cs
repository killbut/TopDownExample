using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    private GridNodes _gridNodes;
    public event Action<Vector2[]> OnPathfinded;

    public Pathfinding(GridNodes gridNodes)
    {
        _gridNodes = gridNodes;
    }

    public Vector2[] FindPath(Vector2 startPos, Vector2 endPos)
    {
        var startNode = _gridNodes.NodeFromWorldPoint(startPos);
        var endNode = _gridNodes.NodeFromWorldPoint(endPos);
        if (endNode.Walkable)
        {
            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                var node = openSet.First();
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                    {
                        if (openSet[i].hCost < node.hCost) 
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);
                if (node == endNode)
                {
                    var path = RetracePath(startNode, endNode);
                    OnPathfinded?.Invoke(path);
                    return path;
                }

                foreach (var neighbour in _gridNodes.GetNeighbours(node))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour)) 
                        continue;
                    var newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, endNode);
                        neighbour.Parent = node;
                        if (!openSet.Contains(neighbour)) 
                            openSet.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    private int GetDistance(Node nodeStart, Node nodeEnd)
    {
        int distX = Mathf.Abs(nodeStart.X - nodeEnd.X);
        int distY = Mathf.Abs(nodeStart.Y - nodeEnd.Y);
        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

    private Vector2[] RetracePath(Node startNode, Node endNode)
    {
        var path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        var waypoints = path.Select(x => x.WorldPosition).ToArray();
        Array.Reverse(waypoints);
        return waypoints;
    }

}