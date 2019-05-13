using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Client
{
	public class SetActiveTitle : GamePacket
	{
		protected override void Read(PacketStream stream)
		{
			var titleId = stream.ReadUInt32();
			Log.Debug("SetActiveTitle, Id: {0}", titleId);
		}
	}
}