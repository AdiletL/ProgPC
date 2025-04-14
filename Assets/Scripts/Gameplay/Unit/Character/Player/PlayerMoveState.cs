using Common.StateMachine;
using UnityEngine;

namespace Unit.Character.Player
{
    public class PlayerMoveState : State, IMovement
    {
        public override StateCategory Category { get; } = StateCategory.Move;
        
        private SO_PlayerMove so_PlayerMove;
        private CharacterController characterController;
        private GameObject gameObject;
        private GameObject head;

        private Vector3 direction;
        private Vector3 directionMove;
        private Vector3 directionGravity;
        
        public float CurrentMovementSpeed { get; protected set; }
        
        
        public void SetConfig(SO_PlayerMove so_PlayerMove) => this.so_PlayerMove = so_PlayerMove;
        public void SetCharacterController(CharacterController characterController) => this.characterController = characterController;
        public void SetGameObject(GameObject gameObject) => this.gameObject = gameObject;
        public void SetHead(GameObject head) => this.head = head;


        public override void Initialize()
        {
            base.Initialize();
            CurrentMovementSpeed = so_PlayerMove.MovementSpeed;
            directionGravity = Physics.gravity;
        }

        public override void Update()
        {
            CheckDirectionMovement();
            ExecuteMovement();

            if (direction.magnitude == 0)
                StateMachine.ExitCategory(Category, null);
        }
        
        private void CheckDirectionMovement()
        {
            direction = Vector3.zero;
            
            if (Input.GetKey(KeyCode.A)) direction.x = -1;
            if (Input.GetKey(KeyCode.D)) direction.x = 1;
            if (Input.GetKey(KeyCode.W)) direction.z = 1;
            if (Input.GetKey(KeyCode.S)) direction.z = -1;
            
            if(direction.magnitude > 0)
                direction.Normalize();
        }

        public void ExecuteMovement()
        {
            directionMove = head.transform.forward * direction.z + head.transform.right * direction.x;
            characterController.Move((directionMove + directionGravity) * (CurrentMovementSpeed * Time.deltaTime));
        }
    }

    public class PlayerMoveStateBuilder : StateBuilder
    {
        public PlayerMoveStateBuilder() : base(new PlayerMoveState())
        {
        }

        public PlayerMoveStateBuilder SetConfig(SO_PlayerMove so_PlayerMove)
        {
            if(state is PlayerMoveState playerMoveState)
                playerMoveState.SetConfig(so_PlayerMove);
            return this;
        }
        public PlayerMoveStateBuilder SetCharacterController(CharacterController characterController)
        {
            if(state is PlayerMoveState playerMoveState)
                playerMoveState.SetCharacterController(characterController);
            return this;
        }

        public PlayerMoveStateBuilder SetGameObject(GameObject gameObject)
        {
            if(state is PlayerMoveState playerMoveState)
                playerMoveState.SetGameObject(gameObject);
            return this;
        }
        public PlayerMoveStateBuilder SetHead(GameObject head)
        {
            if(state is PlayerMoveState playerMoveState)
                playerMoveState.SetHead(head);
            return this;
        }
    }
}