using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState _currentState;
    public Dictionary<Type,BaseState> States { get; protected set; }
    
    private void Start()
    {
        States = new Dictionary<Type, BaseState>();
        _currentState = GetFirstState();
        if (_currentState != null) _currentState.Enter();
    }

    public void ChangeState(BaseState newState)
    {
        if (newState == null)
            new NullReferenceException("State cant change because is null");
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    private void OnGUI()
    {
        if(_currentState!=null)
            _currentState.OnGUI();
    }

    private void Update()
    {
        _currentState.Update();
    }

    protected virtual BaseState GetFirstState()
    {
        return null;
    }
}