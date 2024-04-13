namespace CoreLayer.Utilities.Results
{
    public interface IDataResult<out T> : IResultObject
    {
        T Data { get; }
    }
}
