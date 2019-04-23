using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class Unk3F1B : GamePacket
    {
        public Unk3F1B()
        {
            Opcode = (ushort) GameOpcode.Unk3F1B;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(0);
            return stream;
        }
    }
}