using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class Unk30A2 : GamePacket
    {
        public Unk30A2(ushort conId)
        {
            Opcode = (ushort) GameOpcode.Unk30A2;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(0);
            stream.Write(0);
            return stream;
        }
    }
}