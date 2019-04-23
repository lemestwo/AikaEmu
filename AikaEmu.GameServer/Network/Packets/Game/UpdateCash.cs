using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdateCash : GamePacket
	{
		private readonly uint _amount;

		public UpdateCash(uint amount)
		{
			_amount = amount;

			Opcode = (ushort) GameOpcode.UpdateCash;
			SenderId = 0;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(_amount);
			return stream;
		}
	}
}