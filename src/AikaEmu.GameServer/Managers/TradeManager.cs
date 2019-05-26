using System;
using System.Collections.Generic;
using AikaEmu.GameServer.Models;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Managers
{
    public class TradeManager : Singleton<TradeManager>
    {
        private readonly Dictionary<uint, Trade> _activeTrades; // tradeId, Trade
        private readonly Dictionary<ushort, (ushort ownerConId, DateTime Time)> _tradeRequests; // targetConId, ()

        public TradeManager()
        {
            _activeTrades = new Dictionary<uint, Trade>();
            _tradeRequests = new Dictionary<ushort, (ushort ownerConId, DateTime Time)>();
        }

        public bool AddTradeRequest(ushort ownerConId, ushort targetConId)
        {
            if (_tradeRequests.ContainsKey(targetConId)) return false;
            _tradeRequests.Add(targetConId, (ownerConId, DateTime.UtcNow.AddMinutes(2)));
            return true;
        }

        public void RemoveTradeRequest(ushort targetConId)
        {
            if (_tradeRequests.ContainsKey(targetConId)) _tradeRequests.Remove(targetConId);
        }

        public bool AddTrade(Trade trade)
        {
            if (_activeTrades.ContainsKey(trade.Id)) return false;

            _activeTrades.Add(trade.Id, trade);
            return true;
        }

        public ushort GetOwnerRequest(ushort targetConId)
        {
            foreach (var (key, (ownerConId, time)) in _tradeRequests)
            {
                if (time < DateTime.UtcNow)
                {
                    RemoveTradeRequest(key);
                    continue;
                }

                if (key == targetConId) return ownerConId;
            }

            return ushort.MaxValue;
        }

        public Trade GetTrade(ushort conId)
        {
            foreach (var trade in _activeTrades.Values)
            {
                if (trade.OwnerConId == conId || trade.TargetConId == conId) return trade;
            }

            return null;
        }

        public void RemoveTrade(uint id)
        {
            if (_activeTrades.ContainsKey(id)) _activeTrades.Remove(id);
        }
    }
}