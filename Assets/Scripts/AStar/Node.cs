using UnityEngine;

public class Node
{
    public bool Walkable { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public Vector2 WorldPosition { get; private set; }
    public int gCost { get; set; }
    public int hCost { get; set; }
    public int fCost => gCost + hCost;
    public Node Parent { get; set; }
    
    public Node(int x, int y, Vector2 worldPosition, bool walkable)
    {
        X = x;
        this.Y = y;
        WorldPosition = worldPosition;
        Walkable = walkable;
    }
}