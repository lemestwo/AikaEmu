using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdateDungeonTimer : GamePacket
	{
		public UpdateDungeonTimer()
		{
			Opcode = (ushort) GameOpcode.UpdateDungeonTimer;
			SenderId = 0; 
		}

		public override PacketStream Write(PacketStream stream)
		{
			for (var i = 0; i < 38; i++)
			{
				stream.Write(0);
			}

			return stream;
		}
	}
}