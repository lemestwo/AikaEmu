using AikaEmu.Shared.Model.Network;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Models
{
    public class Message : BasePacket
    {
        private readonly byte _type1;
        private readonly byte _type2;
        private readonly string _message;

        public Message(byte type1, byte type2, string message)
        {
            _type1 = type1;
            _type2 = type2;
            _message = message;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((byte) 0);
            stream.Write(_type1);
            stream.Write(_type2);
            stream.Write((byte) 0);
            stream.Write(_message, 128, true);
            return stream;
        }
    }
}