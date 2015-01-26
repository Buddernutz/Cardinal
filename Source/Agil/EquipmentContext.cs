using System.Collections.Generic;
using System.Linq;
using ff14bot;
using ff14bot.Enums;

namespace Cardinal
{
    public class EquipmentContext
    {
        public readonly Dictionary<ushort, bool> SlotChecked = new Dictionary<ushort, bool>
        {
            {0, false},
            {1, false},
            {2, false},
            {3, false},
            {4, false},
            {5, false},
            {6, false},
            {7, false},
            {8, false},
            {9, false},
            {10, false},
            {11, false},
            {12, false}
        };

        public volatile bool TimerElapsed;

        public EquipmentContext(AgilSettings settings)
        {
            OptimizeBySpiritbind = settings.OptimizeBySpiritbond;
            OptimizerEnabled = settings.OptimizeGear;
            MinimumItemLevel = settings.MinimumItemLevel;
        }

        public ClassJobType Job { get; set; }
        public byte Level { get; set; }
        public bool InitialCheck { get; set; }
        public bool Optimizing { get; set; }
        public bool OptimizeBySpiritbind { get; set; }
        public bool OptimizerEnabled { get; set; }
        public byte MinimumItemLevel { get; set; }

        public void Reset()
        {
            Optimizing = false;
            InitialCheck = true;
            TimerElapsed = false;
            Level = Core.Me.ClassLevel;
            Job = Core.Me.CurrentJob;
            foreach (ushort key in SlotChecked.Keys.ToList()) { SlotChecked[key] = false; }
        }
    }
}