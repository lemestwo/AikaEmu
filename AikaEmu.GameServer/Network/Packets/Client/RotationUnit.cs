using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
	public class RotationUnit : GamePacket
	{
		protected override void Read(PacketStream stream)
		{
			var rotation = stream.ReadInt32();

			if (rotation > 360 || rotation < 0) rotation = 0;
			Connection.ActiveCharacter.SetRotation(rotation);
		}
	}
}