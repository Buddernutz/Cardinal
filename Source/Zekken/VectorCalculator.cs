using System;
using ff14bot;

namespace Cardinal
{
    public static class VectorCalculator
    {
        private const ShapeFlag CIRCLE_ON_CASTER = ShapeFlag.Circle | ShapeFlag.TargetedFromCaster;

        public static AvoidanceVector GetAvoidanceVector(SpellCast cast)
        {
            var shape = cast.Shape;
            var flag = shape.CombinedFlag;
            var myLocation = Core.Me.Location;

            if (flag.HasFlag(ShapeFlag.NotTelegraphed)) { return AvoidanceVector.Zero; }

            var originPoint = flag.HasFlag(ShapeFlag.TargetedOnTarget) ? cast.TargetLocation : cast.CasterLocation;
            float distanceToCaster = myLocation.Distance(originPoint);
            float effectiveReach = shape.Reach - distanceToCaster + 1.5f;
            if (effectiveReach <= 0) { return AvoidanceVector.Zero; }

            if (flag.HasFlag(ShapeFlag.TargetedOnTarget) || flag.HasFlag(CIRCLE_ON_CASTER))
            {
                return VectorTools.GetOppositeVector(myLocation,
                    originPoint, effectiveReach);
            }

            if (!flag.HasFlag(ShapeFlag.TargetedFromCaster)) { return AvoidanceVector.Zero; }

            bool isFacingMe = VectorTools.IsFacing(originPoint, cast.CasterHeading, myLocation);
            if (flag.HasFlag(ShapeFlag.DirForward) && !isFacingMe) { return AvoidanceVector.Zero; }

            return distanceToCaster < 7f ? VectorTools.GetStraightVector(myLocation, originPoint, Math.Max(8f, distanceToCaster * 2)) 
                : VectorTools.GetLateralVector(myLocation, originPoint, effectiveReach);
        }
    }
}
