using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class UpdateSkills : GamePacket
    {
        private readonly Character _character;

        public UpdateSkills(Character character)
        {
            _character = character;

            Opcode = (ushort) GameOpcode.UpdateSkills;
            SenderId = character.Connection.Id;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_character.Skills.WriteSkills());
            stream.Write(_character.SkillPoints);
            stream.WriteCc(2);
            return stream;
        }
    }
}