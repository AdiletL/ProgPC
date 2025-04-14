using UnityEngine;

[CreateAssetMenu(fileName = "SO_GameConfig", menuName = "SO/Gameplay/Config", order = 51)]
public class SO_GameConfig : ScriptableObject
{
    [Header("BuildableObject")]
    [field: SerializeField] public float AlphaColor { get; protected set; } = .4f;
    [field: SerializeField] public float AngleRotate { get; protected set; } = 45f;
}
