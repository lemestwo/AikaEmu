using AikaEmu.GameServer.Managers;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Client
{
	public class IngameState : GamePacket
	{
		protected override void Read(PacketStream stream)
		{
			WorldManager.Instance.ShowVisibleUnits(Connection.ActiveCharacter);
		}
	}
}