public interface IRotate
{
    public float CurrentRotationSpeed { get; }
    public void ExecuteRotate();
}

public interface IRotateConfig
{
    public float RotationSpeed { get; }
}