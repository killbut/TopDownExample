using UnityEngine;

public class HoldingPosition : MonoBehaviour
{
    private Vector2 _min;
    private Vector2 _max;
    public static HoldingPosition Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        var camera = Camera.main;
        _min = camera.ViewportToWorldPoint(new Vector2(0, 0));
        _max = camera.ViewportToWorldPoint(new Vector2(1, 1));
    }
    public Vector2 CheckPosition(Vector2 position)
    {
        position.x = Mathf.Clamp(position.x, _min.x, _max.x);
        position.y = Mathf.Clamp(position.y, _min.y, _max.y);
        return position;
    }
}