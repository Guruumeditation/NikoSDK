using System.Runtime.Serialization;

namespace NikoSDK.Interfaces.Data
{
    public interface ILocation : IBaseResponse
    {
        int Id { get; }
        string Name { get; }
    }
}
