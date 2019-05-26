using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
	public class InGameState : GamePacket
	{
		protected override void Read(PacketStream stream)
		{
			var character = Connection.ActiveCharacter;
			
		}
	}
}