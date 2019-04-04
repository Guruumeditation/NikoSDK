using System;
using System.Runtime.Serialization;
using Net.ArcanaStudio.NikoSDK.Interfaces;

namespace Net.ArcanaStudio.NikoSDK.Models
{
    [DataContract]
    public sealed class Action :IAction, IEquatable<IAction>
    {
        [DataMember]
        public int Id { get; }
        [DataMember]
        public string Name { get; }
        [DataMember]
        public int Type { get;}
        [DataMember]
        public int LocationId { get;}
        [DataMember]
        public int Value { get; }

        public Action(int id, string name, int type, int locationid, int value)
        {
            Id = id;
            Name = name;
            Type = type;
            LocationId = locationid;
            Value = value;
        }

        #region Equality members

        public bool Equals(IAction other)
        {
            return Id == other.Id && string.Equals(Name, other.Name) && Type == other.Type && LocationId == other.LocationId && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Action) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Type;
                hashCode = (hashCode * 397) ^ LocationId;
                hashCode = (hashCode * 397) ^ Value;
                return hashCode;
            }
        }

        #endregion
    }
}
