using UnityEngine;

[CreateAssetMenu(fileName = "SO_GameColor", menuName = "SO/Gameplay/Color", order = 51)]
public class SO_GameColor : ScriptableObject
{
    [Header("Buildable Object")]
    [field: SerializeField] public Color ValidatePlacement { get; private set; }
    [field: SerializeField] public Color InvalidatePlacement { get; private set; }
}
