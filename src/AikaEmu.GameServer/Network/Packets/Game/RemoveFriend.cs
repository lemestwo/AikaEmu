using AikaEmu.GameServer.Network.GameServer;
using AikaEmu.Shared.Network;

namespace AikaEmu.GameServer.Network.Packets.Game
{
    public class RemoveFriend : GamePacket
    {
        private readonly uint _friendId;

        public RemoveFriend(ushort conId, uint friendId)
        {
            _friendId = friendId;

            Opcode = (ushort) GameOpcode.RemoveFriend;
            SenderId = conId;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write(_friendId);
            return stream;
        }
    }
}