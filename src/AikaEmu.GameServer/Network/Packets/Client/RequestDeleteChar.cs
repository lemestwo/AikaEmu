using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
    public class RequestDeleteChar : GamePacket
    {
        protected override void Read(PacketStream stream)
        {
            var accId = stream.ReadUInt32();
            var slot = stream.ReadInt32();
            var unk = stream.ReadInt32();
            var token = stream.ReadString(4);
        }
    }
}