public  class BaseState
{
    protected StateMachine _stateMachine;
    public BaseState(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    public virtual void Enter(){}
    public  virtual void Update(){}
    public  virtual void FixedUpdate(){}
    public  virtual void OnGUI (){}
    public  virtual void Exit(){}
}