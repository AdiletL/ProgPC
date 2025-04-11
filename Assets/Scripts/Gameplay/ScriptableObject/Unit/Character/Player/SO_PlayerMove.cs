using UnityEngine;

namespace Unit.Character.Player
{
    [CreateAssetMenu(fileName = "SO_PlayerMove", menuName = "SO/Gameplay/Unit/Character/Player/Move", order = 51)]
    public class SO_PlayerMove : ScriptableObject, IMovementConfig, IRotateConfig
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float RotationSpeed { get; private set; }
    }
}