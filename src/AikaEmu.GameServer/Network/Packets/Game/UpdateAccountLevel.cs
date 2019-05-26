using AikaEmu.GameServer.Models.Account;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdateAccountLevel : GamePacket
	{
		private readonly Account _acc;

		public UpdateAccountLevel(Account acc)
		{
			_acc = acc;

			Opcode = (ushort) GameOpcode.UpdateAccountLevel;
			SenderId = acc.Connection.Id;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write((uint) _acc.Level);
			return stream;
		}
	}
}