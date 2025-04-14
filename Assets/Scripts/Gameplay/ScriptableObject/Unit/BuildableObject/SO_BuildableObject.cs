using UnityEngine;

namespace Unit.BuildableObject
{
    public abstract class SO_BuildableObject : ScriptableObject, IBuildableConfig
    {
        public abstract BuildableObjectType BuildableObjectTypeID { get; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public LayerMask SurfacePlacementLayer { get; protected set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}