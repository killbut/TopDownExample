using UnityEngine;

public class HoldingPosition : MonoBehaviour, ICheckPosition
{
    private Vector2 _min;
    private Vector2 _max;
    private void Awake()
    {
        var camera = Camera.main;
        _min = camera.ViewportToWorldPoint(new Vector2(0, 0));
        _max = camera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    protected void OnEnable()
    {
        EventBus.Subscribe(this);
    }

    protected void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    public Vector2 CheckPosition(Vector2 position)
    {
        var clampPosition = position;
        clampPosition.x = Mathf.Clamp(clampPosition.x, _min.x, _max.x);
        clampPosition.y = Mathf.Clamp(clampPosition.y, _min.y, _max.y);
        return clampPosition;
    }
}