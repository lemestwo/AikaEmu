using AikaEmu.GameServer.Managers.Configuration;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.GA;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
	public class RequestEnterGame : GamePacket
	{
		protected override void Read(PacketStream stream)
		{
			var accId = stream.ReadUInt32();
			var user = stream.ReadString(32);
			var key = stream.ReadInt32();
			var mac = stream.ReadMacAddress();
			stream.ReadInt64();
			var netVersion = stream.ReadUInt16();
			stream.ReadInt32();
			var clientKey1 = stream.ReadString(12);
			var clientKey2 = stream.ReadString(12);
			var hash = stream.ReadString(32);
			Log.Info("RequestEnterGame, User: ({1}) {0}", user, accId);

			// TODO - Check netVersion and clientKeys
			// INFO - Maybe can use Mac
			AikaEmu.GameServer.GameServer.AuthGameConnection.SendPacket(new AuthEnterGame(accId, user, key, hash, AppConfigManager.Instance.GameServerConfig.Id,
				Connection.SessionId));
		}
	}
}