using System;
using Clio.Utilities;
using ProtoBuf;
using YamlDotNet.Serialization;

namespace Cardinal
{
    [ProtoContract]
    public class Mender
    {
        private Vector3 location = Vector3.Zero;

        [ProtoMember(1)]
        public string NpcName { get; set; }

        [ProtoMember(2)]
        public uint ZoneId { get; set; }

        [YamlIgnore]
        [ProtoMember(3)]
        public Vector3 Location { get; set; }

        [ProtoMember(4)]
        public uint NpcId { get; set; }

        [ProtoIgnore]
        public float X
        {
            get { return Location.X; }
            set { Location = new Vector3(value, Location.Y, Location.Z); }
        }

        [ProtoIgnore]
        public float Y
        {
            get { return Location.Y; }
            set { Location = new Vector3(Location.X, value, Location.Z); }
        }

        [ProtoIgnore]
        public float Z
        {
            get { return Location.Z; }
            set { Location = new Vector3(Location.X, Location.Y, value); }
        }

        public override string ToString()
        {
            return String.Format("NpcId: {0} ZoneId: {1} XYZ: {2}", NpcId, ZoneId, Location);
        }
    }
}