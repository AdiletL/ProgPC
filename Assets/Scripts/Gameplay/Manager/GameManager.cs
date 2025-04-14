using System;
using Unit.BuildableObject;
using UnityEngine;
using Zenject;

namespace Gameplay.Manager
{
    public class GameManager : MonoBehaviour, IManager
    {
        [Inject] private DiContainer diContainer;

        [SerializeField] private SO_GameColor so_GameColor;
        [SerializeField] private SO_GameConfig so_GameConfig;
        [SerializeField] private SO_BuildableObjectContainer so_BuildableObjectContainer;
        [SerializeField] private GameObject playerPrefab;
        
        [Space(15)]
        [SerializeField] private GameObject uiManagerPrefab;
        
        public void Initialize()
        {
            diContainer.Bind<SO_GameColor>().FromInstance(so_GameColor);
            diContainer.Bind<SO_GameConfig>().FromInstance(so_GameConfig);
            diContainer.Bind<SO_BuildableObjectContainer>().FromInstance(so_BuildableObjectContainer);

            var uiManagerObject = diContainer.InstantiatePrefab(uiManagerPrefab);
            var uiManager = uiManagerObject.GetComponent<UIManager>();
            diContainer.Bind(uiManager.GetType()).FromInstance(uiManager).AsSingle();
            uiManager.Initialize();
            
            var newGameObject = diContainer.InstantiatePrefab(playerPrefab);
            newGameObject.transform.position = Vector3.zero;
            newGameObject.GetComponent<IController>().Initialize();
            
        }
    }
}