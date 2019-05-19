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

namespace AikaEmu.GameServer.Helpers
{
    public static class GearCoreHelper
    {
        public static void GearConvert(GameConnection connection, ushort coreSlot, ushort itemSlot, IEnumerable<ushort> extractSlots)
        {
            var inventory = connection.ActiveCharacter.Inventory;

            // Create two lists to store extract
            var uniqueList = new HashSet<ushort>();
            var repeatList = extractSlots.Where(slot => !uniqueList.Add(slot)).ToList();

            // Filter and add quantity+data to unique slots
            var extractList = new Dictionary<ushort, (byte qty, Item data)>();
            foreach (var slot in uniqueList)
                extractList.Add(slot, (1, inventory.GetItem(SlotType.Inventory, slot)));
            foreach (var slotRepeat in repeatList)
                extractList[slotRepeat] = ((byte) (extractList[slotRepeat].qty + 1), extractList[slotRepeat].data);

            // Get data for core item and source item
            var coreData = inventory.GetItem(SlotType.Inventory, coreSlot);
            var itemData = inventory.GetItem(SlotType.Inventory, itemSlot);
            if (coreData == null || itemData == null) return;
            // Get core data from DataManager
            var coreConvertData = DataManager.Instance.GearCoresData.GetData(itemData.ItemId, (ushort) coreData.ItemData.GearCoreLevel);
            if (coreConvertData == null) return;

            // Check if valid items
            if (coreData.ItemData.ItemType != ItemType.GearCore || itemData.Quantity < 7 << 4 || coreData.Quantity < 1) return;
            var isWeapon = itemData.ItemData.ItemType > ItemType.MonsterWeapon && itemData.ItemData.ItemType <= ItemType.Staff;
            var chance = (int) coreConvertData.Chance;
            foreach (var (qty, data) in extractList.Values)
            {
                if (isWeapon && data.ItemData.ItemType != ItemType.ExtractWeapon && data.ItemData.ItemType != ItemType.ExtractConcWeapon ||
                    !isWeapon && data.ItemData.ItemType != ItemType.ExtractArmor && data.ItemData.ItemType != ItemType.ExtractConcArmor ||
                    qty > data.Quantity) return;
                if (data.ItemData.ItemType == ItemType.ExtractArmor || data.ItemData.ItemType == ItemType.ExtractArmor)
                    chance += coreConvertData.ExtChance * qty;
                if (data.ItemData.ItemType == ItemType.ExtractConcWeapon || data.ItemData.ItemType == ItemType.ExtractConcArmor)
                    chance += coreConvertData.ConcExtChance * qty;
            }

            // Lucky stuff
            var random = new Random();
            var result = random.Next(0, 100) <= chance;

            // Inventory update
            foreach (var (qty, item) in extractList.Values)
                inventory.RemoveItem(SlotType.Inventory, item.Slot, qty, false);
            inventory.RemoveItem(SlotType.Inventory, coreData.Slot, 1, false);
            if (result)
                inventory.UpdateItem(SlotType.Inventory, itemData.Slot, coreConvertData.ResultItemId, false);

            // Save
            connection.ActiveCharacter.Save(SaveType.Inventory);
            connection.SendPacket(new CoreConvertResult(connection.Id, result));
        }
    }
}