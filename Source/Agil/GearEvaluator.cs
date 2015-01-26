using System.Collections.Generic;
using System.Linq;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;

namespace Cardinal
{
    public static class GearEvaluator
    {
        public static float Score(this Item item)
        {
            var job = Core.Me.CurrentJob;
            return Score(item, job);
        }

        public static float Score(this Item item, ClassJobType job)
        {
            var statWeights = Database.StatWeights.FirstOrDefault(sw => sw.Job == job);
            return statWeights == null ? 0f : Score(item, statWeights);
        }

        public static float Score(this Item item, StatWeights statWeights)
        {
            float value = 0f;
            foreach (var attribute in item.Attributes)
            {
                float weight;
                if (!statWeights.Weights.TryGetValue(attribute.Key, out weight)) { continue; }
                value += weight * attribute.Value;
            }

            return value;
        }

        public static bool IsBetterThan(this Item item1, Item item2)
        {
            return item1.Score() > item2.Score();
        }

        public static List<BagSlot> GetProspects(EquipmentContext context, ushort slotId)
        {
            return context.OptimizeBySpiritbind
                ? GetProspectsBySpiritbond(context, slotId) : GetProspectsByScore(context, slotId);
        }

        private static List<BagSlot> GetProspectsBySpiritbond(EquipmentContext context, ushort slot)
        {
            byte minLevel = context.MinimumItemLevel;
            var statWeights = Database.StatWeights.First(sw => sw.Job == Core.Me.CurrentJob);

            var prospects = Equipment.GetAllEquipmentFor(slot, minLevel)
                .OrderBy(s => s.SpiritBond)
                .ThenByDescending(s => s.Item.Score(statWeights))
                .ThenByDescending(i => i.Item.ItemLevel)
                .ToList();

            return prospects;
        }

        private static List<BagSlot> GetProspectsByScore(EquipmentContext context, ushort slot)
        {
            byte minLevel = context.MinimumItemLevel;
            var statWeights = Database.StatWeights.First(sw => sw.Job == Core.Me.CurrentJob);

            var prospects = Equipment.GetAllEquipmentFor(slot, minLevel)
                .OrderByDescending(s => s.Item.Score(statWeights))
                .ThenByDescending(i => i.Item.ItemLevel)
                .ToList();

            return prospects;
        }
    }
}