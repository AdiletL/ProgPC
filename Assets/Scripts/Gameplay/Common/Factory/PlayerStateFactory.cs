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
        private Camera playerCamera;
        
        private SO_PlayerMove so_PlayerMove;
        private SO_PlayerBuild so_PlayerBuild;
        
        public void SetStateMachine(IStateMachine stateMachine) => this.stateMachine = stateMachine;
        public void SetCharacterController(CharacterController characterController) => this.characterController = characterController;
        public void SetGameObject(GameObject gameObject) => this.gameObject = gameObject;
        public void SetHead(GameObject head) => this.head = head;
        public void SetPlayerCamera(Camera camera) => this.playerCamera = camera;
        public void SetMoveConfig(SO_PlayerMove so_PlayerMove) => this.so_PlayerMove = so_PlayerMove;
        public void SetBuildConfig(SO_PlayerBuild so_PlayerBuild) => this.so_PlayerBuild = so_PlayerBuild;
        
        
        public IState CreateState(Type type)
        {
            IState result = type switch
            {
                _ when type == typeof(PlayerMoveState) => CreateMoveState(),
                _ when type == typeof(PlayerIdleState) => CreateIdleState(),
                _ when type == typeof(PlayerBuildState) => CreateBuildState(),
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
            return (PlayerIdleState)new PlayerIdleStateBuilder()
                .SetStateMachine(stateMachine)
                .Build();
        }
        
        private PlayerBuildState CreateBuildState()
        {
            return (PlayerBuildState)new PlayerBuildStateBuilder()
                .SetConfig(so_PlayerBuild)
                .SetPlayerCamera(playerCamera)
                .SetStateMachine(stateMachine)
                .Build();
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
        public PlayerStateFactoryBuilder SetPlayerCamera(Camera camera)
        {
            factory.SetPlayerCamera(camera);
            return this;
        }
        public PlayerStateFactoryBuilder SetMoveConfig(SO_PlayerMove so_PlayerMove)
        {
            factory.SetMoveConfig(so_PlayerMove);
            return this;
        }
        public PlayerStateFactoryBuilder SetBuildConfig(SO_PlayerBuild so_PlayerBuild)
        {
            factory.SetBuildConfig(so_PlayerBuild);
            return this;
        }
        
        public PlayerStateFactory Build()
        {
            return factory;
        }
    }
}