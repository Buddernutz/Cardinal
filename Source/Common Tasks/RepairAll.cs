using ff14bot.RemoteWindows;
using TreeSharp;

namespace Cardinal
{
    public class RepairAll : TimedTaskNode
    {
        protected override bool PreCondition(object context)
        {
            return Repair.IsOpen;
        }

        protected override RunStatus PostAction(object context)
        {
            try { Repair.RepairAll(); }
            catch { return RunStatus.Failure; }
            Logger.AgilMessage("Repaired everything.");

            return RunStatus.Success;
        }
    }
}