using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.CharacterM;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdateCharGold : GamePacket
	{
		private readonly Character _character;

		public UpdateCharGold(Character character)
		{
			_character = character;

			Opcode = (ushort) GameOpcode.UpdateCharGold;
			SenderId = character.ConnectionId;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.WriteCc(4);

			stream.Write(_character.Money);
			stream.Write(_character.BankMoney);

			return stream;
		}
	}
}