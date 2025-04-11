using System;
using System.Collections.Generic;

public interface IState
{
    public IStateMachine StateMachine { get; }
    
    public void Initialize();
    public void Enter();
    public void Update();
    public void Exit();
}

public interface IStateMachine
{
    public event Action<IState> OnExitState;
    public IState CurrentState { get; }
    public List<IState> States { get; }
    
    public void Initialize();
    public void AddStates(params IState[] states);
    public void ChangeState(Type type);
    public void Update();
}

public interface IStateMachinable
{
    public IStateMachine StateMachine { get; }
}