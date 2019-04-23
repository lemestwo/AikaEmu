using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
	public class SendMessage : GamePacket
	{
		private readonly Message _message;

		public SendMessage(Message message, ushort sender = 0)
		{
			_message = message;

			Opcode = (ushort) GameOpcode.SendMessage;
			SenderId = sender != 0 ? sender : AikaEmu.GameServer.GameServer.SystemSenderMsg;
			
			// NOTE 
			// If sender == 30003 or 30005 indead of default 30000
			// client do more unknown things
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write(_message);
			return stream;
		}
	}
}