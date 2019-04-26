using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Models.CharacterM;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.GameServer
{
	public class GameConnection : BaseConnection
	{
		public Account Account { get; set; }
		public Character ActiveCharacter => Account.ActiveCharacter;
		public ushort Id { get; set; }

		public GameConnection(Session session) : base(session)
		{
		}

		public void OnDisconnect()
		{
			AccountsManager.Instance.RemoveAccount(Id);

			if (ActiveCharacter == null) return;
			
			ActiveCharacter.Save();
			WorldManager.Instance.Despawn(ActiveCharacter);
		}

		public void SendPacket(GamePacket packet)
		{
			packet.Connection = this;
			Session.SendPacket(packet.Encode());
		}
	}
}