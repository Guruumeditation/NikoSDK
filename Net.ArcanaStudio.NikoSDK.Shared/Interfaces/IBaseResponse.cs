namespace Net.ArcanaStudio.NikoSDK.Interfaces
{
    public interface IBaseResponse
    {
        int Error { get; }

        bool IsError { get; }
    }
}
