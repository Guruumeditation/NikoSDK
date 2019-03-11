namespace NikoSDK.Interfaces.Data
{
    public interface IBaseResponse
    {
        int Error { get; }

        bool IsError { get; }
    }
}
