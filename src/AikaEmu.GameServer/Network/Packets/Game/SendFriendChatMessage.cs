using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendFriendChatMessage : GamePacket
    {
        private readonly string _name;
        private readonly string _msg;

        public SendFriendChatMessage(ushort senderConId, string name, string msg)
        {
            _name = name;
            _msg = msg;
            Opcode = (ushort) GameOpcode.SendFriendChatMessage;
            SenderId = senderConId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_name, 16);
            stream.Write(_msg, 76);
            stream.Write("", 52);
            stream.Write(0);
            return stream;
        }
    }
}
