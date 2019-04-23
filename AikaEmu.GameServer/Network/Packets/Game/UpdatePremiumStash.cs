using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class UpdatePremiumStash : GamePacket
	{
		public UpdatePremiumStash()
		{
			Opcode = (ushort) GameOpcode.UpdatePremiumStash;
			SenderId = 0;
		}

		public override PacketStream Write(PacketStream stream)
		{
			// TODO
			for (var i = 0; i < 48; i++)
			{
				stream.Write(0);
			}

			return stream;
		}
	}
}