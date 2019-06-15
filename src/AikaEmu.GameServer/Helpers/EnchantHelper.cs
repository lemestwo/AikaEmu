using System;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Chat.Const;
using AikaEmu.GameServer.Models.Item.Const;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Character.Const;
using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.GameServer.Utils;
using NLog;

namespace AikaEmu.GameServer.Helpers
{
    public static class EnchantHelper
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static void Reinforcement(GameConnection connection, byte slotEquip, byte slotReagent, byte slotCash)
        {
            var character = connection.ActiveCharacter;
            var equipData = character.Inventory.GetItem(SlotType.Inventory, slotEquip);
            var equipReagent = character.Inventory.GetItem(SlotType.Inventory, slotReagent);
            var equipCash = slotCash == byte.MaxValue ? null : character.Inventory.GetItem(SlotType.Inventory, slotCash);
            if (equipData == null || !equipData.ItemData.Reinforceable || equipReagent == null || slotCash < byte.MaxValue && equipCash == null)
            {
                return;
            }

            var actualEnchant = (byte) ((equipData.Quantity >> 4) & 0xFF);
            var isWeapon = GlobalUtils.IsWeapon(equipData.ItemData.ItemType);
            var enchantData = isWeapon
                ? DataManager.Instance.ReinforceWData.GetData((ushort) equipData.ItemData.GearCoreLevel)
                : DataManager.Instance.ReinforceAData.GetData((ushort) equipData.ItemData.GearCoreLevel);

            if (enchantData == null || actualEnchant >= DataManager.Instance.CharacterData.Data.MaxEnchant) return;

            if (character.Money < enchantData.Price ||
                equipReagent.Quantity < enchantData.ReagentQty ||
                equipReagent.ItemData.Rank < equipData.ItemData.Rank ||
                equipCash != null && (
                    isWeapon && equipCash.ItemData.ItemType != ItemType.ExtractWeapon && equipCash.ItemData.ItemType != ItemType.ExtractConcWeapon ||
                    !isWeapon && equipCash.ItemData.ItemType != ItemType.ExtractArmor && equipCash.ItemData.ItemType != ItemType.ExtractConcArmor ||
                    equipCash.ItemData.Rank < equipData.ItemData.Rank
                ))
            {
                connection.SendPacket(new SendMessage(new Message("Not enough materials.", MessageType.Error)));
                return;
            }

            if (!character.Inventory.RemoveItem(SlotType.Inventory, equipReagent.Slot, enchantData.ReagentQty, false) ||
                equipCash != null && !character.Inventory.RemoveItem(SlotType.Inventory, equipCash.Slot, 1, false) ||
                !character.UpdateMoney(enchantData.Price))
            {
                GlobalUtils.InternalDisconnect(connection);
                return;
            }

            var random = new Random();
            var nResult = random.Next(0, 1000);
            var result = nResult <= enchantData.Chance[actualEnchant] + (equipCash?.ItemData.Unk52 ?? 0)
                ? ResultEnchantType.Success
                : ResultEnchantType.Failure;

            if (result == ResultEnchantType.Failure)
            {
                var result2 = random.Next(0, 1000);
                if (result2 <= 333) result = ResultEnchantType.Failure;
                else if (result2 > 333 && result2 <= 666) result = ResultEnchantType.FailureNoDestruction;
                else result = ResultEnchantType.MajorFailure;

                if (result == ResultEnchantType.MajorFailure && equipCash != null) result = ResultEnchantType.FailureNoDestruction;
                if (equipCash != null && result == ResultEnchantType.Failure && (
                        equipCash.ItemData.ItemType != ItemType.ExtractConcWeapon || equipCash.ItemData.ItemType != ItemType.ExtractConcArmor
                    ))
                    result = ResultEnchantType.FailureNoDestruction;
            }

            switch (result)
            {
                case ResultEnchantType.MajorFailure:
                    character.Inventory.RemoveItem(SlotType.Inventory, equipData.Slot, 0, false);
                    break;
                case ResultEnchantType.Failure:
                    character.Inventory.UpdateItem(SlotType.Inventory, equipData.Slot, (byte) ((actualEnchant - 1) << 4), false);
                    break;
                case ResultEnchantType.Success:
                    character.Inventory.UpdateItem(SlotType.Inventory, equipData.Slot, (byte) ((actualEnchant + 1) << 4), false);
                    break;
                case ResultEnchantType.FailureNoDestruction:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            character.Save(SaveType.Inventory);
            connection.SendPacket(result == ResultEnchantType.MajorFailure
                ? new SendEnchantResult(connection.Id, ActionType.Reinforcement, (uint) result, (uint) equipData.Slot)
                : new SendEnchantResult(connection.Id, ActionType.Reinforcement, (uint) result));

            Log.Debug("Reinforcement result: {0}, Dice: {1}", result, nResult);
        }
    }
}