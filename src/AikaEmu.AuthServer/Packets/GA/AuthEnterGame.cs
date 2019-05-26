using AikaEmu.AuthServer.Managers;
using AikaEmu.AuthServer.Network.GameServer;
using AikaEmu.AuthServer.Packets.AG;
using AikaEmu.Shared.Network;

namespace AikaEmu.AuthServer.Packets.GA
{
    public class AuthEnterGame : GameAuthPacket
    {
        protected override void Read(PacketStream stream)
        {
            var accId = stream.ReadUInt32();
            var user = stream.ReadString(32);
            var key = stream.ReadInt32();
            var hash = stream.ReadString(32);
            var gsId = stream.ReadByte();
            var conId = stream.ReadUInt32();

            var gsConn = AuthGameManager.Instance.GetGameServer(gsId);
            if (gsConn == null) return;

            var account = AuthAccountsManager.Instance.GetAccount(accId);
            if (account == null) return;

            if (account.Key != key || account.Account.User != user || account.Account.SessionHash != hash)
            {
                gsConn.Connection.SendPacket(new RequestEnterResult(accId, conId, 0));
            }
            else
            {
                gsConn.Connection.SendPacket(new RequestEnterResult(accId, conId, 1));
            }

            AuthAccountsManager.Instance.Remove(accId);
        }
    }
}