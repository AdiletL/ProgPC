using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI
{
    public class UIBuildableObjectView : MonoBehaviour
    {
        public event Action<int> OnSelectBuildableObject; 
        
        [SerializeField] private UIBuilderObject uiBuilderObjectPrefab;
        [SerializeField] private Transform container;

        private List<UIBuilderObject> uiBuilderObjects;
        
        public void Initialize(int maxBuildableObjects)
        {
            uiBuilderObjects ??= new List<UIBuilderObject>();
            for (int i = 0; i < maxBuildableObjects; i++)
            {
                var uiBuilderObject = Instantiate(uiBuilderObjectPrefab, container);
                uiBuilderObject.SetSlotID(i);
                uiBuilderObject.OnSelect += OnSelectObject;
                uiBuilderObjects.Add(uiBuilderObject);
            }
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
        
        private void OnSelectObject(int slotID) => OnSelectBuildableObject?.Invoke(slotID);

        public void SetImage(Sprite sprite, int index) => uiBuilderObjects[index].SetImage(sprite);
    }
}