using AikaEmu.GameServer.Models.Chat.Const;
using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models.Chat
{
    public class ChatMessage : BasePacket
    {
        public readonly ushort OwnerConId;

        private readonly int _unk;
        private readonly string _senderName;
        private readonly string _msg;
        private readonly ChatMessageType _type;
        private readonly ChatMessageTitle _govType;
        private readonly int _unk2;

        public ChatMessage(ushort ownerConId, int unk, string senderName, string msg, ChatMessageType type, ChatMessageTitle govType, int unk2)
        {
            OwnerConId = ownerConId;
            _unk = unk;
            _senderName = senderName;
            _msg = msg;
            _type = type;
            _govType = govType;
            _unk2 = unk2;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((short) _type);
            stream.Write((short) _govType);
            stream.Write(_unk2);
            stream.Write(_unk);
            stream.Write(_senderName, 16);
            stream.Write(_msg, 128);
            return stream;
        }
    }
}