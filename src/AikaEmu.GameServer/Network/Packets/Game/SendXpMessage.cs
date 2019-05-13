using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class SendXpMessage : GamePacket
	{
		public SendXpMessage()
		{
			Opcode = (ushort) GameOpcode.SendXpMessage;
			SenderId = 0;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(0); // xp
			stream.Write(0); // additional xp (x + y Experience)
			stream.Write(0); // battle points
			return stream;
		}
	}
}