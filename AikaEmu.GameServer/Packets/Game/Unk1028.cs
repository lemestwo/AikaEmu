using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class Unk1028 : GamePacket
    {
        public Unk1028()
        {
            Opcode = (ushort) GameOpcode.Unk1028;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(new byte[]
            {
                0x00, 0x05, 0x14, 0x09, 0x10, 0x27, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            });
            return stream;
        }
    }
}