using System;
using System.Collections.Generic;

namespace Common.StateMachine
{
    public class StateMachine : IStateMachine
    {
        public event Action<IState> OnExitState;
        
        public IState CurrentState { get; protected set; }
        public List<IState> States { get; protected set; }
        
        public void Initialize()
        {
            foreach (var VARIABLE in States)
                VARIABLE.Initialize();
        }

        public void AddStates(params IState[] states)
        {
            States ??= new List<IState>();
            foreach (var VARIABLE in states)
            {
                if(!States.Contains(VARIABLE))
                    States.Add(VARIABLE);
            }
        }

        public void ChangeState(Type stateType)
        {
            for (int i = States.Count - 1; i >= 0; i--)
            {
                var currentStateType = States[i].GetType();
                if (stateType.IsAssignableFrom(currentStateType))
                {
                    if(CurrentState == States[i]) return;
                    
                    CurrentState?.Exit();
                    if(CurrentState != null)
                        OnExitState?.Invoke(CurrentState);
                    CurrentState = States[i];
                    CurrentState?.Enter();
                    break;
                }
            }
        }
        
        public void Update()
        {
            CurrentState?.Update();
        }
    }
}