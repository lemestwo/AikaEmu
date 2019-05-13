using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendChatMessage : GamePacket
    {
        private readonly ChatMessage _message;

        public SendChatMessage(ChatMessage message)
        {
            _message = message;
            Opcode = (ushort) GameOpcode.SendChatMessage;
            SenderId = message.OwnerConId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_message);
            return stream;
        }
    }
}