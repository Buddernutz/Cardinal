using System;
using Clio.Utilities;
using ff14bot.Objects;

namespace Cardinal
{
    public class SpellCast
    {
        public SpellCast(BattleCharacter caster)
        {
            CasterId = caster.NpcId;
            SpellId = caster.SpellCastInfo.ActionId;
            Name = caster.SpellCastInfo.Name;
            CasterLocation = caster.Location;
            TargetLocation = caster.TargetCharacter == null ? CasterLocation : caster.TargetCharacter.Location;
            CasterHeading = caster.Heading;
            Caster = caster;
            StartTime = DateTime.Now;
            CastTime = caster.SpellCastInfo.CastTime.TotalMilliseconds;

            SpellShape shape;
            if (!Database.Shapes.TryGetValue(SpellId, out shape))
            {
                shape = new SpellShape();
                IsUnknown = true;
            }

            Shape = shape;
            TotalDuration = shape.Duration + CastTime;
        }

        public bool IsUnknown { get; set; }
        public uint CasterId { get; set; }
        public uint SpellId { get; set; }
        public Vector3 TargetLocation { get; set; }
        public SpellShape Shape { get; set; }
        public Vector3 CasterLocation { get; set; }
        public float CasterHeading { get; set; }
        public BattleCharacter Caster { get; set; }
        public string Name { get; set; }
        public bool HasAvoidanceVector { get; set; }
        public DateTime StartTime { get; set; }
        public double CastTime { get; set; }
        public double TotalDuration { get; set; }
    }
}
