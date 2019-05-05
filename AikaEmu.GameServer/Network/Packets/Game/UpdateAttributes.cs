using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdateAttributes : GamePacket
    {
        private readonly Character _character;

        public UpdateAttributes(Character character)
        {
            _character = character;

            Opcode = (ushort) GameOpcode.UpdateAttributes;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_character.Attributes);
            stream.Write((ushort) 0);
            stream.Write(_character.AttrPoints); // attr point
            stream.Write(_character.SkillPoints); // skill point
            return stream;
        }
    }
}