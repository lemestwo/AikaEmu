using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
	public class UpdateAccountLevel : GamePacket
	{
		private readonly Account _acc;

		public UpdateAccountLevel(Account acc)
		{
			_acc = acc;

			Opcode = (ushort) GameOpcode.UpdateAccountLevel;
			SenderId = acc.ConnectionId;
		}

		public override PacketStream Write(PacketStream stream)
		{
			SenderId = Connection.ConnectionId;

			stream.Write(_acc.Level);
			return stream;
		}
	}
}