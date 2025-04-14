using System;
using Unit.BuildableObject;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public abstract class BuildableObject : MonoBehaviour, IBuildable
    {
        [Inject] private SO_GameConfig so_GameConfig;
        [Inject] private SO_BuildableObjectContainer so_BuildableObjectContainer;
        
        public event Action<GameObject> OnCollisionBuildableObject;
        
        protected SO_BuildableObject so_BuildableObject;
        private Renderer mainRenderer;
        private Collider mainCollider;
        private Color baseColor;

        private float alphaColor;

        public abstract BuildableObjectType BuildableObjectTypeID { get; }
        public LayerMask SurfacePlacementLayer { get; protected set; }

        public virtual bool TryGetPlacementPosition(RaycastHit hit, GameObject currentObject, out Vector3 position)
        {
            position = default;
            return false;
        }

        
        protected virtual void Awake()
        {
            so_BuildableObject = so_BuildableObjectContainer.GetBuildableObjectConfig(BuildableObjectTypeID);
            
            SurfacePlacementLayer = so_BuildableObject.SurfacePlacementLayer;
            alphaColor = so_GameConfig.AlphaColor;

            mainRenderer = GetComponent<Renderer>();
            baseColor = mainRenderer.material.color;
            mainCollider = GetComponent<Collider>();
        }
        
        public void ExecutePreview()
        {
            mainRenderer.material.color = new Color(
                mainRenderer.material.color.r,
                mainRenderer.material.color.g,
                mainRenderer.material.color.b,
                alphaColor);
            mainCollider.isTrigger = true;
        }

        public void StopPreview()
        {
            mainRenderer.material.color = baseColor;
            mainCollider.isTrigger = false;
        }

        public void Build()
        {
            StopPreview();
        }
        public void ChangeColor(Color color)
        {
            color.a = alphaColor;
            mainRenderer.material.color = color;
        }

        public void ResetColor()
        {
            mainRenderer.material.color = new Color(
                baseColor.r, baseColor.g, baseColor.b, alphaColor);
        }

        public void ChangeRotate(float angle)
        {
            transform.Rotate(0f, angle, 0f);
        }

        public void ChangePosition(Vector3 position)
        {
            transform.position = position;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out IBuildable buildable))
            {
                OnCollisionBuildableObject?.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IBuildable buildable))
            {
                OnCollisionBuildableObject?.Invoke(null);
            }
        }
    }
}