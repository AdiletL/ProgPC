using UnityEngine;

public interface IMovement
{
    public float CurrentMovementSpeed { get; }
    public void ExecuteMovement();
}

public interface IMovementConfig
{
    public float MovementSpeed { get; }
}
