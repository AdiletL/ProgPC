using UnityEngine;

namespace Unit.BuildableObject
{
    [CreateAssetMenu(fileName = "SO_SphereBuildable", menuName = "SO/Gameplay/Unit/Buildable/Sphere", order = 51)]
    public class SO_SphereBuildable : SO_BuildableObject
    {
        public override BuildableObjectType BuildableObjectTypeID { get; } = BuildableObjectType.Sphere;
    }
}