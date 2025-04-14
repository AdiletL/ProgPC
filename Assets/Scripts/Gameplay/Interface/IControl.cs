public interface IControl
{
    public void Activate();
    public void Deactivate();
    public void UpdateInputHandler();
}

public interface IControlable
{
    public IControl Control { get; }
}