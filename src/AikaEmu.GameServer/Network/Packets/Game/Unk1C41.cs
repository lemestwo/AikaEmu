using AikaEmu.GameServer.Models.Account;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class Unk1C41 : GamePacket
	{
		private readonly Account _acc;

		public Unk1C41(Account acc)
		{
			_acc = acc;
			Opcode = (ushort) GameOpcode.Unk1C41;
			SenderId = 0;
		}

		public override PacketStream Write(PacketStream stream)
		{
			// Every map change triggers this packet
			stream.Write(_acc.DbId);
			stream.Write("", 16);

			for (ushort i = 0; i < 12; i++)
			{
				stream.Write((ushort) 0);
				stream.Write(i);
				stream.Write("", 36);
			}

			return stream;
		}
	}
}