using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.GameServer.Network.Packets.Game;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
	public class RequestUpdateCash : GamePacket
	{
		protected override void Read(PacketStream stream)
		{
			// TODO
			Connection.SendPacket(new UpdateCash(100000));
		}
	}
}