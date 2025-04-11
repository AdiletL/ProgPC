using Gameplay.Common.Mouse;
using UnityEngine;

namespace Unit.Character.Player
{
    public class PlayerControl : IControl
    {
        private IStateMachine stateMachine;
        private MouseLook mouseLook;

        private GameObject head;
        private float rotationSpeed;
        private bool isCanMovement = true;
        
        public PlayerControl(IStateMachine stateMachine, float rotationSpeed, GameObject head)
        {
            this.stateMachine = stateMachine;
            this.rotationSpeed = rotationSpeed;
            this.head = head;
            SubscribeEvent();

            mouseLook = new MouseLook(this.head, this.rotationSpeed);
        }

        ~PlayerControl()
        {
            UnsubscribeEvent();
        }

        public void SubscribeEvent()
        {
            stateMachine.OnExitState += OnExitState;
        }

        public void UnsubscribeEvent()
        {
            stateMachine.OnExitState -= OnExitState;
        }

        private void OnExitState(IState state)
        {
            if (typeof(IMovement).IsAssignableFrom(state.GetType()))
                isCanMovement = true;
        }
        
        public void UpdateInputHandler()
        {
            if((Input.GetKey(KeyCode.A)
               || Input.GetKey(KeyCode.D)
               || Input.GetKey(KeyCode.W) 
               || Input.GetKey(KeyCode.S))
               && isCanMovement)
            {
                stateMachine.ChangeState(typeof(PlayerMoveState));
                isCanMovement = false;
            }
            
            mouseLook.ExecuteRotate();
        }
    }
}