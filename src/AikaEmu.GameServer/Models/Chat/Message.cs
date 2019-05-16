using AikaEmu.GameServer.Models.Chat.Const;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models.Chat
{
    public class Message : BasePacket
    {
        private readonly string _message;
        private readonly MessageType _type;
        private readonly MessageSender _sender;

        public Message(string message, MessageType type = MessageType.Normal, MessageSender sender = MessageSender.System)
        {
            _message = message;
            _type = type;
            _sender = sender;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((byte) 0); // unk
            stream.Write((byte) _sender);
            stream.Write((byte) _type);
            stream.Write((byte) 0); // unk
            stream.Write(_message, 126, _type == MessageType.Error);
            stream.Write((short) 0); // 2 empty bytes
            return stream;
        }
    }
}