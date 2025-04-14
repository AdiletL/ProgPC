using System;
using Common.Factory;
using Common.StateMachine;
using Gameplay;
using UnityEngine;
using Zenject;

namespace Unit.Character.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerBuildPresenter))]
    public class PlayerController : MonoBehaviour, IControlable, IStateMachinable, IController
    {
        [Inject] private DiContainer diContainer;
        
        [SerializeField] private GameObject head;
        [SerializeField] private Camera headCamera;
        
        [Space]
        [SerializeField] private SO_PlayerMove so_PlayerMove;
        [SerializeField] private SO_PlayerBuild so_PlayerBuild;
        
        public IControl Control { get; private set; }
        public IStateMachine StateMachine { get; protected set; }
        
        
        private IFactory<Type, IState> stateFactory;

        private IFactory<Type, IState> CreateStateFactory()
        {
            return new PlayerStateFactoryBuilder()
                .SetGameObject(gameObject)
                .SetHead(head)
                .SetPlayerCamera(headCamera)
                .SetMoveConfig(so_PlayerMove)
                .SetBuildConfig(so_PlayerBuild)
                .SetCharacterController(GetComponent<CharacterController>())
                .SetStateMachine(StateMachine)
                .Build();
        }
        
        public void Initialize()
        {
            StateMachine = new StateMachine();
            stateFactory = CreateStateFactory();

            Control = new PlayerControl(StateMachine, so_PlayerMove.RotationSpeed, head);
            
            InitializeStates();
            
            GetComponent<PlayerBuildPresenter>().Initialize();
        }

        private void InitializeStates()
        {
            var idleState = stateFactory.CreateState(typeof(PlayerIdleState));
            diContainer.Inject(idleState);
            StateMachine.AddStates(idleState);
            
            var moveState = stateFactory.CreateState(typeof(PlayerMoveState));
            diContainer.Inject(moveState);
            StateMachine.AddStates(moveState);
            
            var buildState = stateFactory.CreateState(typeof(PlayerBuildState));
            diContainer.Inject(buildState);
            StateMachine.AddStates(buildState);
            
            StateMachine.Initialize();
        }
        
        private void Update()
        {
            Control?.UpdateInputHandler();
            StateMachine.Update();
        }
    }
}
