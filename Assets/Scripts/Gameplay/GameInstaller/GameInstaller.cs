using Gameplay.Manager;
using UnityEngine;
using Zenject;

namespace Gameplay.GameInstaller
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameObject gameManagerPrefab;

        public override void InstallBindings()
        {
            Container = new DiContainer();
        }

        public override void Start()
        {
            base.Start();
            var newGameObject = Container.InstantiatePrefab(gameManagerPrefab);
            newGameObject.GetComponent<IManager>().Initialize();
        }
    }
}