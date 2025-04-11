public interface IFactory<in TIn, out TOut>
{
    public TOut CreateState(TIn state);
}
