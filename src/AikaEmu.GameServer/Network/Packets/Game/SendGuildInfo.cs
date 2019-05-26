using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendGuildInfo : GamePacket
    {
        public SendGuildInfo(ushort conId)
        {
            Opcode = (ushort) GameOpcode.SendGuildInfo;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            return stream;
        }
    }
}
