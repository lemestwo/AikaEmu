using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
	public class UpdateExperience : GamePacket
	{
		private readonly Character _character;
		private readonly ushort _test;

		public UpdateExperience(Character character, ushort test)
		{
			_character = character;
			_test = test;

			Opcode = (ushort) GameOpcode.UpdateExperience;
			SenderId = character.ConnectionId;
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