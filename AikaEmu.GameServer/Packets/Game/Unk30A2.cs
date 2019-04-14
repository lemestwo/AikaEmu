using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class Unk30A2 : GamePacket
    {
        public Unk30A2()
        {
            Opcode = (ushort) GameOpcode.Unk30A2;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(0);
            stream.Write(0);
            return stream;
        }
    }
}