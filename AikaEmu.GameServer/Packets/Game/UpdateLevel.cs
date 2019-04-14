using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class UpdateLevel : GamePacket
    {
        private readonly Character _character;

        public UpdateLevel(Character character)
        {
            _character = character;
            Opcode = (ushort) GameOpcode.UpdateLevel;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(new byte[]
            {
                0x00, 0x00, 0xCC, 0xCC, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x01, 0x00, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
            });
//            stream.Write(_character.Level);
//            stream.WriteCc(2);
//            stream.Write(_character.Experience);
//            stream.Write((ushort) 1);
//            stream.WriteCc(6);
//            stream.Write(0L);
            return stream;
        }
    }
}