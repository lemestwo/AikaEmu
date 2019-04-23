using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;
using RegisterGs = AikaEmu.GameServer.Network.Packets.GA.RegisterGs;

namespace AikaEmu.GameServer.Network.AuthServer
{
	public class AuthGameConnection : BaseConnection
	{
		public AuthGameConnection(Session session) : base(session)
		{
		}

		public void OnConnect()
		{
			// TODO - KEY FOR INTERNAL REGISTRATION
			SendPacket(new RegisterGs(AppConfigManager.Instance.GameServerConfig.Id));
		}

		public void SendPacket(AuthGamePacket packet)
		{
			Session.SendPacket(packet.Encode());
		}
	}
}