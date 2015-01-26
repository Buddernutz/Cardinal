using System.Diagnostics;
using TreeSharp;

namespace Cardinal
{
    public class TimedTaskNode : Action
    {
        private Stopwatch stopwatch = new Stopwatch();
        public int Time = 500;

        protected override RunStatus Run(object context)
        {
            if (!PreCondition(context)) { return RunStatus.Failure; }
            return stopwatch.ElapsedMilliseconds < Time ? RunStatus.Running : PostAction(context);
        }

        public override void Start(object context)
        {
            stopwatch.Start();
            base.Start(context);
        }

        public override void Stop(object context)
        {
            stopwatch.Reset();
            base.Stop(context);
        }

        protected virtual bool PreCondition(object context)
        {
            return true;
        }

        protected virtual RunStatus PostAction(object context)
        {
            return RunStatus.Success;
        }
    }
}
