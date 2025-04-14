using System;
using UnityEngine;

public enum BuildableObjectType
{
   Nothing,
   Cube,
   Sphere,
}

public interface IBuildable
{
   public event Action<GameObject> OnCollisionBuildableObject;
   
   public BuildableObjectType BuildableObjectTypeID { get; }
   public LayerMask SurfacePlacementLayer { get; }
   
   public bool TryGetPlacementPosition(RaycastHit hit, GameObject currentObject, out Vector3 position);
   
   public void ExecutePreview();
   public void StopPreview();
   public void Build();
   public void ChangeColor(Color color);
   public void ResetColor();
   public void ChangeRotate(float angle);
   public void ChangePosition(Vector3 position);
}

public interface IBuildableConfig
{
   public BuildableObjectType BuildableObjectTypeID { get; }
   public Sprite Icon { get; }
   public LayerMask SurfacePlacementLayer { get; }
}

public interface IBuildMode
{
   public void Initialize();
   public void SetBuildableObject(GameObject buildableObject);
}
