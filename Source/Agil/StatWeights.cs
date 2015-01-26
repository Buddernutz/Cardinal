using System.Collections.Generic;
using ff14bot.Enums;
using ProtoBuf;

namespace Cardinal
{
    [ProtoContract]
    public class StatWeights
    {
        private Dictionary<ItemAttribute, float> weights;

        [ProtoMember(1)]
        public ClassJobType Job { get; set; }

        [ProtoMember(2)]
        public ItemUiCategory Primary { get; set; }

        [ProtoMember(3)]
        public ItemUiCategory Primary2H { get; set; }

        [ProtoMember(4)]
        public Dictionary<ItemAttribute, float> Weights
        {
            get { return weights ?? (weights = new Dictionary<ItemAttribute, float>()); }
            set { weights = value; }
        }
    }
}