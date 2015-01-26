using System.Linq;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.RemoteWindows;
using TreeSharp;

namespace Cardinal
{
    public static class RepairBehavior
    {
        private static bool IsDoingFate
        {
            get
            {
                return (BotManager.Current.Name == "Fate Bot")
                    && (FateManager.WithinFate || Poi.Current.Type == PoiType.Fate);
            }
        }

        public static Composite CreateBehavior(RepairContext context)
        {
            var changeContext = new Decorator(req => WorldManager.ZoneId != context.Mender.ZoneId,
                new Action(r => ChangeContext(context)));

            var repair = new Decorator(req => ShouldRepair(context, context.MinimumDurability) && CanRepair(context),
                RepairProcedure(context));

            return new PrioritySelector(changeContext, repair);
        }

        public static RunStatus ChangeContext(RepairContext context)
        {
            context.State = RepairState.None;
            ushort zoneId = WorldManager.ZoneId;

            var mender = Database.Menders
                .Where(m => m.ZoneId == zoneId)
                .OrderBy(m => m.Location.Distance(Core.Me.Location))
                .FirstOrDefault(m => m.ZoneId == zoneId);

            if (mender == null) { return RunStatus.Failure; }
            if (context.Mender != null && mender.NpcId == context.Mender.NpcId) 
            {
                return RunStatus.Failure; 
            }

            context.Mender = mender;
            Logger.AgilMessage("Changed mender to: {0}", mender);

            return RunStatus.Failure;
        }

        private static Composite RepairProcedure(RepairContext context)
        {
            var initialStep = GetInitialStep(context);
            var returnArg = GetReturnStep(context);
            var repairStep = GetRepairStep(context);

            return new Switch<RepairState>(r => context.State, initialStep, repairStep, returnArg);
        }

        private static SwitchArgument<RepairState> GetReturnStep(RepairContext context)
        {
            var finalize = new Action(r =>
            {
                context.State = RepairState.None;
                Logger.AgilMessage("Finished moving back.");
                return RunStatus.Success;
            });

            var move = Movement.SprintMove(r => context.Location);

            var returnStep = new PrioritySelector(move, finalize,
                new Action(r => RunStatus.Success));

            return new SwitchArgument<RepairState>(returnStep, RepairState.MovingBack);
        }

        private static SwitchArgument<RepairState> GetRepairStep(RepairContext context)
        {
            var closeAction = new Action(r =>
            {
                Repair.Close();
                context.State = BotManager.Current.Name != "Fate Bot"
                    ? RepairState.MovingBack : RepairState.None;

                Logger.AgilMessage("Ended repair step, now on: {0}", context.State);
            });

            var close = new Decorator(req => Equipment.GetCondition() >= context.MinimumDurability && Repair.IsOpen, 
                closeAction);

            var dismount = new Decorator(req => Core.Me.IsMounted,
                new Action(r => Actionmanager.Dismount()));

            var selectYes = new Decorator(req => SelectYesno.IsOpen,
                new Action(r => SelectYesno.ClickYes()));

            var move = Movement.SprintMove(r => context.Location);
            var repairAll = new RepairAll();
            var selectRepair = new SetIconString(2);
            var setTarget = new SetTarget(() => context.NpcId);

            var interact = new Decorator(req => !Repair.IsOpen && !SelectIconString.IsOpen && !SelectYesno.IsOpen,
                new Interact());

            var repairStep = new PrioritySelector(move, dismount, close, selectYes, repairAll, selectRepair,
                setTarget, interact, new Action(r => RunStatus.Success));

            return new SwitchArgument<RepairState>(repairStep, RepairState.Repairing);
        }

        private static SwitchArgument<RepairState> GetInitialStep(RepairContext context)
        {
            var initStep = new Action(r =>
            {
                context.State = RepairState.Repairing;
                context.InitialLocation = Core.Me.Location;
                Logger.AgilMessage("Going to repair.");
                return RunStatus.Success;
            });

            return new SwitchArgument<RepairState>(initStep, RepairState.None);
        }

        private static bool CanRepair(RepairContext context)
        {
            return !GatheringWindow.WindowOpen && (!IsDoingFate || (IsDoingFate && context.State != RepairState.None))
                   && WorldManager.ZoneId == context.Mender.ZoneId;
        }

        private static bool ShouldRepair(RepairContext context, float condition)
        {
            return context.State != RepairState.None 
                || Equipment.GetCondition() < condition;
        }
    }
}