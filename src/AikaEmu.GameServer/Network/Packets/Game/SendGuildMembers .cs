using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendGuildMembers : GamePacket
    {
        public SendGuildMembers(ushort conId)
        {
            Opcode = (ushort) GameOpcode.SendGuildMembers;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            // 10 members per packet
            // 64 bytes per member
            for (var i = 0; i < 10; i++)
            {
                stream.Write(0);
                stream.Write("Member Name", 20);
                // TODO
            }

            return stream;
        }
    }
}