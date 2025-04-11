public interface IControl
{
    public void UpdateInputHandler();
}

public interface IControlable
{
    public IControl Control { get; }
}