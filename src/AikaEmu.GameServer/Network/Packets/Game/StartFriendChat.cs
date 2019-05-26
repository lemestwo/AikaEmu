using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class StartFriendChat : GamePacket
    {
        private readonly uint _friendId;

        public StartFriendChat(ushort conId, uint friendId)
        {
            _friendId = friendId;
            Opcode = (ushort) GameOpcode.StartFriendChat;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_friendId);
            stream.Write(0);
            return stream;
        }
    }
}