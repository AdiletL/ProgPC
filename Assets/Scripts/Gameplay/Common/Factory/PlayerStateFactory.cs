using System;
using Unit.Character.Player;
using UnityEngine;

namespace Common.Factory
{
    public class PlayerStateFactory : IFactory<Type, IState>
    {
        private IStateMachine stateMachine;
        private CharacterController characterController;
        private GameObject gameObject;
        private GameObject head;
        
        private SO_PlayerMove so_PlayerMove;
        
        public void SetStateMachine(IStateMachine stateMachine) => this.stateMachine = stateMachine;
        public void SetCharacterController(CharacterController characterController) => this.characterController = characterController;
        public void SetGameObject(GameObject gameObject) => this.gameObject = gameObject;
        public void SetHead(GameObject head) => this.head = head;
        public void SetMoveConfig(SO_PlayerMove so_PlayerMove) => this.so_PlayerMove = so_PlayerMove;
        
        
        
        public IState CreateState(Type type)
        {
            IState result = type switch
            {
                _ when type == typeof(PlayerMoveState) => CreateMoveState(),
                _ when type == typeof(PlayerIdleState) => CreateIdleState(),
                _ => throw new ArgumentException($"Unknown state type: {type}")
            };
            
            return result;
        }

        private PlayerMoveState CreateMoveState()
        {
            return (PlayerMoveState)new PlayerMoveStateBuilder()
                .SetConfig(so_PlayerMove)
                .SetGameObject(gameObject)
                .SetHead(head)
                .SetCharacterController(characterController)
                .SetStateMachine(stateMachine)
                .Build();
        }

        private PlayerIdleState CreateIdleState()
        {
            return new PlayerIdleState();
        }
    }

    public class PlayerStateFactoryBuilder
    {
        private PlayerStateFactory factory;

        public PlayerStateFactoryBuilder()
        {
            factory = new PlayerStateFactory();
        }
        
        public PlayerStateFactoryBuilder SetStateMachine(IStateMachine stateMachine)
        {
            factory.SetStateMachine(stateMachine);
            return this;
        } 
        public PlayerStateFactoryBuilder SetCharacterController(CharacterController characterController)
        {
            factory.SetCharacterController(characterController);
            return this;
        }
        public PlayerStateFactoryBuilder SetGameObject(GameObject gameObject)
        {
            factory.SetGameObject(gameObject);
            return this;
        }
        public PlayerStateFactoryBuilder SetHead(GameObject head)
        {
            factory.SetHead(head);
            return this;
        }
        public PlayerStateFactoryBuilder SetMoveConfig(SO_PlayerMove so_PlayerMove)
        {
            factory.SetMoveConfig(so_PlayerMove);
            return this;
        }
        
        public PlayerStateFactory Build()
        {
            return factory;
        }
    }
}