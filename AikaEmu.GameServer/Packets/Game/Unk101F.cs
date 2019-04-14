using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class Unk101F : GamePacket
    {
        public Unk101F()
        {
            Opcode = (ushort) GameOpcode.Unk101F;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(new byte[]
            {
                0x6C, 0x1E, 0x00, 0x00, 0x80, 0x09, 0x00, 0x00,
            });
            return stream;
        }
    }
}