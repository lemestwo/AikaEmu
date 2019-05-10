using AikaEmu.GameServer.Network;
using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class SendRequestFriend : GamePacket
    {
        public SendRequestFriend()
        {
            Opcode = (ushort) GameOpcode.SendRequestFriend;
            SenderId = 0;
        }

        public override PacketStream Write(PacketStream stream)
        {
            var a = 74;
            stream.Write(a); // conId?
            stream.Write("FriendName", 16);
            return stream;
        }
    }
}