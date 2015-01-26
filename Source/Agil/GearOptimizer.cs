using System.Linq;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.RemoteWindows;
using TreeSharp;

namespace Cardinal
{
    public class GearOptimizer
    {
        private static bool CanOptimize
        {
            get
            {
                return !Repair.IsOpen && !SelectIconString.IsOpen
                    && !SelectString.IsOpen && !SelectYesno.IsOpen && !GatheringWindow.WindowOpen
                    && !FateManager.WithinFate && Poi.Current.Type != PoiType.Fate;
            }
        }

        public static Composite GetBehavior(EquipmentContext context)
        {
            var startup = new Decorator(req => !context.Optimizing, new Action(r => context.Optimizing = true));
            var cleanup = new Action(r => context.Reset());

            var optimize = new PrioritySelector(startup, EquipWeapon(context), EquipArmor(context, 2), EquipArmor(context, 7),
                EquipArmor(context, 3), EquipArmor(context, 4), EquipArmor(context, 5), EquipRing(context, 11, 12),
                EquipArmor(context, 6), EquipArmor(context, 8), EquipArmor(context, 9), EquipRing(context, 12, 11),
                EquipArmor(context, 10), EquipOffhand(context), cleanup);

            return new Decorator(req => CanOptimize && NeedsOptimization(context), optimize);
        }

        public static Composite EquipArmor(EquipmentContext context, ushort slot)
        {
            var equip = new Action(r => EquipOptimalArmor(context, slot));
            return new Decorator(req => !context.SlotChecked[slot], equip);
        }

        public static Composite EquipWeapon(EquipmentContext context)
        {
            var equip = new Action(r => EquipOptimalMainHand(context));
            return new Decorator(req => !context.SlotChecked[0], equip);
        }

        public static Composite EquipOffhand(EquipmentContext context)
        {
            var equip = new Action(r => EquipOptimalOffhand(context));
            return new Decorator(req => !context.SlotChecked[1], equip);
        }

        public static Composite EquipRing(EquipmentContext context, ushort slot, ushort other)
        {
            var equip = new Action(r => EquipOptimalRing(context, slot, other));
            return new Decorator(req => !context.SlotChecked[slot], equip);
        }

        public static RunStatus EquipOptimalArmor(EquipmentContext context, ushort slot)
        {
            var prospects = GearEvaluator.GetProspects(context, slot);
            var current = Equipment.GetEquipmentSlot(slot);
            var prospect = prospects.FirstOrDefault();

            if (prospect == null || prospect.Slot == slot)
            {
                context.SlotChecked[slot] = true;
                return RunStatus.Success;
            }

            if (context.OptimizeBySpiritbind && prospect.SpiritBond >= 100f)
            {
                if (current.Item.ItemLevel < context.MinimumItemLevel) { EquipItem(current, prospect); }
                
                context.SlotChecked[slot] = true;
                return RunStatus.Success;
            }

            EquipItem(current, prospect);
            context.SlotChecked[slot] = true;
            return RunStatus.Success;
        }

        public static RunStatus EquipOptimalRing(EquipmentContext context, ushort slot, ushort other)
        {
            var prospects = GearEvaluator.GetProspects(context, slot)
                .Where(s => s.Slot != other);

            var current = Equipment.GetEquipmentSlot(slot);
            var prospect = prospects.FirstOrDefault();

            if (prospect == null || prospect.Slot == slot)
            {
                context.SlotChecked[slot] = true;
                return RunStatus.Success;
            }

            if (context.OptimizeBySpiritbind && prospect.SpiritBond >= 100f)
            {
                if (current.Item.ItemLevel < context.MinimumItemLevel) { EquipItem(current, prospect); }
                
                context.SlotChecked[slot] = true;
                return RunStatus.Success;
            }

            EquipItem(current, prospect);
            context.SlotChecked[slot] = true;
            return RunStatus.Success;
        }

        private static RunStatus EquipOptimalMainHand(EquipmentContext context)
        {
            var statWeights = Database.StatWeights.First(sw => sw.Job == Core.Me.CurrentJob);
            var mainHands = GearEvaluator.GetProspects(context, 0);

            var primaries = mainHands
                    .Where(s => s.Item.EquipmentCatagory == statWeights.Primary)
                    .ToList();

            var alternatives = mainHands
                    .Where(s => s.Item.EquipmentCatagory == statWeights.Primary2H)
                    .ToList();

            var secondaries = GearEvaluator.GetProspects(context, 1);

            var bestOneHand = primaries.FirstOrDefault();
            var bestOffhand = secondaries.FirstOrDefault();
            var bestTwoHand = alternatives.FirstOrDefault();

            float oneHandScore = bestOneHand == null ? 0f : bestOneHand.Item.Score();
            float offhandScore = bestOffhand == null ? 0f : bestOffhand.Item.Score();
            float twoHandScore = bestTwoHand == null ? 0f : bestTwoHand.Item.Score();
            float oneHandTotal = oneHandScore + offhandScore;

            var current = Equipment.GetEquipmentSlot(0);
            if (oneHandTotal > twoHandScore && bestOneHand != null)
            {
                if (bestOneHand.Slot != 0) { EquipItem(current, bestOneHand); }
            }
            else if (bestTwoHand != null && bestTwoHand.Slot != 0)
            {
                EquipItem(current, bestTwoHand);
            }

            context.SlotChecked[0] = true;
            return RunStatus.Success;
        }

        private static RunStatus EquipOptimalOffhand(EquipmentContext context)
        {
            var statWeights = Database.StatWeights.First(sw => sw.Job == Core.Me.CurrentJob);
            var mainHand = Equipment.GetEquipmentSlot(0);

            if (mainHand.Item.EquipmentCatagory == statWeights.Primary) { return EquipOptimalArmor(context, 1); }
            context.SlotChecked[1] = true;

            return RunStatus.Success;
        }

        private static bool NeedsOptimization(EquipmentContext context)
        {
            return context.Optimizing
                || context.Level != Core.Me.ClassLevel
                || context.Job != Core.Me.CurrentJob
                || !context.InitialCheck
                || context.TimerElapsed;
        }

        private static void EquipItem(BagSlot current, BagSlot prospect)
        {
            var item = prospect.Item;
            prospect.Move(current);

            Logger.AgilMessage("Equipped {0} on slot {2}.", item.EnglishName,
                prospect.Slot, Equipment.SlotToCategory[current.Slot]);
        }
    }
}