using System.Collections.Generic;
using Gameplay;
using Gameplay.UI;
using Unit.BuildableObject;
using UnityEngine;
using Zenject;

namespace Unit.Character.Player
{
    public class PlayerBuildPresenter : MonoBehaviour
    {
        [Inject] private DiContainer diContainer;
        [Inject] private UIBuildableObjectView uiBuildableObjectView;
        [Inject] private SO_BuildableObjectContainer so_BuildableObjectContainer;
        
        [SerializeField] private PlayerController playerController;
        
        private IBuildMode buildMode;
        
        public void Initialize()
        {
            buildMode = playerController.StateMachine.GetInterfaceImplementingClass<IBuildMode>();
            buildMode.Initialize();
            uiBuildableObjectView.OnSelectBuildableObject += OnSelectBuildableObject;
            playerController.StateMachine.OnExitCategory += OnExitCategory;
            
            var allBuildableObjects = so_BuildableObjectContainer.GetBuildableObjects();
            for (int i = 0; i < allBuildableObjects.Length; i++)
            {
                uiBuildableObjectView.SetImage(allBuildableObjects[i].Icon, i);
            }
            
            playerController.Control.Deactivate();
            Cursor.lockState = CursorLockMode.None;
            uiBuildableObjectView.Show();
        }

        private void OnSelectBuildableObject(int slotID)
        {
            var newGameObject = diContainer.InstantiatePrefab(so_BuildableObjectContainer.GetBuildableObjects()[slotID].Prefab);
            buildMode.SetBuildableObject(newGameObject);
            playerController.StateMachine.SetStates(true, typeof(IBuildMode));
            playerController.Control.Activate();
            Cursor.lockState = CursorLockMode.Locked;
            uiBuildableObjectView.Hide();
        }

        private void OnExitCategory(IState state)
        {
            if (typeof(IBuildMode).IsAssignableFrom(state.GetType()))
            {
                playerController.Control.Deactivate();
                Cursor.lockState = CursorLockMode.None;
                uiBuildableObjectView.Show();
            }
        }
    }
}