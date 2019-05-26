using AikaEmu.GameServer.Models.Units.Character;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdateExperience : GamePacket
	{
		private readonly Character _character;

		public UpdateExperience(Character character)
		{
			_character = character;

			Opcode = (ushort) GameOpcode.UpdateExperience;
			SenderId = character.Connection.Id;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(_character.Level);
			stream.WriteCc(2);
			stream.Write(_character.Experience);
			stream.Write((ushort) 1); // unk - always 1
			stream.WriteCc(6);
			stream.Write(0L);
			return stream;
		}
	}
}