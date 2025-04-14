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
        private bool isCanControl;
        
        public PlayerControl(IStateMachine stateMachine, float rotationSpeed, GameObject head)
        {
            this.stateMachine = stateMachine;
            this.rotationSpeed = rotationSpeed;
            this.head = head;
            SubscribeEvent();

            mouseLook = new MouseLook(this.head, this.rotationSpeed);
            Activate();
        }

        ~PlayerControl()
        {
            UnsubscribeEvent();
        }

        public void SubscribeEvent()
        {
            stateMachine.OnExitCategory += OnExitCategory;
        }

        public void UnsubscribeEvent()
        {
            stateMachine.OnExitCategory -= OnExitCategory;
        }

        private void OnExitCategory(IState state)
        {
            if (typeof(IMovement).IsAssignableFrom(state.GetType()))
                isCanMovement = true;
        }

        public void Activate() => isCanControl = true;

        public void Deactivate()
        {
            isCanControl = false;
            stateMachine.ExitCategory(StateCategory.Move, null);
        }

        public void UpdateInputHandler()
        {
            if(!isCanControl) return;
            
            if((Input.GetKey(KeyCode.A)
               || Input.GetKey(KeyCode.D)
               || Input.GetKey(KeyCode.W) 
               || Input.GetKey(KeyCode.S))
               && isCanMovement)
            {
                stateMachine.ExitOtherStates(typeof(PlayerMoveState));
                isCanMovement = false;
            }
            
            mouseLook.ExecuteRotate();
        }
    }
}