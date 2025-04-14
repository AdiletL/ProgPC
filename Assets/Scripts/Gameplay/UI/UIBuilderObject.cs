using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class UIBuilderObject : MonoBehaviour
    {
        public event Action<int> OnSelect;
        
        [SerializeField] private Image image;
        [SerializeField] private Button button;

        private int slotID;
        
        private void Start()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick() => OnSelect?.Invoke(slotID);
        
        public void SetSlotID(int slotID) => this.slotID = slotID;
        public void SetImage(Sprite sprite) => image.sprite = sprite;

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}