using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private bool _drawGizmos;
    private GridNodes _gridNodes;
    private Vector2[] _path;
    public event Action<Vector2[]> OnPathfinded; 
    private void Awake()
    {
        _gridNodes = GetComponent<GridNodes>();
    }

    public void FindPath(Vector2 startPos, Vector2 endPos)
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
                    _path = RetracePath(startNode, endNode);
                    OnPathfinded?.Invoke(_path);
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

    public void OnDrawGizmos()
    {
        if (_path != null && _drawGizmos)
        {
            for (int i = 1; i < _path.Length; i++)
            {
                Gizmos.DrawLine(_path[i-1],_path[i]);
            }
        }
    }
}