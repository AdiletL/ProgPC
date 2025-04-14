using Gameplay.UI;
using Unit.BuildableObject;
using UnityEngine;
using Zenject;

namespace Gameplay.Manager
{
    public class UIManager : MonoBehaviour, IManager
    {
        [Inject] private DiContainer diContainer;
        [Inject] private SO_BuildableObjectContainer so_BuildableObjectContainer;
        
        [SerializeField] private UIBuildableObjectView buildableObjectViewPrefab;
        
        public void Initialize()
        {
            var buildableObjectViewObject = diContainer.InstantiatePrefab(buildableObjectViewPrefab);
            var buildableObjectView = buildableObjectViewObject.GetComponent<UIBuildableObjectView>();
            diContainer.Bind(buildableObjectView.GetType()).FromInstance(buildableObjectView).AsSingle();
            buildableObjectView.Initialize(so_BuildableObjectContainer.GetBuildableObjectsCount);
        }
    }
}