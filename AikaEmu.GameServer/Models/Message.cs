using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models
{
	public enum MessageSender : byte
	{
		System = 0x10
	}

	public enum MessageType : byte
	{
		Normal = 0x00,
		Error = 0x01
	}

	public class Message : BasePacket
	{
		private readonly MessageSender _sender;
		private readonly MessageType _type;
		private readonly string _message;

		public Message(MessageSender sender, MessageType type, string message)
		{
			_sender = sender;
			_type = type;
			_message = message;
		}

		public override PacketStream Write(PacketStream stream)
		{
			stream.Write((byte) 0); // unk
			stream.Write((byte) _sender);
			stream.Write((byte) _type);
			stream.Write((byte) 0); // unk
			stream.Write(_message, 128, _type == MessageType.Error);
			return stream;
		}
	}
}