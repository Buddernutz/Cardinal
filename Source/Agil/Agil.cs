using ff14bot;
using ff14bot.Managers;
using TreeSharp;

namespace Cardinal
{
    public static class Agil
    {
        public static bool CanAct
        {
            get
            {
                return !Core.Me.InCombat && !Core.Me.IsDead && !QuestLogManager.InCutscene
                    && !CraftingManager.IsCrafting;
            }
        }

        public static Composite CreateBehavior(EquipmentContext equipContext, RepairContext repairContext)
        {
            var repair = repairContext.RepairEnabled ? RepairBehavior.CreateBehavior(repairContext) 
                : new Action(r => RunStatus.Failure);

            var optimize = equipContext.OptimizerEnabled ? GearOptimizer.GetBehavior(equipContext) 
                : new Action(r => RunStatus.Failure);

            var behavior = new PrioritySelector(repair, optimize);

            return new Decorator(req => CanAct, behavior);
        }
    }
}