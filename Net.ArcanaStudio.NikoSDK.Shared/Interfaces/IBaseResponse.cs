namespace Net.ArcanaStudio.NikoSDK.Interfaces
{
    public interface IBaseResponse
    {
        string Command { get; }
        int Error { get; }

        bool IsError { get; }
    }
}
