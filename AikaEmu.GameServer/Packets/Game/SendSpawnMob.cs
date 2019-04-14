using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class SendSpawnMob : GamePacket
    {
        public SendSpawnMob()
        {
            Opcode = (ushort) GameOpcode.SendSpawnMob;
        }

        public override PacketStream Write(PacketStream stream)
        {
            // 22 01 00 00 00 00 00 00 00 00 00 00 DE 05 00 00 66 E6 13 45 CD 8C 93 44 00 00 00 00 8E 16 00 00 00 00 00 00 8E 16 00 00 00 00 00 00 00 19 38 00 AE 00 00 00 00 00 00 00 00 07 77 77 00 00 05 07 E2 02 00 00 04 00 00 00
            return stream;
        }
    }
}