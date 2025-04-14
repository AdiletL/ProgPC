using System;
using System.Collections.Generic;

public enum StateCategory
{
    nothing,
    Idle,
    Move,
    Build,
}

public interface IState
{
    public StateCategory Category { get; }
    public IStateMachine StateMachine { get; }
    public bool IsActive { get; }
    public bool IsCanExit { get; }
    public bool IsInitialized { get; }

    public void Initialize();
    public void Enter();
    public void Subscribe();
    public void Update();
    public void Unsubscribe();
    public void Exit();
}

public interface IStateMachine
{
    public event Action<IState> OnExitCategory;
    public event Action OnUpdate;
    
    public void Initialize();
    public T GetState<T>() where T : IState;
    public T GetInterfaceImplementingClass<T>() where T : class;
    public void AddStates(params IState[] states);
    public void SetStates(bool isForceSetState = false, params Type[] desiredStates);
    public void ExitCategory(StateCategory excludedCategory, Type installationState, bool isForceSetState = false);
    public void ExitOtherStates(Type installationState, bool isForceSetState = false);
    public void Update();
}

public interface IStateMachinable
{
    public IStateMachine StateMachine { get; }
}