using AikaEmu.GameServer.Models.Char;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class UpdateAttributes : GamePacket
    {
        private readonly CharAttributes _attr;

        public UpdateAttributes(CharAttributes attr)
        {
            _attr = attr;
            Opcode = (ushort) GameOpcode.UpdateAttributes;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(new byte[]
            {
                0x07, 0x00, 0x0A, 0x00, 0x0F, 0x00, 0x09, 0x00, 0x09, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
            });
//            stream.Write(_attr);
//            stream.Write((ushort) 0);
//            stream.Write(0);
            return stream;
        }
    }
}