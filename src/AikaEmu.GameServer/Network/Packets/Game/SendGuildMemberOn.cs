using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendGuildMemberOn : GamePacket
    {
        public SendGuildMemberOn()
        {
            Opcode = (ushort) GameOpcode.SendGuildMemberOn;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(0); // dbId
            stream.Write(1); // status?
            return stream;
        }
    }
}
