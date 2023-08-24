namespace CoreLayer.Utilities.Results
{
    public interface IResultObject
    {
        bool Success { get; }
        string Message { get; }
    }
}
