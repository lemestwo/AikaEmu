using System;
using System.Collections.Generic;
using AikaEmu.AuthServer.Managers;
using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.AuthServer.Packets.GA
{
    public class ResetAuthHash : GameAuthPacket
    {
        protected override void Read(PacketStream stream)
        {
            var accountId = stream.ReadUInt32();
            if (accountId <= 0) return;

            var parameters = new Dictionary<string, object>
            {
                {"session_hash", string.Empty},
                {"session_time", DateTime.UtcNow}
            };
            DatabaseManager.Instance.UpdateAccount(accountId, parameters);
        }
    }
}