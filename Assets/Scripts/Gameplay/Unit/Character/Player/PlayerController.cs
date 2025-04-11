using System;
using Common.Factory;
using Common.StateMachine;
using UnityEngine;

namespace Unit.Character.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, IControlable, IStateMachinable
    {
        [SerializeField] private GameObject head;
        
        [Space]
        [SerializeField] private SO_PlayerMove so_PlayerMove;
        
        public IControl Control { get; private set; }
        public IStateMachine StateMachine { get; protected set; }
        
        
        private IFactory<Type, IState> stateFactory;

        private IFactory<Type, IState> CreateStateFactory()
        {
            return new PlayerStateFactoryBuilder()
                .SetGameObject(gameObject)
                .SetHead(head)
                .SetMoveConfig(so_PlayerMove)
                .SetCharacterController(GetComponent<CharacterController>())
                .SetStateMachine(StateMachine)
                .Build();
        }
        
        private void Start()
        {
            StateMachine = new StateMachine();
            stateFactory = CreateStateFactory();

            InitializeStates();
            
            Control = new PlayerControl(StateMachine, so_PlayerMove.RotationSpeed, head);
            
            StateMachine.Initialize();
        }

        private void InitializeStates()
        {
            var idleState = stateFactory.CreateState(typeof(PlayerIdleState));
            StateMachine.AddStates(idleState);
            
            var moveState = stateFactory.CreateState(typeof(PlayerMoveState));
            StateMachine.AddStates(moveState);
        }

        private void Update()
        {
            Control?.UpdateInputHandler();
            StateMachine.Update();
        }
    }
}
