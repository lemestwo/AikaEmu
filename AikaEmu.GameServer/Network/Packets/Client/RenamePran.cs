using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
	public class RenamePran : GamePacket
	{
		protected override void Read(PacketStream stream)
		{
			var slot = stream.ReadUInt32(); // slot?
			var name = stream.ReadString(16);
			var accId = stream.ReadUInt32(); // alawys 0 from client
		}
	}
}