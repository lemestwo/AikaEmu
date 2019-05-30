using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Chat.Const;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Models.Units.Character.Const;
using AikaEmu.GameServer.Models.Units.Npc.Const;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.GameServer.Utils;

namespace AikaEmu.GameServer.Helpers
{
    public static class CraftHelper
    {
        public static void AnvilCraft(GameConnection connection, ushort itemId, uint quantity)
        {
            var craftData = DataManager.Instance.MakeItemsData.GetData(itemId);
            if (craftData == null) return;

            var character = connection.ActiveCharacter;
            var errorMsg = string.Empty;
            if (character.Level < craftData.Level)
            {
                errorMsg = "Not enough level.";
            }
            else if (character.Money < craftData.Price)
            {
                errorMsg = "Not enough money.";
            }
            else if (character.Inventory.GetFreeSlots(SlotType.Inventory) <= 0)
            {
                errorMsg = "Inventory is full.";
            }

            if (errorMsg != string.Empty)
            {
                connection.SendPacket(new SendMessage(new Message(errorMsg, MessageType.Error)));
                return;
            }

            var dicItems = new Dictionary<ushort, uint>();
            foreach (var dataIngredient in craftData.Ingredients)
            {
                var (_, itemsQty) = character.Inventory.GetItems(SlotType.Inventory, dataIngredient.ItemId);
                if (itemsQty < dataIngredient.Quantity * quantity)
                {
                    connection.SendPacket(new SendMessage(new Message("Not enough materials.", MessageType.Error)));
                    return;
                }

                dicItems.Add(dataIngredient.ItemId, dataIngredient.Quantity * quantity);
            }

            var random = new Random();
            var resultType = random.Next(0, 1000) <= craftData.Rate ? ResultCraftType.Success : ResultCraftType.Failure;
            if (resultType == ResultCraftType.Success)
            {
                var craftResultSup = random.Next(0, 1000) <= craftData.RateSup;
                if (craftResultSup) resultType = ResultCraftType.Superior;
                else
                {
                    var craftResultDouble = random.Next(0, 1000) <= craftData.RateDouble;
                    if (craftResultDouble) resultType = ResultCraftType.Double;
                }
            }

            if (!character.Inventory.RemoveItems(SlotType.Inventory, dicItems, false) || !character.UpdateMoney(craftData.Price))
            {
                GlobalUtils.InternalDisconnect(connection);
                return;
            }

            uint finalQty = 0;
            ushort finalItemId = 0;
            if (resultType != ResultCraftType.Failure)
            {
                finalQty = craftData.Quantity * quantity * (uint) (resultType == ResultCraftType.Double ? 2 : 1);
                finalItemId = resultType == ResultCraftType.Superior ? craftData.ResultSupItemId : craftData.ResultItemId;

                if (!character.Inventory.AddItem(SlotType.Inventory, finalQty, finalItemId))
                {
                    GlobalUtils.InternalDisconnect(connection);
                    return;
                }
            }

            character.Save(SaveType.Inventory);
            character.SendPacket(new SendEnchantResult(connection.Id, ActionType.Craft, finalItemId, finalQty, (uint) resultType));
        }
    }
}