using System;
using System.Runtime.Serialization;

namespace Net.ArcanaStudio.NikoSDK.Model.Commands
{
    [DataContract]
    public abstract class NikoCommandBase
    {
        [DataMember(Name = "cmd",IsRequired = true)]
        public string CommandName { get; internal set; }
        [DataMember(Name = "id",EmitDefaultValue = false)]
        public int? ID { get; internal set; }
        [DataMember(Name = "value1",EmitDefaultValue = false)]
        public int? Value { get; internal set; }

        protected NikoCommandBase(string command_name)
        {
            CommandName = command_name ?? throw new ArgumentNullException(nameof(command_name));
        }

        protected NikoCommandBase(string command_name, int id, int value)
        {
            CommandName = command_name ?? throw new ArgumentNullException(nameof(command_name));
            ID = id;
            Value = value;
        }
    }
}
