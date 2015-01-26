using Clio.Common;
using Clio.Utilities;
using ff14bot;
using ff14bot.Helpers;

namespace Cardinal
{
    public struct AvoidanceVector
    {
        public bool FinishOnDistance;
        public float Heading;
        public float Magnitude;
        public Vector3 TargetLocation;

        public AvoidanceVector(float heading, float magnitude, bool finishOnDistance)
        {
            Heading = heading;
            Magnitude = magnitude;
            FinishOnDistance = finishOnDistance;
            TargetLocation = MathEx.GetPointAt(Core.Me.Location, magnitude, heading);
        }

        public bool IsActive
        {
            get { return Magnitude > 0.5f; }
        }

        public static AvoidanceVector Zero
        {
            get { return new AvoidanceVector(0f, 0f, true); }
        }

        public AvoidanceVector GetUpdatedVector()
        {
            if (!IsActive) { return Zero; }

            var myLocation = Core.Me.Location;
            float heading = MathEx.NormalizeRadian(MathHelper.CalculateHeading(TargetLocation,
                myLocation));

            float magnitude = myLocation.Distance(TargetLocation);

            return new AvoidanceVector(heading, magnitude, FinishOnDistance);
        }

        public override string ToString()
        {
            return string.Format("Magnitude: {0} Heading: {1}", Magnitude, Heading);
        }
    }
}
