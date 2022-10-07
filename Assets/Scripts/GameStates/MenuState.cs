using UnityEngine;

public class MenuState : BaseState
{
    private Vector2 _centerScreen;
    
    public MenuState(StateMachine stateMachine,Camera camera) : base(stateMachine)
    {
        _centerScreen = camera.WorldToScreenPoint(new Vector3(0, 0, 0));
    }

    public override void OnGUI()
    {
        base.OnGUI();
        if (GUI.Button(new Rect(_centerScreen, new Vector2(100f, 50f)), "Play"))
        {
            _stateMachine.ChangeState(_stateMachine.States[typeof(GameState)]);
        }
    }
}