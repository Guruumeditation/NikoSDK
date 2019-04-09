using System;
using System.Runtime.Serialization;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK.Models
{
    [DataContract]
    public sealed class Location : ILocation, IEquatable<ILocation>
    {
        [DataMember]
        public int Id { get; }
        [DataMember]
        public string Name { get; }

        public Location(int id, string name)
        {
            Id = id;
            Name = name;
        }

        #region Equality members

        public bool Equals(ILocation other)
        {
            return Id == other.Id && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Location) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        #endregion
    }
}
