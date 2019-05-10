using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestFriendResult : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var result = stream.ReadUInt16() == 1;
            var id = stream.ReadUInt16();
            var name = stream.ReadString(16);
            Log.Debug("RequestFriendResult, Result: {0}, Id: {1}, Name: {2}", result, id, name);
            Connection.SendPacket(new SendFriendInfo(Connection.Id));
        }
    }
}