namespace Net.ArcanaStudio.NikoSDK.Interfaces
{
    public interface ILocation : IBaseResponse
    {
        int Id { get; }
        string Name { get; }
    }
}
