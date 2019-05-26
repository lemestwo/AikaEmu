using System;
using System.Collections.Generic;
using AikaEmu.AuthServer.Managers;
using AikaEmu.AuthServer.Managers.Id;
using AikaEmu.AuthServer.Network.AuthServer;
using AikaEmu.AuthServer.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.AuthServer.Packets.Client
{
    public class AuthAccount : AuthPacket
    {
        protected override void Read(PacketStream stream)
        {
            var user = stream.ReadString(32).Trim();
            var hash = stream.ReadString(32);
            // stream.ReadBytes(994);
            // stream.ReadByte(); // 1 ?
            // then 45 more empty bytes

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(hash))
            {
                Log.Error("Login information is empty.");
                return;
            }

            var resultAuth = DatabaseManager.Instance.AuthAccount(user, hash);
            if (resultAuth == null)
            {
                Connection.SendPacket(new AuthResult(uint.MaxValue, 0));
                Connection.Close();
                return;
            }

            // TODO - GENERATE KEY TO AUTH-GAME
            var generatedKey = IdSerialManager.Instance.GetNextId();

            Connection.Account = resultAuth;
            Connection.Account.LastIp = Connection.Ip;
            var parameters = new Dictionary<string, object>
            {
                {"last_ip", Connection.Ip.ToString()},
                /*{"session_hash", ""},
                {"session_time", DateTime.UtcNow}*/
            };
            DatabaseManager.Instance.UpdateAccount(Connection.Account.Id, parameters);
            AuthAccountsManager.Instance.Add(Connection.Account, generatedKey);
            Connection.SendPacket(new AuthResult(Connection.Account.Id, generatedKey));
        }
    }
}