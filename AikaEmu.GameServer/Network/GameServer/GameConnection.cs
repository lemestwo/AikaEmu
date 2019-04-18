using AikaEmu.GameServer.Models;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.GameServer
{
	public class GameConnection : BaseConnection
	{
		public Account Account { get; set; }
		public Character ActiveCharacter => Account.ActiveCharacter;
		public ushort ConnectionId { get; set; }

		public GameConnection(Session session) : base(session)
		{
		}

		public void SendPacket(GamePacket packet)
		{
			packet.Connection = this;
			Session.SendPacket(packet.Encode());
		}
	}
}