using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class Unk3CBE : GamePacket
    {
        public Unk3CBE()
        {
            Opcode = (ushort) GameOpcode.Unk3CBE;
        }

        public override PacketStream Write(PacketStream stream)
        {
            return stream;
        }
    }
}
