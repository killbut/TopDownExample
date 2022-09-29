using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridNode : MonoBehaviour
{
    [SerializeField] private Vector2 _gridWorldSize;
    [SerializeField] private float _nodeRadius;
    [SerializeField] private LayerMask _unwalkableMask;

    private Node[,] _grid;
    private float _nodeDiameter;
    public int SizeX { get; private set; }
    public int SizeY { get; private set; }
    public List<Node> Path { get; set; }
    public Node[,] Grid => _grid;
    public static GridNode Instance { get; private set; }

    protected void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _nodeDiameter = _nodeRadius * 2;
        SizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
        SizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        _grid = new Node[SizeX, SizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * _gridWorldSize.x / 2 -
                                  Vector2.up * _gridWorldSize.y / 2;
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                var worldPoint = worldBottomLeft + Vector2.right * (x * _nodeDiameter + _nodeRadius) +
                                 Vector2.up * (y * _nodeDiameter + _nodeRadius);
                bool walkable = !Physics2D.OverlapCircle(worldPoint, _nodeRadius, _unwalkableMask);
                _grid[x, y] = new Node(x, y, worldPoint, walkable);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector2 worldPos)
    {
        var percentX = (worldPos.x - transform.position.x + _gridWorldSize.x / 2) / _gridWorldSize.x;
        var percentY = (worldPos.y - transform.position.y + _gridWorldSize.y / 2) / _gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.FloorToInt(Mathf.Clamp((SizeX) * percentX, 0, SizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp((SizeY) * percentY, 0, SizeY - 1));
        return _grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        var neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                int neighboursX = node.X - x;
                int neighboursY = node.Y - y;
                if (neighboursX >= 0 && neighboursX < SizeX && neighboursY >= 0 && neighboursY < SizeY)
                    neighbours.Add(_grid[neighboursX, neighboursY]);
            }
        }

        return neighbours;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, _gridWorldSize);
        if (_grid != null)
        {
            foreach (var node in _grid)
            {
                Gizmos.color = node.Walkable ? Color.green : Color.red;
                if (Path != null)
                    if (Path.Contains(node))

                        Gizmos.color = Color.blue;

                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - .1f));
            }
        }
    }
}