using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendGuildMemberOff : GamePacket
    {
        public SendGuildMemberOff()
        {
            Opcode = (ushort) GameOpcode.SendGuildMemberOff;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(0); // characterDbId
            stream.Write(0); // TODO - EPOCH TIME
            return stream;
        }
    }
}