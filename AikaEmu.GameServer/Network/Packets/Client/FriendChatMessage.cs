using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class FriendChatMessage : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var name = stream.ReadString(16);
            var msg = stream.ReadString(76);
            stream.ReadBytes(52);
            var unk = stream.ReadInt32(); // <= 5 triggers client msg
            
            // TODO
        }
    }
}