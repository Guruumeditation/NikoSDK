using System;

namespace Net.ArcanaStudio.NikoSDK.Interfaces
{
    public interface ISystemInfo : IBaseResponse
    {
        string SwVersion { get; }
        string Api { get; }
        DateTime? Date { get; }
        string Language { get; }
        string Currency { get; }
        string Units { get; }
        string Dst { get; }
        string Tz { get; }
        DateTime? LastEnergyErase { get; }
        DateTime? LastConfig { get; }
    }
}