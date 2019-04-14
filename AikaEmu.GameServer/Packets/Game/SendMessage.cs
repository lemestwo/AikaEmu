using AikaEmu.GameServer.Models;
using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Packets.Game
{
    public class SendMessage : GamePacket
    {
        private readonly Message _message;

        public SendMessage(Message message)
        {
            _message = message;
            Opcode = (ushort) GameOpcode.SendMessage;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_message);
            return stream;
        }
    }
}