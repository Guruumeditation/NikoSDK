namespace Net.ArcanaStudio.NikoSDK.Interfaces
{
    public interface INikoResponse<out T>
    {
        string Command { get; }
        T Data { get; }
    }

}
