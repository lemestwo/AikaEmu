using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class Unk106F : GamePacket
    {
        public Unk106F()
        {
            Opcode = (ushort) GameOpcode.Unk106F;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(new byte[]
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            });
            return stream;
        }
    }
}