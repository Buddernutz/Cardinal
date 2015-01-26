using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using YamlDotNet.Serialization;

namespace Cardinal
{
    [Flags]
    public enum ShapeFlag
    {
        None = 0,

        Cone = 1 << 0,
        Rectangle = 1 << 1,
        Circle = 1 << 2,

        DirForward = 1 << 3,
        DirBackward = 1 << 4,
        DirLeft = 1 << 5,
        DirRight = 1 << 6,

        TargetedOnTarget = 1 << 7,
        TargetedFromCaster = 1 << 8,

        Interruptible = 1 << 9,
        NotTelegraphed = 1 << 10
    }

    [ProtoContract]
    public class SpellShape
    {
        [YamlIgnore]
        [ProtoIgnore]
        private List<ShapeFlag> flags;

        [YamlIgnore]
        [ProtoIgnore]
        private ShapeFlag combintedFlag;

        [YamlIgnore]
        [ProtoIgnore]
        public ShapeFlag CombinedFlag
        {
            get
            {
                if (combintedFlag == ShapeFlag.None && Flags.Count > 0)
                {
                    combintedFlag = GetCombinedFlag();
                }

                return combintedFlag;
            }
        }

        [ProtoMember(1)]
        public uint Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public List<ShapeFlag> Flags
        {
            get { return flags ?? (flags = new List<ShapeFlag>()); }
            set { flags = value; }
        }

        [ProtoMember(4)]
        public float Reach { get; set; }

        [ProtoMember(5)]
        public float Width { get; set; }

        [ProtoMember(6)]
        public float Duration { get; set; }

        public void MergeFlags(SpellShape other)
        {
            foreach (var flag in other.Flags.Where(f => !Flags.Contains(f))) 
            {
                Flags.Add(flag);
            }
        }

        public ShapeFlag GetCombinedFlag()
        {
            return Flags.Aggregate(ShapeFlag.None, (current, flag) => flag | current);
        }
    }
}
