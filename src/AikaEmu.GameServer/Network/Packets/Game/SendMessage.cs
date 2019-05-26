using AikaEmu.GameServer.Models.Chat;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendMessage : GamePacket
    {
        private readonly Message _message;

        public SendMessage(Message message, ushort sender = AikaEmu.GameServer.GameServer.SystemSenderMsg)
        {
            _message = message;

            Opcode = (ushort) GameOpcode.SendMessage;
            SenderId = sender;

            // NOTE 
            // If sender == 30003 or 30005 intead of default 30000
            // client do more unknown things
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_message);
            return stream;
        }
    }
}