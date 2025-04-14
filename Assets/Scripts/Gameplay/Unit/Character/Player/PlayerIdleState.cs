using Common.StateMachine;

namespace Unit.Character.Player
{
    public class PlayerIdleState : State
    {
        public override StateCategory Category { get; } = StateCategory.Idle;

        public override void Update()
        {
            
        }
    }

    public class PlayerIdleStateBuilder : StateBuilder
    {
        public PlayerIdleStateBuilder() : base(new PlayerIdleState())
        {
        }
    }
}