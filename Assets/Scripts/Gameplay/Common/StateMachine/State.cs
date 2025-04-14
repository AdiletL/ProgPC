namespace Common.StateMachine
{
    public abstract class State : IState
    {
        public abstract StateCategory Category { get; }
        public IStateMachine StateMachine { get; protected set; }
        public bool IsActive { get; private set; }
        public bool IsCanExit { get; protected set; } = true;
        public bool IsInitialized { get; protected set; }
        
        public void SetStateMachine(IStateMachine stateMachine) => this.StateMachine = stateMachine;

        public virtual void Initialize()
        {
            IsInitialized = true;
        }
        public virtual void Enter()
        {
            IsActive = true;
            Subscribe();
        }

        public virtual void Subscribe()
        {
            StateMachine.OnUpdate += Update;
        }

        public abstract void Update();
        
        public virtual void Unsubscribe()
        {
            StateMachine.OnUpdate -= Update;
        }

        public virtual void Exit()
        {
            IsActive = false;
            Unsubscribe();
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