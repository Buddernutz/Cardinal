using Clio.Utilities;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using TreeSharp;

namespace Cardinal
{
    public static class Movement
    {
        public static Composite SprintMove(ValueRetriever<Vector3> location)
        {
            var move = CommonBehaviors.MoveAndStop(location, 1f, true);
            var sprint = new Decorator(req => CanSprint,
                new Action(r =>
                {
                    Actionmanager.Sprint();
                    return RunStatus.Failure;
                }));

            return new PrioritySelector(sprint, move);
        }

        public static bool CanSprint
        {
            get
            {
                return Actionmanager.IsSprintReady && MovementManager.IsMoving && !Core.Me.IsMounted;
            }
        }
    }
}