using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class GridNodes : MonoBehaviour
{
    [SerializeField] private Vector2 _gridWorldSize;
    [SerializeField] private float _nodeRadius;
    [SerializeField] private LayerMask _unwalkableLayer;
    [SerializeField] private bool _drawGrid;
    private float _nodeDiameter;
    public int SizeX { get; private set; }
    public int SizeY { get; private set; }
    public Node[,] Grid { get; private set; }
    public  Vector2[]  Path { get; private set; }
    public Pathfinding Pathfinding { get; private set; }
    
    
    protected void Awake()
    {
        _nodeDiameter = _nodeRadius * 2;
        SizeX = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
        SizeY = Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);
        CreateGrid();
        Pathfinding = new Pathfinding(this);
        Pathfinding.OnPathfinded += SetPath;
    }
    
    private void SetPath( Vector2[]  obj)
    {
        Path = obj;
    }
    
    private void CreateGrid()
    {
        Grid = new Node[SizeX, SizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * _gridWorldSize.x / 2 -
                                  Vector2.up * _gridWorldSize.y / 2;
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                var worldPoint = worldBottomLeft + Vector2.right * (x * _nodeDiameter + _nodeRadius) +
                                 Vector2.up * (y * _nodeDiameter + _nodeRadius);
                //bool walkable = !Physics2D.OverlapCircle(worldPoint, _nodeRadius, _unwalkableLayer);
                bool walkable = !Physics2D.OverlapBox(worldPoint, new Vector2(_nodeRadius, _nodeRadius), 0,
                    _unwalkableLayer);
                Grid[x, y] = new Node(x, y, worldPoint, walkable);
            }
        }
    }
    public Node GetRandomNode(bool walkable = true)
    {
        while (true)
        {
            int x = Random.Range(0, SizeX );
            int y = Random.Range(0, SizeY );
            if (Grid[x, y].Walkable == walkable)
                return Grid[x, y];
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
        return Grid[x, y];
    }

    public List<Node> GetNeighbours(Node node,int radius=1)
    {
        var neighbours = new List<Node>();
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x == 0 && y == 0) continue;
                int neighboursX = node.X - x;
                int neighboursY = node.Y - y;
                if (neighboursX >= 0 && neighboursX < SizeX && neighboursY >= 0 && neighboursY < SizeY)
                    neighbours.Add(Grid[neighboursX, neighboursY]);
            }
        }
        return neighbours;
    }

    private void OnDrawGizmos()
    {
        if (_drawGrid)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, _gridWorldSize);
            if (Grid != null)
            {
                foreach (var node in Grid)
                {
                    Gizmos.color = node.Walkable ? Color.green : Color.red;
                    if(Path!=null)
                        if(Path.Contains(node.WorldPosition))
                            Gizmos.color=Color.blue;
                    var color = Gizmos.color;
                    color.a = 0.4f;
                    Gizmos.color = color;
                    Gizmos.DrawCube(node.WorldPosition, Vector3.one * (_nodeDiameter - .1f));
                }
            }
        }
    }
    
}