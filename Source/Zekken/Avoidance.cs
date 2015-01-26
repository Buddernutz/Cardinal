using ff14bot.Behavior;
using TreeSharp;

namespace Cardinal
{
    public static class Avoidance
    {
        public static Composite GetBehavior(AvoidanceContext context)
        {
            var reportUnknown = new Action(r => context.Capture.CaptureScreenshot(context));
            var update = new Action(r => context.Update());
            var move = CommonBehaviors.MoveAndStop(r => context.SafeLocation, 1f, true);
            var avoidCheck = new PrioritySelector(new Decorator(req => context.IsAvoiding, move),
                new Decorator(req => context.IsWaiting, new Action(r => RunStatus.Success)));

            return new Sequence(update, avoidCheck);
        }
    }
}
