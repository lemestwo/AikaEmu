using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdateSiegeInfo : GamePacket
	{
		public UpdateSiegeInfo()
		{
			Opcode = (ushort) GameOpcode.UpdateSiegeInfo;
			SenderId = 0;
		}

		public override PacketStream Write(PacketStream stream)
		{
			// 4 defense guilds
			// 4 attack guilds gate 1
			// 4 attack guilds gate 2
			// 4 attack guilds gate 3
			// 4 attack guilds gate 4
			for (var i = 0; i < 20; i++)
				stream.Write("Guild" + (i + 1), 20);

			return stream;
		}
	}
}