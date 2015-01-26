using ff14bot;
using TreeSharp;

namespace Cardinal
{
    public class Interact : Action
    {
        private const float RANGE = 7f;

        protected override RunStatus Run(object context)
        {
            var target = Core.Me.CurrentTarget;
            if (target == null || target.Distance(Core.Me.Location) > RANGE) { return RunStatus.Failure; }

            target.Interact();
            return RunStatus.Success;
        }
    }
}
