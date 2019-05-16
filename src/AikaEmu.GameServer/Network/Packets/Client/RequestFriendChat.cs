using AikaEmu.GameServer.Helpers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestFriendChat : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var friendId = stream.ReadUInt32();
            // stream.ReadUInt32();
            FriendHelper.FriendRequestChat(Connection, friendId);
        }
    }
}