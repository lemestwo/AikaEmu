using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class Unk30A6 : GamePacket
    {
        public Unk30A6()
        {
            Opcode = (ushort) GameOpcode.Unk30A6;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write("", 12);
            return stream;
        }
    }
}