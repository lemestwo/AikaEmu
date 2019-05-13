using System.Collections.Generic;
using AikaEmu.GameServer.Managers.Id;
using AikaEmu.GameServer.Models;
using AikaEmu.Shared.Utils;

namespace AikaEmu.GameServer.Managers
{
    public class TradeManager : Singleton<TradeManager>
    {
        private readonly Dictionary<uint, Trade> _activeTrades; // tradeId, Trade

        public TradeManager()
        {
            _activeTrades = new Dictionary<uint, Trade>();
        }

        public bool StartTrade(ushort ownerConId, ushort targetConId)
        {
            if (IsTrading(ownerConId) || IsTrading(targetConId)) return false;
            var owner = WorldManager.Instance.GetCharacter(ownerConId);
            var target = WorldManager.Instance.GetCharacter(targetConId);
            if (owner == null || target == null) return false;

            var temp = new Trade
            {
                Id = IdTradeManager.Instance.GetNextId(),
                Owner = owner,
                Target = target
            };
            return _activeTrades.TryAdd(temp.Id, temp);
        }

        public void EndTrade(uint id)
        {
            if (_activeTrades.ContainsKey(id)) _activeTrades.Remove(id);
        }

        public bool IsTrading(ushort conId)
        {
            foreach (var (_, trade) in _activeTrades)
            {
                if (trade.Owner.Connection?.Id == conId || trade.Target.Connection?.Id == conId) return true;
            }

            return false;
        }
    }
}