using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class Unk1086 : GamePacket
    {
        public Unk1086()
        {
            Opcode = (ushort) GameOpcode.Unk1086;
        }

        public override PacketStream Write(PacketStream stream)
        {
            for (var i = 0; i < 32; i++)
            {
                stream.Write(i == 2 ? 16 : 0);
            }

            return stream;
        }
    }
}