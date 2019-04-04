using System;
using System.Runtime.Serialization;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK.Models
{
    [DataContract]
    public sealed class SystemInfo : ISystemInfo, IEquatable<ISystemInfo>
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

        public SystemInfo(string sw_version, string api, DateTime? date, string language, string currency, string units, string dst, string tz, DateTime? last_energy_erase, DateTime? last_config)
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

        #region IEquality

        public bool Equals(ISystemInfo other)
        {
            return string.Equals(SwVersion, other.SwVersion) && string.Equals(Api, other.Api) && Date.Equals(other.Date) && string.Equals(Language, other.Language) && string.Equals(Currency, other.Currency) && string.Equals(Units, other.Units) && string.Equals(Dst, other.Dst) && string.Equals(Tz, other.Tz) && LastEnergyErase.Equals(other.LastEnergyErase) && LastConfig.Equals(other.LastConfig);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ISystemInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (SwVersion != null ? SwVersion.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Api != null ? Api.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Date.GetHashCode();
                hashCode = (hashCode * 397) ^ (Language != null ? Language.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Currency != null ? Currency.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Units != null ? Units.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Dst != null ? Dst.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Tz != null ? Tz.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ LastEnergyErase.GetHashCode();
                hashCode = (hashCode * 397) ^ LastConfig.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}
