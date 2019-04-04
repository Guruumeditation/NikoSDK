namespace Net.ArcanaStudio.NikoSDK.Interfaces
{
    public interface INikoResponse<out T> : IBaseResponse
    {
        T Data { get; }
    }

}
