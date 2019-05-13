using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models.Chat
{
    public enum MessageSender : byte
    {
        System = 16,
        Unk1 = 32,

        Unk2 = 4,
    }

    public enum MessageType : byte
    {
        Normal = 0,
        Error = 1,
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
            stream.Write(_message, 126, _type == MessageType.Error);
            stream.Write((short) 0); // 2 empty bytes
            return stream;
        }
    }
}