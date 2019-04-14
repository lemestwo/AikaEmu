using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class Unk102C : GamePacket
    {
        public Unk102C()
        {
            Opcode = (ushort) GameOpcode.Unk102C;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write("", 84);
            return stream;
        }
    }
}