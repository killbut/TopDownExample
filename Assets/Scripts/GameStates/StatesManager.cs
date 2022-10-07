using UnityEngine;

public class StatesManager : StateMachine
{
    [SerializeField] private Level _level;
    private GameState _gameState;
    private MenuState _menuState;
    private void Awake()
    {
        _gameState = new GameState(this,_level);
        _menuState = new MenuState(this, Camera.main);
    }

    protected override BaseState GetFirstState()
    {
        States.Add(typeof(MenuState), _menuState);
        States.Add(typeof(GameState),_gameState);
        return _menuState;
    }
}