using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestFriendResult : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var result = stream.ReadUInt16() == 1;
            var id = stream.ReadUInt16();
            // var name = stream.ReadString(16);

            FriendHelper.FriendRequestResult(Connection, result, id);
        }
    }
}