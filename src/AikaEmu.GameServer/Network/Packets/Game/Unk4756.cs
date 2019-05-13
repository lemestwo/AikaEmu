using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class Unk4756 : GamePacket
    {
        public Unk4756()
        {
            Opcode = (ushort) GameOpcode.Unk4756;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(new byte[]
            {
                0x77, 0x9C, 0xBF, 0x06, 0x2B, 0xFD, 0xCB, 0xCC,
            });
            return stream;
        }
    }
}