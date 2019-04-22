using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Client
{
	public class RequestUpdateCash : GamePacket
	{
		protected override void Read(PacketStream stream)
		{
			// TODO
			Connection.SendPacket(new UpdateCash(Connection.Id));
		}
	}
}