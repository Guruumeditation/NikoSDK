using System;
using NikoSDK.Interfaces.Data;

namespace NikoSDK.Model.Data
{
    public class SystemInfo : BaseResponse, ISystemInfo
    {
        public string SwVersion { get; }
        public string Api { get; }
        public DateTime? Date  { get; }
        public string Language { get; }
        public string Currency { get; }
        public string Units { get; }
        public string Dst { get; }
        public string Tz { get; }
        public DateTime? LastEnergyErase { get; }
        public DateTime? LastConfig { get; }

        public SystemInfo(string sw_version, string api, DateTime? date, string language, string currency, string units, string dst, string tz, DateTime? last_energy_erase, DateTime? last_config) : base(0)
        {
            SwVersion = sw_version;
            Api = api;
            Date = date;
            Language = language;
            Currency = currency;
            Units = units;
            Dst = dst;
            Tz = tz;
            LastEnergyErase = last_energy_erase;
            LastConfig = last_config;
        }

        public SystemInfo(int error) : base(error)
        {
        }
    }
}
