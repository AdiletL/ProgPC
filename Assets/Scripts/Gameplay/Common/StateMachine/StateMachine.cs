using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.StateMachine
{
    public class StateMachine : IStateMachine
    {
        public event Action<IState> OnExitCategory;
        public event Action OnUpdate;
        
        private readonly Dictionary<StateCategory, IState> activeStates = new();
        private readonly Dictionary<Type, IState> states = new();
        private readonly List<StateCategory> cachedCategories = new();

        private IState defaultIdleState;
        private bool isBlockChangeState;

        public bool IsStateNotNull(Type state) => FindMostDerivedState(state) != null;

        public bool IsActivateType(Type state)
        {
            foreach (var item in activeStates.Values)
            {
                if (state.IsAssignableFrom(item.GetType()))
                {
                    return true;
                }
            }
            return false;
        }

        public T GetState<T>() where T : IState
        {
            foreach (var state in states.Values)
            {
                if (state is T desiredState)
                {
                    return desiredState;
                }
            }
            return default(T);
        }

        public T GetInterfaceImplementingClass<T>() where T : class
        {
            foreach (var kvp in states)
            {
                if (kvp.Key.GetInterfaces().Contains(typeof(T)))
                {
                    return kvp.Value as T;
                }
            }

            return null;
        }
        
        public bool TryGetInterfaceImplementingClass<T>(out T result) where T : class
        {
            foreach (var kvp in states)
            {
                if (kvp.Key.GetInterfaces().Contains(typeof(T)))
                {
                    result = kvp.Value as T;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public void Initialize()
        {
            foreach (var state in states.Values)
            {
                if(!state.IsInitialized) state.Initialize();
                if (state.Category == StateCategory.Idle && defaultIdleState == null)
                {
                    defaultIdleState = FindMostDerivedState(state.GetType());
                }
            }
        }

        public void AddStates(params IState[] states)
        {
            foreach (var state in states)
            {
                this.states[state.GetType()] = state;
            }
        }

        public void ActiveBlockChangeState()
        { 
            isBlockChangeState = true;
        }
        public void InActiveBlockChangeState()
        { 
            isBlockChangeState = false;
        }
        
        public void SetStates(bool isForceSetState = false, params Type[] desiredStates)
        {
            if(isBlockChangeState) return;
            
            foreach (var baseType in desiredStates)
            {
                var iState = FindMostDerivedState(baseType);
                if (iState == null) continue;

                var category = iState.Category;
                var type = iState.GetType();
                if (activeStates.TryGetValue(category, out var activeState))
                {
                    if (activeState.GetType() == type ||
                        !activeState.IsCanExit)
                        continue;
                    
                    activeState.Exit();
                }
                
                if (!activeStates.ContainsKey(category) || (isForceSetState && activeStates[category] != iState) || 
                    activeStates[category] != iState)
                {
                    activeStates[category] = iState;
                    iState.Enter();
                }
            }
        }

        private IState FindMostDerivedState(Type baseType)
        {
            if (baseType == null) return null;
            
            IState mostDerivedState = null;

            foreach (var state in states.Values)
            {
                Type stateType = state.GetType();

                if (baseType.IsAssignableFrom(stateType) &&
                    (mostDerivedState == null || stateType.IsSubclassOf(mostDerivedState.GetType())))
                {
                    mostDerivedState = state;
                }
            }

            return mostDerivedState;
        }

        public void ExitOtherStates(Type installationState, bool isForceSetState = false)
        {
            if(this.isBlockChangeState) return;
            
            cachedCategories.Clear();
            cachedCategories.AddRange(activeStates.Keys);
            var targetState = FindMostDerivedState(installationState);
            
            foreach (var category in cachedCategories)
            {
                if (category == targetState?.Category ||
                    !activeStates[category].IsCanExit) continue;

                activeStates[category].Exit();
                OnExitCategory?.Invoke(activeStates[category]);
                activeStates.Remove(category);
            }

            SetStates(isForceSetState, installationState);

            if (activeStates.Count == 0)
                SetDefaultState();
        }

        public void ExitCategory(StateCategory excludedCategory, Type installationState, bool isForceSetState = false)
        {
            if (this.isBlockChangeState) return;
            
            if (activeStates.TryGetValue(excludedCategory, out var state))
            {
                if (state.IsCanExit && !isForceSetState ||
                    isForceSetState)
                {
                    state.Exit();
                    activeStates.Remove(excludedCategory);
                    OnExitCategory?.Invoke(state);
                }
            }

            if (installationState != null)
            {
                SetStates(isForceSetState, installationState);
            }
            
            if (activeStates.Count == 0)
                SetDefaultState();
        }

        private void SetDefaultState()
        {
            if (defaultIdleState != null)
            {
                SetStates(desiredStates: defaultIdleState.GetType());
            }
        }

        public void Update() => OnUpdate?.Invoke();

    }
}
       /* public event Action<IState> OnExitState;
        
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
        }*/
