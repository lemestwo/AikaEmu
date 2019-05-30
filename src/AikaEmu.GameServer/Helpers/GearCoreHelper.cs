using System;
using System.Collections.Generic;
using System.Linq;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Item;
using AikaEmu.GameServer.Models.Item.Const;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Character.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.GameServer.Utils;

namespace AikaEmu.GameServer.Helpers
{
    public static class GearCoreHelper
    {
        public static void CoreConvert(GameConnection connection, ushort coreSlot, ushort itemSlot, IEnumerable<ushort> extractSlots)
        {
            var inventory = connection.ActiveCharacter.Inventory;
            var extractList = FormatExtractList(inventory, extractSlots);
            var coreData = inventory.GetItem(SlotType.Inventory, coreSlot);
            var itemData = inventory.GetItem(SlotType.Inventory, itemSlot);
            if (coreData == null || itemData == null) return;

            var coreConvertData = DataManager.Instance.ConvertCoreData.GetData(itemData.ItemId, (ushort) coreData.ItemData.GearCoreLevel);
            if (coreConvertData == null) return;

            var (isOk, chance) = CheckValidItems(ItemType.GearCoreConvert, itemData, coreData, extractList, coreConvertData.Chance, coreConvertData.ExtChance,
                coreConvertData.ConcExtChance);
            if (!isOk) return;

            var random = new Random();
            var result = random.Next(0, 100) <= chance;

            foreach (var (qty, item) in extractList.Values)
                inventory.RemoveItem(SlotType.Inventory, item.Slot, qty, false);
            inventory.RemoveItem(SlotType.Inventory, coreData.Slot, 1, false);
            if (result)
            {
                var newRefinement = (uint) ((itemData.Quantity >> 4) - 3) << 4;
                inventory.RemoveItem(SlotType.Inventory, itemData.Slot, 0, false);
                inventory.AddItem(SlotType.Inventory, newRefinement, coreConvertData.ResultItemId);
            }

            connection.ActiveCharacter.Save(SaveType.Inventory);
            connection.SendPacket(new CoreConvertResult(connection.Id, result));
        }

        public static void CoreUpgrade(GameConnection connection, ushort coreSlot, ushort itemSlot, IEnumerable<ushort> extractSlots)
        {
            var inventory = connection.ActiveCharacter.Inventory;
            var extractList = FormatExtractList(inventory, extractSlots);
            // Get data for core item and source item
            var coreData = inventory.GetItem(SlotType.Inventory, coreSlot);
            var itemData = inventory.GetItem(SlotType.Inventory, itemSlot);
            if (coreData == null || itemData == null) return;
            // Get core data from DataManager
            var coreUpgradeData = DataManager.Instance.GearCoresData.GetData(itemData.ItemId, (ushort) coreData.ItemData.GearCoreLevel);
            if (coreUpgradeData == null) return;

            // Check if valid items
            var (isOk, chance) = CheckValidItems(ItemType.GearCore, itemData, coreData, extractList, coreUpgradeData.Chance, coreUpgradeData.ExtChance,
                coreUpgradeData.ConcExtChance);
            if (!isOk) return;

            // Lucky stuff
            var random = new Random();
            var result = random.Next(0, 100) <= chance;

            // Inventory update
            foreach (var (qty, item) in extractList.Values)
                inventory.RemoveItem(SlotType.Inventory, item.Slot, qty, false);
            inventory.RemoveItem(SlotType.Inventory, coreData.Slot, 1, false);
            if (result)
                inventory.UpdateItem(SlotType.Inventory, itemData.Slot, coreUpgradeData.ResultItemId, false);

            // Save
            connection.ActiveCharacter.Save(SaveType.Inventory);
            connection.SendPacket(new CoreUpgradeResult(connection.Id, result));
        }

        private static (bool isOk, int chance) CheckValidItems(ItemType gearType, Item itemData, Item coreData,
            Dictionary<ushort, (byte qty, Item data)> extractList, byte baseChance, byte extChance, byte concExtChance)
        {
            if (coreData.ItemData.ItemType != gearType || itemData.Quantity < 7 << 4 || coreData.Quantity < 1) return (false, 0);
            var isWeapon = GlobalUtils.IsWeapon(itemData.ItemData.ItemType);
            var chance = (int) baseChance;
            foreach (var (qty, data) in extractList.Values)
            {
                if (isWeapon && data.ItemData.ItemType != ItemType.ExtractWeapon && data.ItemData.ItemType != ItemType.ExtractConcWeapon ||
                    !isWeapon && data.ItemData.ItemType != ItemType.ExtractArmor && data.ItemData.ItemType != ItemType.ExtractConcArmor ||
                    qty > data.Quantity) return (false, 0);
                if (data.ItemData.ItemType == ItemType.ExtractArmor || data.ItemData.ItemType == ItemType.ExtractArmor)
                    chance += extChance * qty;
                if (data.ItemData.ItemType == ItemType.ExtractConcWeapon || data.ItemData.ItemType == ItemType.ExtractConcArmor)
                    chance += concExtChance * qty;
            }

            return (true, chance);
        }

        private static Dictionary<ushort, (byte qty, Item data)> FormatExtractList(Inventory inventory, IEnumerable<ushort> extractSlots)
        {
            // Create two lists to store extract
            var uniqueList = new HashSet<ushort>();
            var repeatList = extractSlots.Where(slot => !uniqueList.Add(slot)).ToList();

            // Filter and add quantity+data to unique slots
            var extractList = new Dictionary<ushort, (byte qty, Item data)>();
            foreach (var slot in uniqueList)
                extractList.Add(slot, (1, inventory.GetItem(SlotType.Inventory, slot)));
            foreach (var slotRepeat in repeatList)
                extractList[slotRepeat] = ((byte) (extractList[slotRepeat].qty + 1), extractList[slotRepeat].data);

            return extractList;
        }
    }
}