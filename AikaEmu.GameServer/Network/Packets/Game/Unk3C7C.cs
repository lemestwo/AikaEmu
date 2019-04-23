using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class Unk3C7C : GamePacket
    {
        public Unk3C7C(ushort conId)
        {
            Opcode = (ushort) GameOpcode.Unk3C7C;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(1); // actions 1, 2, 3, 5
            stream.Write(0);
            stream.Write(30); // unk
            stream.Write(0);
            stream.Write(60); // unk
            stream.Write(0);
            return stream;
        }
    }
}
