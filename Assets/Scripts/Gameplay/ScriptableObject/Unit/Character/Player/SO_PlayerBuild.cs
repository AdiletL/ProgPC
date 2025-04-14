using UnityEngine;

namespace Unit.Character.Player
{
    [CreateAssetMenu(fileName = "SO_PlayerBuild", menuName = "SO/Gameplay/Unit/Character/Player/Build", order = 51)]
    public class SO_PlayerBuild : ScriptableObject
    {
        [field: SerializeField] public float MaxPlacementDistance { get; private set; }
        [field: SerializeField] public float MaxPreviewDistance { get; private set; } = 3.5f;
    }
}