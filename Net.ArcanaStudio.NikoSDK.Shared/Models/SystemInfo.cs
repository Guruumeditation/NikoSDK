using System;
using System.Runtime.Serialization;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK.Models
{
    [DataContract]
    public class SystemInfo : BaseResponse, ISystemInfo
    {
        [DataMember]
        public string SwVersion { get; }
        [DataMember]
        public string Api { get; }
        [DataMember]
        public DateTime? Date  { get; }
        [DataMember]
        public string Language { get; }
        [DataMember]
        public string Currency { get; }
        [DataMember]
        public string Units { get; }
        [DataMember]
        public string Dst { get; }
        [DataMember]
        public string Tz { get; }
        [DataMember]
        public DateTime? LastEnergyErase { get; }
        [DataMember]
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
