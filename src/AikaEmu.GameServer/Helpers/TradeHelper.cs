using System.Collections.Generic;
using System.Linq;
using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Models.Item;
using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;

namespace AikaEmu.GameServer.Helpers
{
    public static class TradeHelper
    {
        public static void TradeRequest(GameConnection connection, ushort targetConId)
        {
            var myTradeData = TradeManager.Instance.GetTrade(connection.Id);
            if (myTradeData != null) return;

            var target = WorldManager.Instance.GetCharacter(targetConId);
            if (target == null)
            {
                connection.SendPacket(new SendMessage(new Message("Target is not available or offline.")));
                return;
            }

            var isTrading = TradeManager.Instance.GetTrade(target.Connection.Id);
            if (isTrading != null)
            {
                connection.SendPacket(new SendMessage(new Message("Target is already in trade.")));
                return;
            }

            if (TradeManager.Instance.AddTradeRequest(connection.Id, target.Connection.Id))
                target.Connection.SendPacket(new SendTradeRequest(target.Connection.Id, connection.Id));
        }

        public static void TradeRequestResult(GameConnection connection, ushort conId, bool result)
        {
            var tradeOwner = TradeManager.Instance.GetOwnerRequest(connection.Id);
            if (tradeOwner != conId) return;

            TradeManager.Instance.RemoveTradeRequest(connection.Id);
            var owner = WorldManager.Instance.GetCharacter(tradeOwner);
            if (owner == null) return;
            
            if (!result)
            {
                owner.SendPacket(new SendMessage(new Message("Target refused your trade invitation.")));
                return;
            }

            owner.SendPacket(new SendTradeResult(connection.Id)); // TODO - Maybe its sent to target not owner

            var trade = new Trade
            {
                Id = IdTradeManager.Instance.GetNextId(),
                OwnerConId = owner.Connection.Id,
                TargetConId = connection.Id
            };
            TradeManager.Instance.AddTrade(trade);
            owner.SendPacket(new SendTradeData(owner.Connection.Id, trade, false));
            connection.SendPacket(new SendTradeData(connection.Id, trade, true));
        }

        public static void TradeRequestCancel(GameConnection connection)
        {
            connection.SendPacket(new SendCancelTrade());

            var tradeData = TradeManager.Instance.GetTrade(connection.Id);
            if (tradeData == null) return;

            TradeManager.Instance.RemoveTrade(tradeData.Id);
            var isOwner = tradeData.OwnerConId == connection.Id;
            var otherChar = WorldManager.Instance.GetCharacter(isOwner ? tradeData.TargetConId : tradeData.OwnerConId);
            otherChar?.SendPacket(new SendCancelTrade());
            otherChar?.SendPacket(new SendCancelTrade(connection.Id));
        }

        public static void TradeUpdateData(GameConnection connection, Dictionary<byte, ushort> itemsSlot, ulong money, bool ok, bool confirm, ushort conId)
        {
            // Get tradeData and check if exists
            var tradeData = TradeManager.Instance.GetTrade(connection.Id);
            if (tradeData == null)
            {
                connection.SendPacket(new SendCancelTrade());
                return;
            }

            // Check if packet was from the trade owner or target
            var isOwner = tradeData.OwnerConId == connection.Id;
            // Get info from both
            var owner = isOwner ? connection.ActiveCharacter : WorldManager.Instance.GetCharacter(tradeData.OwnerConId);
            var target = !isOwner ? connection.ActiveCharacter : WorldManager.Instance.GetCharacter(tradeData.TargetConId);

            // Check if online
            if (owner == null || target == null) TradeCancel(owner, target, tradeData.Id);
            // Check packet data from correct connectionId
            if (isOwner && tradeData.TargetConId != conId || !isOwner && tradeData.OwnerConId != conId) TradeCancel(owner, target, tradeData.Id);
            // Check enough money in inventory
            if (money > connection.ActiveCharacter.Money) TradeCancel(owner, target, tradeData.Id);

            // Check if items/buttons changed and reset
            if (IsItemsChanged(isOwner ? tradeData.OwnerSlots : tradeData.TargetSlots, itemsSlot) || isOwner && tradeData.OwnerOk && !ok ||
                !isOwner && tradeData.TargetOk && !ok)
            {
                tradeData.OwnerOk = false;
                tradeData.TargetOk = false;
                tradeData.OwnerConfirm = false;
                tradeData.TargetConfirm = false;
                ok = false;
                confirm = false;
            }

            if (isOwner)
            {
                tradeData.OwnerItems.Clear();
                tradeData.OwnerSlots = itemsSlot;
                tradeData.OwnerItems = GetItemsData(owner, itemsSlot);
                if (tradeData.OwnerItems.Count != itemsSlot.Count) TradeCancel(owner, target, tradeData.Id);
                tradeData.OwnerMoney = money;
                tradeData.OwnerOk = ok;
                tradeData.OwnerConfirm = confirm;
                target?.SendPacket(new SendTradeData(target.Connection.Id, tradeData, true));
            }
            else
            {
                tradeData.TargetItems.Clear();
                tradeData.TargetSlots = itemsSlot;
                tradeData.TargetItems = GetItemsData(target, itemsSlot);
                if (tradeData.TargetItems.Count != itemsSlot.Count) TradeCancel(owner, target, tradeData.Id);
                tradeData.TargetMoney = money;
                tradeData.TargetOk = ok;
                tradeData.TargetConfirm = confirm;
                owner?.SendPacket(new SendTradeData(owner.Connection.Id, tradeData, false));
            }

            if (tradeData.OwnerConfirm && tradeData.TargetConfirm)
            {
                // TODO - TRADE ITEMS
            }
        }

        private static void TradeCancel(Character owner, Character target, uint tradeId)
        {
            owner?.SendPacket(new SendCancelTrade());
            target?.SendPacket(new SendCancelTrade());
            TradeManager.Instance.RemoveTrade(tradeId);
        }

        private static bool IsItemsChanged(Dictionary<byte, ushort> oldSlots, Dictionary<byte, ushort> newSlots)
        {
            return !(oldSlots.Keys.Count == newSlots.Keys.Count && oldSlots.Keys.All(k => newSlots.ContainsKey(k) && newSlots[k] == oldSlots[k]));
        }

        private static Dictionary<byte, Item> GetItemsData(Character character, Dictionary<byte, ushort> slotList)
        {
            var list = new Dictionary<byte, Item>();
            foreach (var (key, slot) in slotList)
            {
                var item = character?.Inventory.GetItem(SlotType.Inventory, slot);
                if (item != null && item.ItemData.Tradeable)
                {
                    list.Add(key, item);
                }
            }

            return list;
        }
    }
}