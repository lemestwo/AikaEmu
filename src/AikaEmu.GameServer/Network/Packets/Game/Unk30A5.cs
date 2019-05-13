using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class Unk30A5 : GamePacket
    {
        public Unk30A5()
        {
            Opcode = (ushort) GameOpcode.Unk30A5;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(1);
            stream.Write(1);
            stream.Write(1);
            return stream;
        }
    }
}