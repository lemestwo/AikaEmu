using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class UpdateHpMp : GamePacket
    {
        private readonly Character _character;

        public UpdateHpMp(Character character)
        {
            _character = character;
            Opcode = (ushort) GameOpcode.UpdateHpMp;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(new byte[]
            {
                0x2E, 0x01, 0x00, 0x00, 0x2E, 0x01, 0x00, 0x00, 0x21, 0x02, 0x00, 0x00,
                0x21, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            });
//            stream.Write(_character.Hp);
//            stream.Write(_character.Hp); // TODO MAX
//            stream.Write(_character.Mp);
//            stream.Write(_character.Mp); // TODO MAX
//            stream.Write(0);
            return stream;
        }
    }
}