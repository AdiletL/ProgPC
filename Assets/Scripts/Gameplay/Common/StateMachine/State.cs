namespace Common.StateMachine
{
    public abstract class State : IState
    {
        public IStateMachine StateMachine { get; protected set; }
        
        public void SetStateMachine(IStateMachine stateMachine) => this.StateMachine = stateMachine;

        public virtual void Initialize()
        {
            
        }

        public virtual void Enter()
        {
            
        }

        public abstract void Update();

        public virtual void Exit()
        {
            
        }
    }

    public abstract class StateBuilder
    {
        protected State state;

        public StateBuilder(State instance)
        {
            state = instance;
        }

        public StateBuilder SetStateMachine(IStateMachine stateMachine)
        {
            state.SetStateMachine(stateMachine);
            return this;
        }
        
        public State Build()
        {
            return state;
        }
    }
}