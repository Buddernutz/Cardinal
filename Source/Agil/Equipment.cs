using System.Collections.Generic;
using System.Linq;
using ff14bot;
using ff14bot.Enums;
using ff14bot.Managers;

namespace Cardinal
{
    public static class Equipment
    {
        public static Dictionary<ushort, EquipmentSlot> SlotToCategory =
            new Dictionary<ushort, EquipmentSlot> 
            {
                {0, EquipmentSlot.MainHand},
                {1, EquipmentSlot.OffHand},
                {2, EquipmentSlot.Head},
                {3, EquipmentSlot.Body},
                {4, EquipmentSlot.Hands},
                {5, EquipmentSlot.Waist},
                {6, EquipmentSlot.Legs},
                {7, EquipmentSlot.Feet},
                {8, EquipmentSlot.Necklace},
                {9, EquipmentSlot.Earring},
                {10, EquipmentSlot.Bracelet},
                {11, EquipmentSlot.Ring1},
                {12, EquipmentSlot.Ring2}
            };

        public static float GetCondition()
        {
            float condition = InventoryManager.EquippedItems
                .Where(s => s.IsFilled)
                .Select(item => item.Condition)
                .Concat(new float[] { 100 })
                .Min();

            return condition;
        }

        public static BagSlot GetSlot(Item item)
        {
            return InventoryManager.FilledInventoryAndArmory
                .Union(InventoryManager.FilledSlots)
                .Union(InventoryManager.EquippedItems)
                .First(s => s.Item == item);
        }

        public static List<BagSlot> GetUnusedEquipment(ushort slot, byte minLevel = 0)
        {
            byte level = Core.Me.ClassLevel;
            var category = SlotToCategory[slot];

            return InventoryManager.FilledInventoryAndArmory
                .Union(InventoryManager.FilledSlots)
                .Where(s => s.Item.IsValidForCurrentClass)
                .Where(s => s.Item.IsArmor || s.Item.IsWeapon)
                .Where(s => s.Item.EquipmentSlots.CanTake.Contains(category))
                .Where(s => s.Item.ItemLevel >= minLevel)
                .Where(s => !InventoryManager.EquippedItems.Contains(s))
                .Where(s => s.Item.RequiredLevel <= level)
                .ToList();
        }

        public static List<BagSlot> GetAllEquipmentFor(ushort slot, byte minLevel = 0)
        {
            byte level = Core.Me.ClassLevel;
            var category = SlotToCategory[slot];

            return InventoryManager.FilledInventoryAndArmory
                .Union(InventoryManager.FilledSlots)
                .Where(s => s.Item.IsValidForCurrentClass)
                .Where(s => s.Item.IsArmor || s.Item.IsWeapon)
                .Where(s => s.Item.EquipmentSlots.CanTake.Contains(category))
                .Where(s => s.Item.ItemLevel >= minLevel)
                .Where(s => s.Item.RequiredLevel <= level)
                .ToList();
        }

        public static List<BagSlot> GetAllEquipment(byte minLevel = 0)
        {
            byte level = Core.Me.ClassLevel;

            return InventoryManager.FilledInventoryAndArmory
                .Union(InventoryManager.FilledSlots)
                .Where(s => s.Item.IsValidForCurrentClass)
                .Where(s => s.Item.IsArmor || s.Item.IsWeapon)
                .Where(s => s.Item.ItemLevel >= minLevel)
                .Where(s => s.Item.RequiredLevel <= level)
                .ToList();
        }

        public static BagSlot GetEquipmentSlot(ushort slotId)
        {
            return InventoryManager.EquippedItems
                .First(s => s.Slot == slotId);
        }

        public static BagSlot GetSlot(ushort slotId)
        {
            return InventoryManager.FilledInventoryAndArmory
                .Union(InventoryManager.FilledSlots)
                .First(s => s.Slot == slotId);
        }
    }
}