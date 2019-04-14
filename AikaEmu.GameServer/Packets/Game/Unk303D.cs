using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class Unk303D : GamePacket
    {
        public Unk303D()
        {
            Opcode = (ushort) GameOpcode.Unk303D;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(new byte[]
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x12, 0x00, 0x41, 0x00,
                0x9E, 0x00, 0xC2, 0x00, 0x08, 0x01, 0x15, 0x01, 0x70, 0x01, 0x9D, 0x01,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x09, 0x02, 0x09, 0x0B, 0x0B, 0x0B,
                0x09, 0x0B, 0x00, 0x00,
            });
//            stream.Write(0);
//            stream.Write(1);
//            stream.Write("", 32);
            return stream;
        }
    }
}