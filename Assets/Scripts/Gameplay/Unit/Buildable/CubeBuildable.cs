using UnityEngine;

namespace Gameplay
{
    public class CubeBuildable : BuildableObject
    {
        public override BuildableObjectType BuildableObjectTypeID { get; } = BuildableObjectType.Cube;
        
        
        Vector3 GetTopPosition(GameObject target, GameObject spawning)
        {
            Collider targetCol = target.GetComponent<Collider>();
            Collider spawnCol = spawning.GetComponent<Collider>();

            return (targetCol != null && spawnCol != null)
                ? new Vector3(
                    target.transform.position.x,
                    targetCol.bounds.max.y + spawnCol.bounds.extents.y,
                    target.transform.position.z)
                : Vector3.zero;
        }
        
        public override bool TryGetPlacementPosition(RaycastHit hit, GameObject currentObject, out Vector3 position)
        {
            if (hit.collider.TryGetComponent(out IBuildable buildable) &&
                buildable.BuildableObjectTypeID == BuildableObjectType.Cube)
            {
                position = GetTopPosition(hit.collider.gameObject, currentObject);
                return position != Vector3.zero;
            }

            position = default;
            return false;
        }
    }
}