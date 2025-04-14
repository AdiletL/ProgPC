using UnityEngine;

namespace Unit.BuildableObject
{
    [CreateAssetMenu(fileName = "SO_CubeBuildable", menuName = "SO/Gameplay/Unit/Buildable/Cube", order = 51)]
    public class SO_CubeBuildable : SO_BuildableObject
    {
        public override BuildableObjectType BuildableObjectTypeID { get; } = BuildableObjectType.Cube;
        
    }
}